using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public float Speed = 120.0f;

    private GameObject obj;
    private GameObject cakeObj;

    private bool flag = false;

    //blade
    public GameObject Blade;
    public GameObject LightSaber;
    private int counter = 0;

    private bool _shouldRotate = false;

    void Start()
    {
        obj = Resources.Load<GameObject>("Prefabs/Cube");
        cakeObj = Resources.Load<GameObject>("Prefabs/Ski");
    }

    void Update () {

        if (!flag)
        {
            if (Input.GetKey(KeyCode.F)) // dpawn collider to push away rigids
            {
                var cube = Instantiate(obj, transform.position, Quaternion.identity);
                cube.tag = "NoSlicing";
                cube.GetComponent<Rigidbody>().velocity = transform.forward * Speed;
                flag = true;
                StartCoroutine(Wait());
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) // spawn object to slice
        {
            var pos = transform.position;
            pos += transform.forward * 15;
            if (pos.y < 2.5f) pos.y = 2.5f;

            var cake = Instantiate(cakeObj, pos, transform.rotation);
            cake.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            cake.AddComponent<HightLightedObject>();
        }
        
        //Rotate blade
        var speed = 100.0f;
        var max = 28;
        
        if (Input.GetKey(KeyCode.Q) && counter < max)
        {
            Blade.transform.Rotate(0, 0, Time.deltaTime * speed);
            LightSaber.transform.Rotate(0, -Time.deltaTime * speed, 0);
            ++counter;
        }
        else if (Input.GetKey(KeyCode.E) && counter > -max)
        {
            Blade.transform.Rotate(0, 0, -Time.deltaTime * speed);
            LightSaber.transform.Rotate(0, Time.deltaTime * speed, 0);

            --counter;
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && !_shouldRotate)
                _shouldRotate = true;

            RotateSaber();
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.08f);
        flag = false;
    }

    void RotateSaber()
    {
        int angle = 160;
        float speed = 0.6f;

        if (_shouldRotate)
            LightSaber.transform.rotation = Quaternion.Slerp(LightSaber.transform.rotation, transform.parent.rotation * Quaternion.Euler(-angle, 180, -180), speed);
        else
            LightSaber.transform.rotation = Quaternion.Slerp(LightSaber.transform.rotation, transform.parent.rotation * Quaternion.Euler(-90, 180, -180), speed);

        if (LightSaber.transform.rotation == transform.parent.rotation * Quaternion.Euler(-angle, 180, -180))
            _shouldRotate = false;
    }
}
