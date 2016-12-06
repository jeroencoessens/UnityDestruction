using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountSimple : MonoBehaviour
{
    private Text counter;
    
	void Start ()
	{
	    counter = GetComponent<Text>();
	}
	
	void Update ()
	{
	    counter.text = Resources.FindObjectsOfTypeAll(typeof(Rigidbody)).Length.ToString();

        // Clear rigidbodies on command
	}
}
