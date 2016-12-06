using UnityEngine;
using System.Collections;

public class SmoothRotate : MonoBehaviour {

    public float Speed = 25;

    void Update()
    {
        transform.Rotate(Time.deltaTime * Speed * Input.GetAxis("Vertical"), 0, 0);
    }
}
