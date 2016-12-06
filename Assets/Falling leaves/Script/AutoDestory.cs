using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class AutoDestory : MonoBehaviour {
	public float delaytime = 0.5f;
	
	void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}
	
	IEnumerator CheckIfAlive ()
	{
		while(true)
		{
			yield return new WaitForSeconds(delaytime);
			if(!GetComponent<ParticleSystem>().IsAlive(true))
				this.gameObject.SetActive(false);

		}
	}
}
