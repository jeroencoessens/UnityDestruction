using UnityEngine;
using System.Collections;

public class RotateSimple : MonoBehaviour {

    public float Speed = 25;
    private bool Stop = false;
	
	// Update is called once per frame
	void Update ()
	{
	    if (Input.GetButtonDown("Space"))
	        Stop = !Stop;
        
	    float multiplier = -Input.GetAxis("Horizontal");

	    if (!Stop)
	    {
            if (multiplier == 0)
                multiplier = 0.2f;
        }
        
        transform.Rotate(0, Time.deltaTime * Speed * multiplier, 0);
	}
}
