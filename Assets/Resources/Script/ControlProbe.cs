using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlProbe : MonoBehaviour
{
    public GameObject plane;
    public RenderTexture texture;
    public GameObject character;

    private float offset;
    private Direction directionFaced;

    private enum Direction
    {
        X,Y,Z
    }

    private ReflectionProbe probe;
	
	// Update is called once per frame
	void Update () {

	    if (directionFaced == Direction.X)
	    {
	        offset = (plane.transform.position.x - character.transform.position.x);

	        transform.position = new Vector3(
                plane.transform.position.x + offset, 
                character.transform.position.y, 
                character.transform.position.z);
        }

        if (directionFaced == Direction.Y)
        {
            offset = (plane.transform.position.y - character.transform.position.y);

            transform.position = new Vector3(
                character.transform.position.x,
                plane.transform.position.z + offset,
                character.transform.position.z);
        }

        if (directionFaced == Direction.Z)
        {
            offset = (plane.transform.position.z - character.transform.position.z);

            transform.position = new Vector3(
                character.transform.position.x,
                character.transform.position.y,
                plane.transform.position.z + offset);
        }

	    probe.RenderProbe(texture);

	}
}
