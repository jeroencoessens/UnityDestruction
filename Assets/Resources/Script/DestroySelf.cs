using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float Timer = 3.0f;

    void Start()
    {
        Invoke("DestroyMe", Timer);
    }

    public void DontDestroy()
    {
        CancelInvoke("DestroyMe");
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
