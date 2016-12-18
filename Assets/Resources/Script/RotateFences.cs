using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFences : MonoBehaviour
{
    private bool _hasRotated = false;
    private bool _touchedBox = false;

    public Transform LeftFence;
    public Transform RightFence;

    public float Speed = 0.1f;
    
	void Update () {
	    if (!_hasRotated && _touchedBox)
	    {
            LeftFence.rotation = Quaternion.Lerp(LeftFence.rotation, Quaternion.Euler(0, -75, 0), Speed);
            RightFence.rotation = Quaternion.Lerp(RightFence.rotation, Quaternion.Euler(0, 75, 0), Speed);
        }

	    if (LeftFence.rotation == Quaternion.Euler(0, -75, 0) && RightFence.rotation == Quaternion.Euler(0, 75, 0))
	        _hasRotated = true;

	}

    void OnTriggerEnter(Collider other)
    {
        if (_hasRotated) return;
        if (other.name == "PLAYER")
        {
            _touchedBox = true;
        }
    }
}
