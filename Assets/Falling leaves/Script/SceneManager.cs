using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour {
	public GameObject Parent;
	public GameObject GridGO;
	public bool Bgrid = true, BslowDown = false;
	public List<string> childname = new List<string>();

	public GameObject PSTextname;
	public int currentPSGO = 0;

	public GameObject PrePSGO, NexPSGO;
	void Start () {
		for (int i =0; i< Parent.transform.childCount; i++)
			childname.Add (Parent.transform.GetChild (i).name);
		NexPSGO = Parent.transform.GetChild (0).gameObject;
		PSPlay ();
	}
	void Update () 
	{
		int position = childname[currentPSGO].IndexOf("[");
		if (position > 0)
			PSTextname.GetComponent<Text> ().text = childname [currentPSGO].Substring (0, position);
		else
			PSTextname.GetComponent<Text> ().text = childname [currentPSGO];
	}
	public void PSPlay()
	{
		if (PrePSGO != null) {
			PrePSGO.SetActive (false);
			StopParticle (PrePSGO);
		}

		NexPSGO.SetActive (true);
		PlayParticle (NexPSGO);
	}
	public void PSPause()
	{
		PauseParticle (NexPSGO);
	}
	public void PSStop()
	{
		StopParticle (NexPSGO);
	}
	// Play
	public void PlayParticle(GameObject PSGO)
	{
		ParticleSystem[] ParticleSys;
		ParticleSys = PSGO.GetComponents<ParticleSystem>();
		foreach (ParticleSystem temp in ParticleSys) {
			temp.Simulate (0, true, true);
			temp.Play();
		}
	}
	// Pause
	public void PauseParticle(GameObject PSGO)
	{
		ParticleSystem[] ParticleSys;
		ParticleSys = PSGO.GetComponents<ParticleSystem>();
		foreach(ParticleSystem temp in ParticleSys)
			temp.Pause();
	}
	// Stop
	public void StopParticle(GameObject PSGO)
	{
		ParticleSystem[] ParticleSys;
		ParticleSys = PSGO.GetComponents<ParticleSystem>();			
		foreach (ParticleSystem temp in ParticleSys) {
			temp.Clear ();
			temp.Stop();
		}
		PauseParticle (NexPSGO);
	}
	public void NextPSGO()
	{
		PrePSGO = Parent.transform.GetChild(currentPSGO).gameObject;
		currentPSGO++;
		if (currentPSGO > Parent.transform.childCount-1)
			currentPSGO = 0;
		NexPSGO= Parent.transform.GetChild(currentPSGO).gameObject;
		PSPlay ();
	}
	public void PrevPSGO()
	{
		PrePSGO = Parent.transform.GetChild(currentPSGO).gameObject;
		currentPSGO--;
		if (currentPSGO <= 0)
			currentPSGO = Parent.transform.childCount - 1;
		NexPSGO= Parent.transform.GetChild(currentPSGO).gameObject;
		PSPlay ();
	}
	public void ShowGrid()
	{
		Bgrid = !Bgrid;
		GridGO.SetActive (Bgrid);
	}
	public void SlowDown()
	{
		BslowDown = !BslowDown;
		if (BslowDown)
			Time.timeScale = 0.5f;
		else
			Time.timeScale = 1.0f;
	}
}
