using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepRotating : MonoBehaviour
{
    private Transform thisTransform;

    void Start()
    {
        thisTransform = GetComponent<Transform>();
    }

    void Update()
    {
        thisTransform.Rotate(0, -Time.deltaTime*800, 0);
    }
}
