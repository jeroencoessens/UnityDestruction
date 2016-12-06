using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCaster : MonoBehaviour {
	
	void Update () {
        
	    RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.gameObject.tag == "Highlightable")
            {
                hit.collider.GetComponent<HightLightedObject>().Highlighted = true;
            }
        }
    }
}
