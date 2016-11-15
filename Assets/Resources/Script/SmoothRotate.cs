using UnityEngine;
using System.Collections;

public class SmoothRotate : MonoBehaviour {

	void Update ()
	{
        // rotate camera ( not smooth atm )
	    transform.Rotate(0, Time.deltaTime * 25, 0);
	}
}
