using UnityEngine;
using System.Collections;

public class RotateSimple : MonoBehaviour {

    public float Speed = 25;
     
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, Time.deltaTime * Speed, 0);
	}
}
