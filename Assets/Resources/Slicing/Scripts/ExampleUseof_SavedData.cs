using UnityEngine;
using System.Collections;
using BLINDED_AM_ME;

public class ExampleUseof_SavedData : MonoBehaviour {

	public bool   isEncrypted  = true;
	public string dataPassword = "no_suck_luck";
	public string dataFileName = "saveData.txt";
	public string dataDestination = "Application.persistentDataPath";
	public SaveData.StringFormat dataFormat = SaveData.StringFormat.json;
	public SaveData data;

	// Use this for initialization
	void Start () {
	
		data = new SaveData(
			password:dataPassword,
			destination:dataDestination,
			fileName:dataFileName,
			isEncrypted:isEncrypted,
			format: dataFormat
		);

		int numBombs = int.Parse(data.Get_Value("numBombs", "0"));
		Debug.Log(numBombs.ToString());

		numBombs = 5;
		data.Set_Value("numBombs", numBombs.ToString());

		data.Save_Data();

		DontDestroyOnLoad(gameObject);

	}


	public void OnApplicationPause(bool pauseStatus) {
		if(pauseStatus)
			data.Save_Data();
	}
	public void OnApplicationQuit(){
		data.Save_Data();
	}
}
