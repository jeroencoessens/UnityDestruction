using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

namespace BLINDED_AM_ME{

	public class SaveData{

		public enum StringFormat{
			json,
			xml
		}
		
		public string secretPassword,fileDestination,fileName;
		public bool   isEncrypted = true;
		public StringFormat format    = StringFormat.json;


		private Dictionary<string, object> allData;

		// constructors
		public SaveData (
			string password    = "no_such_luck",
			string destination = "Application.persistentDataPath",
			string fileName    = "Saved_Data.txt",
			bool   isEncrypted = true,
			StringFormat format = StringFormat.json)
		{
			this.secretPassword = password;
			this.fileDestination = destination;
			this.fileName        = fileName;
			this.format          = format;
			this.isEncrypted     = isEncrypted;
			this.Init();
		}


		private void Init(){

			allData = new Dictionary<string, object>();
			allData.Add("emptyObj", "0");

			if(fileDestination.Equals("Application.persistentDataPath"))
				fileDestination = Application.persistentDataPath;

			Load_Data();
		}

		/// <summary>
		/// Call this at the beginning of the App's opening
		/// </summary>
		public void Load_Data(){

			string finalOutcome = "";
			string line = "";

			if(System.IO.File.Exists(fileDestination+"/"+fileName)){

				//Pass the file path and file name to the StreamReader constructor
				using (StreamReader sr = new StreamReader(fileDestination+"/"+fileName)){

					//Read the first line of text
					line = sr.ReadLine();

					//Continue to read until you reach end of file
					while (line != null) 
					{
						finalOutcome += line;
						//Read the next line
						line = sr.ReadLine();
					}

					//close the file
					sr.Close();

					if(!isEncrypted){

						if(format == StringFormat.json)
							allData = MiniJSON.Json.Deserialize(finalOutcome) as Dictionary<string,object>;
						else
							allData = XML_Deserialize(finalOutcome);

					}else{

						byte[] theKeyBytes  = Encoding.UTF8.GetBytes(secretPassword);
						byte[] theDataBytes = Convert.FromBase64String(finalOutcome);

						// decipher
						int tempInt = 0;
						for(int i=0; i<theDataBytes.Length; i++){

							tempInt = (int) theDataBytes[i];
							tempInt -= (int) theKeyBytes[i % theKeyBytes.Length];
							if(tempInt < 0)// aka negative
								tempInt += 256;
							theDataBytes[i] = (byte) tempInt;
						}

						string theDecodedString = Encoding.UTF8.GetString(theDataBytes);

						if(format == StringFormat.json)
							allData = MiniJSON.Json.Deserialize(theDecodedString) as Dictionary<string,object>;
						else
							allData = XML_Deserialize(theDecodedString);

					}

				}

			}else{

				allData = new Dictionary<string, object>();
				allData.Add("emptyObj", "0");
			}

		}


		/// <summary>
		/// Saves the data to destination.
		/// </summary>
		/// <param name="destination">Destination.</param>
		/// <param name="fileName">File name.</param>
		public void Save_Data(){


			if(!isEncrypted){

				string theString = "";

				if(format == StringFormat.json)
					theString = MiniJSON.Json.Serialize(allData);
				else
					theString = XML_Serialize(allData);

				// check if destination exists
				if(!System.IO.Directory.Exists(fileDestination))
					System.IO.Directory.CreateDirectory(fileDestination);


				//Pass the filepath and filename to the StreamWriter Constructor
				using(StreamWriter sw = new StreamWriter(fileDestination + "/" + fileName)){
					sw.Write(theString);
					//Close the file
					sw.Close();
				}	

			}else{

				byte[] theKeyBytes  = Encoding.UTF8.GetBytes(secretPassword);
				byte[] theDataBytes;
				if(format == StringFormat.json)
					theDataBytes = Encoding.UTF8.GetBytes(MiniJSON.Json.Serialize(allData));
				else
					theDataBytes = Encoding.UTF8.GetBytes(XML_Serialize(allData));
				

				int tempInt = 0;  // byte can equal 0 - 255
				for(int i=0; i<theDataBytes.Length; i++){
								
						tempInt = (int) theDataBytes[i];
						tempInt += (int) theKeyBytes[i % theKeyBytes.Length];
						tempInt = tempInt  % 256;
						theDataBytes[i] = (byte) tempInt;
				}
				
				string theEncodedString = Convert.ToBase64String(theDataBytes, Base64FormattingOptions.InsertLineBreaks);

				// check if destination exists
				if(!System.IO.Directory.Exists(fileDestination))
					System.IO.Directory.CreateDirectory(fileDestination);


				//Pass the filepath and filename to the StreamWriter Constructor
				using(StreamWriter sw = new StreamWriter(fileDestination + "/" + fileName)){
					sw.Write(theEncodedString);
					//Close the file
					sw.Close();
				}	
		
			}
		}

		public void Set_Value(string key, string value){

			if(allData.ContainsKey(key)){
				allData[key] = value;
			}else{
				allData.Add(key, value);
			}
		}
					
		public string Get_Value(string key, string defualtValue){

			if(allData.ContainsKey(key)){
					return allData[key].ToString();
			}else{
					return defualtValue;
			}
		}


		// XML


		public static string XML_Serialize(Dictionary<string, object> dict) 
		{ 

			// it won't do Dictionary so make it a List

			MemoryStream memory = new MemoryStream(); 
			XmlTextWriter writer = new XmlTextWriter(memory, Encoding.UTF8); 
			XmlSerializer serializer = new XmlSerializer(typeof(List<string>)); 

			List<string> list = new List<string>();
			// convert dictionary to list manually
			foreach(var item in dict)
			{
				list.Add(item.Key);
				list.Add(item.Value.ToString());
			}

			serializer.Serialize(writer, list); 
			memory = (MemoryStream)writer.BaseStream; 

			return Encoding.UTF8.GetString(memory.ToArray()); 

		} 

		public static Dictionary<string, object> XML_Deserialize(string dataString) 
		{ 

			MemoryStream memory = new MemoryStream(Encoding.UTF8.GetBytes(dataString));
			XmlTextWriter writer = new XmlTextWriter(memory, Encoding.UTF8); 
			XmlSerializer serializer = new XmlSerializer(typeof(List<string>)); 

			List<string> list = (List<string>) serializer.Deserialize(memory);
			Dictionary<string, object> dict = new Dictionary<string, object>();

			for(int i=0; i<list.Count; i+=2){

				dict.Add(list[i], list[i+1]);
			}

			return dict;
		} 



	}
}