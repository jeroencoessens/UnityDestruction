using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLookat : MonoBehaviour
{
    private GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("FPSController");
    }
	
	// Update is called once per frame
	void Update ()
	{
	    transform.LookAt(player.transform.rotation.eulerAngles);
	}
}
