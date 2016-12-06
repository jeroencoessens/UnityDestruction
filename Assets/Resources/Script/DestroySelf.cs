using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float timer = 3.0f;

    void Update () {
		Destroy(gameObject, timer);
	}
}
