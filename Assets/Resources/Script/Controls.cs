using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public float Speed = 120.0f;

    private GameObject obj;
    private GameObject cakeObj;
    private GameObject menuObj;

    private bool flag = false;

    //blade
    public GameObject Blade;
    public GameObject LightSaber;
    public GameObject Shuriken;

    public AudioSource[] LightSaber_AudioSources = {null, null};
    public AudioSource[] Shuriken_AudioSources = { null };

    private int counter = 0;

    private bool _shouldRotate = false;
    private bool _zoomed = false;

    public ToolUser BladeTool;

    void Start()
    {
        obj = Resources.Load<GameObject>("Prefabs/Cube");
        cakeObj = Resources.Load<GameObject>("Prefabs/Ski");
        menuObj = Resources.Load<GameObject>("Prefabs/Menu");
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

        if (Input.GetButtonDown("Back"))
        {
            var pos = transform.position;
            pos += transform.forward * 5;
            pos.y = 0.0f;

            var menuItem = Instantiate(menuObj, pos, Quaternion.identity);
            menuItem.transform.rotation = Quaternion.Euler(-90, 0, 0);
        }

        // Switch Blade
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            BladeTool.UseShuriken = !BladeTool.UseShuriken;
            LightSaber.SetActive(!BladeTool.UseShuriken);
            Shuriken.SetActive(BladeTool.UseShuriken);

            PlayActivateLightSaber();
        }

        // Zoom Blade
        Zoom();

        // Rotate blade
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
            {
                _shouldRotate = true;
                PlayHitLightSaber();
                PlayShurikenThrow();
            }

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
        int angle = 165;
        float speed = 0.65f;

        if (_shouldRotate)
            LightSaber.transform.rotation = Quaternion.Slerp(LightSaber.transform.rotation, transform.parent.rotation * Quaternion.Euler(-angle, 180, -180), speed);
        else
            LightSaber.transform.rotation = Quaternion.Slerp(LightSaber.transform.rotation, transform.parent.rotation * Quaternion.Euler(-90, 180, -180), speed);

        if (LightSaber.transform.rotation == transform.parent.rotation * Quaternion.Euler(-angle, 180, -180))
            _shouldRotate = false;
    }

    void Zoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            _zoomed = true;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            _zoomed = false;
        }

        if (_zoomed) Camera.main.fieldOfView = 30;
        else Camera.main.fieldOfView = 82;
    }

    void PlayActivateLightSaber()
    {
        LightSaber_AudioSources[0].Play();
    }

    void PlayHitLightSaber()
    {
        LightSaber_AudioSources[1].Play();
    }

    void PlayShurikenThrow()
    {
        Shuriken_AudioSources[0].Play();
    }
}
