                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              #pragma strict

class Mega_Toon_FX extends EditorWindow {
	
		var Particle_Scale : float = 0.0;
		var Particle_Duration : float = 0.0;
		var Particle_Speed : float = 0.0;
		var Particle_Delay : float = 0.0;
		var loop : boolean = false;
		var Prewarm : boolean = false;
		var AffectChild : boolean = true;
		var StartColor : Color = Color.white;
		
		@MenuItem("Mega Toon FX/Particle Editor")
		
		static function Init() {
			var window = GetWindow(Mega_Toon_FX);
			window.position = Rect(0,0,425,435);
			window.Show();
		}
		
		function OnGUI() {
			//GUILayout.BeginArea(new Rect(0,0,this.position.width - 8,this.position.height));
			
			GUILayout.Space(15);
			
			GUILayout.Label("Universal Particle Editor", EditorStyles.boldLabel);
			GUILayout.BeginHorizontal();	
			GUILayout.Label("Select a particle to changes it properties", EditorStyles.miniLabel);
			AffectChild = EditorGUILayout.Toggle("Apply Setting to childrens", AffectChild);
			GUILayout.EndHorizontal();
			//Scale
			GUILayout.Box("",GUILayout.Width(this.position.width - 12), GUILayout.Height(3));
			GUILayout.BeginHorizontal();						
			if(GUILayout.Button(new GUIContent("Apply Scaler", "It will scale the Particle System"),GUILayout.Width(100),GUILayout.Height(30)))
			{
				ScaleParticle();
			}
			Particle_Scale = EditorGUILayout.Slider ("", Particle_Scale, 0, 100);
			//Particle_Scale = EditorGUI.Slider(Rect(120,70,150,20),Particle_Scale,0, 100);			
			GUILayout.EndHorizontal();
			GUILayout.Label("Just Multiply the scale value with the current size of the selected particle", EditorStyles.miniLabel);
			GUILayout.Box("",GUILayout.Width(this.position.width - 12), GUILayout.Height(3));
			
			// Duration
			GUILayout.BeginHorizontal();						
			if(GUILayout.Button(new GUIContent("Apply Duration", "Set the Duration of the selected particle"),GUILayout.Width(100),GUILayout.Height(30)))
			{
				Durationvalue();
			}
			Particle_Duration = EditorGUILayout.Slider ("", Particle_Duration, 0, 100);
			//Particle_Duration = EditorGUI.Slider(Rect(120,130,150,20),Particle_Duration,0, 100);
			GUILayout.EndHorizontal();
			GUILayout.Label("Duration of the particle system", EditorStyles.miniLabel);
			GUILayout.Box("",GUILayout.Width(this.position.width - 12), GUILayout.Height(3));
			
			// Speed
			GUILayout.BeginHorizontal();						
			if(GUILayout.Button(new GUIContent("Apply Speed", "Set the Speed of the selected particle"),GUILayout.Width(100),GUILayout.Height(30)))
			{
				SetSpeed();
			}
			Particle_Speed = EditorGUILayout.Slider ("", Particle_Speed, 0, 100);
			//Particle_Speed = EditorGUI.Slider(Rect(120,190,150,20),Particle_Speed,0, 100);
			GUILayout.EndHorizontal();
			GUILayout.Label("Speed of the particle system", EditorStyles.miniLabel);
			GUILayout.Box("",GUILayout.Width(this.position.width - 12), GUILayout.Height(3));
			
			// Delay
			GUILayout.BeginHorizontal();						
			if(GUILayout.Button(new GUIContent("Apply Delay", "Set the Delay of the selected particle"),GUILayout.Width(100),GUILayout.Height(30)))
			{
				Delaytheparticle();
			}
			Particle_Delay = EditorGUILayout.Slider ("", Particle_Delay, 0, 100);
			//Particle_Delay = EditorGUI.Slider(Rect(120,250,150,20),Particle_Delay,0, 100);
			GUILayout.EndHorizontal();
			GUILayout.Label("Delay of the particle system", EditorStyles.miniLabel);
			GUILayout.Box("",GUILayout.Width(this.position.width - 12), GUILayout.Height(3));
			
		
			GUILayout.BeginHorizontal();						
			loop = EditorGUILayout.Toggle("Loop Selected Particle", loop);
			Prewarm = EditorGUILayout.Toggle("Prewarm Selected Particle", Prewarm);
			
		
			GUILayout.EndHorizontal();
			if(GUILayout.Button(new GUIContent("Apply", "Apply Loop and Prewarm to selected particle"),GUILayout.Width(100),GUILayout.Height(30)))
			{
				prewarmvalue(Prewarm);	
				loopvalue(loop);
			}
		
			GUILayout.Box("",GUILayout.Width(this.position.width - 12), GUILayout.Height(3));
			GUILayout.BeginHorizontal();	
			if(GUILayout.Button(new GUIContent("Play", "Play the selected particle"),GUILayout.Width(100),GUILayout.Height(30)))
			{
				PlayParticle();
			}	
			if(GUILayout.Button(new GUIContent("Pause", "Pause the selected particle"),GUILayout.Width(100),GUILayout.Height(30)))
			{
				PauseParticle();
			}	
			if(GUILayout.Button(new GUIContent("Stop", "Stop the selected particle"),GUILayout.Width(100),GUILayout.Height(30)))
			{
				StopParticle();
			}
			if(GUILayout.Button(new GUIContent("Clear", "Clear the selected particle"),GUILayout.Width(100),GUILayout.Height(30)))
			{	
				ClearParticle();
			}		
			GUILayout.EndHorizontal();
			GUILayout.Box("",GUILayout.Width(this.position.width - 12), GUILayout.Height(3));
			GUILayout.BeginHorizontal();
			if(GUILayout.Button(new GUIContent("More Asset", ""),GUILayout.Width(100),GUILayout.Height(30)))
			{
				Application.OpenURL ("https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:431");
			}	
			if(GUILayout.Button(new GUIContent("Mega Toon Fx Pack1", ""),GUILayout.Width(150),GUILayout.Height(30)))
			{
				Application.OpenURL ("https://www.assetstore.unity3d.com/#!/content/51195");
			}
			GUILayout.EndHorizontal();			
			GUILayout.Box("",GUILayout.Width(this.position.width - 12), GUILayout.Height(3));
	
		}
		function OnInspectorUpdate() {	
			//prewarmvalue(Prewarm);					
		}
	
		//Scale the particle
		function ScaleParticle()
		{
			for(var obj:GameObject in Selection.gameObjects)
			{
				var ParticleSys : ParticleSystem[];
				if(AffectChild)
					ParticleSys = obj.GetComponentsInChildren.<ParticleSystem>();
				else
					ParticleSys = obj.GetComponents.<ParticleSystem>();	
				
				for(var temp:ParticleSystem in ParticleSys)		
				{
					temp.startSize  = temp.startSize * Particle_Scale;			
					temp.gravityModifier = temp.gravityModifier * Particle_Scale;
					if(temp.startSpeed > 0.01f)
						temp.startSpeed = temp.startSpeed * Particle_Scale;
					if(temp.gameObject != obj)
						temp.transform.localPosition = temp.transform.localPosition * Particle_Scale;		
							
					Particlescaler(temp, obj);
				}
				
//				Scale Lights
				var PSlight : Light[];
				PSlight = obj.GetComponentsInChildren.<Light>();
				for(var light1:Light in PSlight)
				{
					light1.range *= Particle_Scale;
					light1.transform.localPosition *= Particle_Scale;
				}

			}
		}
		//Scale PArticleSystem
		function Particlescaler(ParticleGO:ParticleSystem,parent:GameObject)
		{
			var ParticleGOS : SerializedObject;
			ParticleGOS = new SerializedObject(ParticleGO);		
			
			//Velocity
			if(ParticleGOS.FindProperty("VelocityModule.enabled").boolValue)
			{
				ParticleGOS.FindProperty("VelocityModule.x.scalar").floatValue *= Particle_Scale;
				ScaleCurve(ParticleGOS.FindProperty("VelocityModule.x.minCurve").animationCurveValue);
				ScaleCurve(ParticleGOS.FindProperty("VelocityModule.x.maxCurve").animationCurveValue);
				ParticleGOS.FindProperty("VelocityModule.y.scalar").floatValue *= Particle_Scale;
				ScaleCurve(ParticleGOS.FindProperty("VelocityModule.y.minCurve").animationCurveValue);
				ScaleCurve(ParticleGOS.FindProperty("VelocityModule.y.maxCurve").animationCurveValue);
				ParticleGOS.FindProperty("VelocityModule.z.scalar").floatValue *= Particle_Scale;
				ScaleCurve(ParticleGOS.FindProperty("VelocityModule.z.minCurve").animationCurveValue);
				ScaleCurve(ParticleGOS.FindProperty("VelocityModule.z.maxCurve").animationCurveValue);
			}
			
			//Size By Speed
			if(ParticleGOS.FindProperty("SizeBySpeedModule.enabled").boolValue)
			{
				ParticleGOS.FindProperty("SizeBySpeedModule.range.x").floatValue *= Particle_Scale;
				ParticleGOS.FindProperty("SizeBySpeedModule.range.y").floatValue *= Particle_Scale;
			}
			
			//Limit Velocity
			if(ParticleGOS.FindProperty("ClampVelocityModule.enabled").boolValue)
			{
				ParticleGOS.FindProperty("ClampVelocityModule.x.scalar").floatValue *= Particle_Scale;
				ScaleCurve(ParticleGOS.FindProperty("ClampVelocityModule.x.minCurve").animationCurveValue);
				ScaleCurve(ParticleGOS.FindProperty("ClampVelocityModule.x.maxCurve").animationCurveValue);
				ParticleGOS.FindProperty("ClampVelocityModule.y.scalar").floatValue *= Particle_Scale;
				ScaleCurve(ParticleGOS.FindProperty("ClampVelocityModule.y.minCurve").animationCurveValue);
				ScaleCurve(ParticleGOS.FindProperty("ClampVelocityModule.y.maxCurve").animationCurveValue);
				ParticleGOS.FindProperty("ClampVelocityModule.z.scalar").floatValue *= Particle_Scale;
				ScaleCurve(ParticleGOS.FindProperty("ClampVelocityModule.z.minCurve").animationCurveValue);
				ScaleCurve(ParticleGOS.FindProperty("ClampVelocityModule.z.maxCurve").animationCurveValue);
				
				ParticleGOS.FindProperty("ClampVelocityModule.magnitude.scalar").floatValue *= Particle_Scale;
				ScaleCurve(ParticleGOS.FindProperty("ClampVelocityModule.magnitude.minCurve").animationCurveValue);
				ScaleCurve(ParticleGOS.FindProperty("ClampVelocityModule.magnitude.maxCurve").animationCurveValue);
			}
			
			//Force 
			if(ParticleGOS.FindProperty("ForceModule.enabled").boolValue)
			{
				ParticleGOS.FindProperty("ForceModule.x.scalar").floatValue *= Particle_Scale;
				ScaleCurve(ParticleGOS.FindProperty("ForceModule.x.minCurve").animationCurveValue);
				ScaleCurve(ParticleGOS.FindProperty("ForceModule.x.maxCurve").animationCurveValue);
				ParticleGOS.FindProperty("ForceModule.y.scalar").floatValue *= Particle_Scale;
				ScaleCurve(ParticleGOS.FindProperty("ForceModule.y.minCurve").animationCurveValue);
				ScaleCurve(ParticleGOS.FindProperty("ForceModule.y.maxCurve").animationCurveValue);
				ParticleGOS.FindProperty("ForceModule.z.scalar").floatValue *= Particle_Scale;
				ScaleCurve(ParticleGOS.FindProperty("ForceModule.z.minCurve").animationCurveValue);
				ScaleCurve(ParticleGOS.FindProperty("ForceModule.z.maxCurve").animationCurveValue);
			}
			
			//Emission 
			if(ParticleGOS.FindProperty("EmissionModule.enabled").boolValue && ParticleGOS.FindProperty("EmissionModule.m_Type").intValue == 1)
				ParticleGOS.FindProperty("EmissionModule.rate.scalar").floatValue /= Particle_Scale;
				
			
			ParticleGOS.ApplyModifiedProperties();
		}
		
		function ScaleCurve(curve : AnimationCurve)
		{
			for(var i = 0; i < curve.keys.Length; i++)
			{
				curve.keys[i].value *= Particle_Scale;
			}
		}

		// Play
		function PlayParticle()
		{
			for(var obj:GameObject in Selection.gameObjects)
			{
				var ParticleSys : ParticleSystem[];
				if(AffectChild)
					ParticleSys = obj.GetComponentsInChildren.<ParticleSystem>();
				else
					ParticleSys = obj.GetComponents.<ParticleSystem>();
				

				for(var temp:ParticleSystem in ParticleSys)
				{
					temp.Play();
				}
			}
		}
		// Pause
		function PauseParticle()
		{
			for(var obj:GameObject in Selection.gameObjects)
			{
				var ParticleSys : ParticleSystem[];
				if(AffectChild)
					ParticleSys = obj.GetComponentsInChildren.<ParticleSystem>();
				else
					ParticleSys = obj.GetComponents.<ParticleSystem>();
				
		
				for(var temp:ParticleSystem in ParticleSys)
				{
					temp.Pause();
				}
			}
		}
		// Stop
		function StopParticle()
		{
			for(var obj:GameObject in Selection.gameObjects)
			{
				var ParticleSys : ParticleSystem[];
				if(AffectChild)
					ParticleSys = obj.GetComponentsInChildren.<ParticleSystem>();
				else
					ParticleSys = obj.GetComponents.<ParticleSystem>();
			
				for(var temp:ParticleSystem in ParticleSys)
				{
					temp.Stop();
				}
			}
		}
		// Clear
		function ClearParticle()
		{
			for(var obj:GameObject in Selection.gameObjects)
			{
				var ParticleSys : ParticleSystem[];
				if(AffectChild)
					ParticleSys = obj.GetComponentsInChildren.<ParticleSystem>();
				else
					ParticleSys = obj.GetComponents.<ParticleSystem>();
				
				//Scale Lifetime
				for(var temp:ParticleSystem in ParticleSys)
				{
					temp.Clear();
				}
			}
		}
		// Speed
		function SetSpeed()
		{
			for(var obj:GameObject in Selection.gameObjects)
			{
				var ParticleSys : ParticleSystem[];
				if(AffectChild)
					ParticleSys = obj.GetComponentsInChildren.<ParticleSystem>();
				else
					ParticleSys = obj.GetComponents.<ParticleSystem>();
				
				//Scale Lifetime
				for(var temp:ParticleSystem in ParticleSys)
				{
					
					if(Particle_Speed != 0)
						temp.startSpeed = 100.0f/Particle_Speed;
					else
						temp.startSpeed =0;
				}
			}
		}
		// Delay
		function Delaytheparticle()
		{
			for(var obj:GameObject in Selection.gameObjects)
			{
				var ParticleSys : ParticleSystem[];
				if(AffectChild)
					ParticleSys = obj.GetComponentsInChildren.<ParticleSystem>();
				else
					ParticleSys = obj.GetComponents.<ParticleSystem>();
				
				//Scale Lifetime
				for(var temp:ParticleSystem in ParticleSys)
				{
					var ParticleGO : SerializedObject;
					ParticleGO = new SerializedObject(temp);					
					if(ParticleGO.FindProperty("prewarm").boolValue)
						this.ShowNotification(new GUIContent("Please Uncheck the prewarm to set the delay"));
					else
						temp.startDelay = Particle_Delay;
				}
			}
		}
		function Durationvalue()
		{
			for(var obj:GameObject in Selection.gameObjects)
			{
				var ParticleSys : ParticleSystem[];
				if(AffectChild)
					ParticleSys = obj.GetComponentsInChildren.<ParticleSystem>();
				else
					ParticleSys = obj.GetComponents.<ParticleSystem>();
				
				for(var temp:ParticleSystem in ParticleSys)
				{
					var ParticleGO : SerializedObject;
					ParticleGO = new SerializedObject(temp);					
					ParticleGO.FindProperty("lengthInSec").floatValue = Particle_Duration;					
					ParticleGO.ApplyModifiedProperties();
				}
			}
		}
		function prewarmvalue(val:boolean)
		{
			for(var obj:GameObject in Selection.gameObjects)
			{
				var ParticleSys : ParticleSystem[];
				if(AffectChild)
					ParticleSys = obj.GetComponentsInChildren.<ParticleSystem>();
				else
					ParticleSys = obj.GetComponents.<ParticleSystem>();
				
				for(var temp:ParticleSystem in ParticleSys)
				{
					var ParticleGO : SerializedObject;
					ParticleGO = new SerializedObject(temp);	
					if(!ParticleGO.FindProperty("looping").boolValue)
						this.ShowNotification(new GUIContent("Please enable the loop to set the prewarm"));
					else
					{				
						ParticleGO.FindProperty("prewarm").boolValue = val;					
						ParticleGO.ApplyModifiedProperties();
					}
				}
			}
		}
		function loopvalue(val:boolean)
		{
			for(var obj:GameObject in Selection.gameObjects)
			{
				var ParticleSys : ParticleSystem[];
				if(AffectChild)
					ParticleSys = obj.GetComponentsInChildren.<ParticleSystem>();
				else
					ParticleSys = obj.GetComponents.<ParticleSystem>();
				
				for(var temp:ParticleSystem in ParticleSys)
				{
					var ParticleGO : SerializedObject;
					ParticleGO = new SerializedObject(temp);					
					ParticleGO.FindProperty("looping").boolValue = val;					
					ParticleGO.ApplyModifiedProperties();
				}
			}
		}
			
	}