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
    private GameObject MenuItem;

    private bool _hasPressedR = false;
    private List<GameObject> _boulderList;
    private int _boulderCounter = 0;

    void Start()
    {
        obj = Resources.Load<GameObject>("Prefabs/Cube");
        cakeObj = Resources.Load<GameObject>("Prefabs/Ski");
        menuObj = Resources.Load<GameObject>("Prefabs/Menu");

        _boulderList = new List<GameObject>();
    }

    void Update () {

        if (!flag)
        {
            if (Input.GetKey(KeyCode.F)) // spawn collider to push away rigids
            {
                var cube = Instantiate(obj, transform.position, Quaternion.identity);
                cube.tag = "NoSlicing";
                cube.GetComponent<Rigidbody>().velocity = transform.forward * Speed;
                flag = true;
                StartCoroutine(Wait());
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            var pos = transform.position;
            pos += transform.forward * 10;
            if (pos.y < 2.5f) pos.y = 2.5f;

            if (Input.GetKeyDown(KeyCode.R) && !_hasPressedR) // spawn object to slice
            {
                ++_boulderCounter;
                _boulderList.Add(Instantiate(cakeObj, pos, transform.rotation));
            }

            _boulderList[_boulderCounter - 1].transform.rotation = Quaternion.Euler(60, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            _boulderList[_boulderCounter - 1].transform.position = pos;
            _hasPressedR = true;
        }
        else
            _hasPressedR = false;

        if (Input.GetButtonDown("Back"))// spawn menu item
        {
            var pos = transform.position;
            pos += transform.forward * 4;
            pos.y = 0.0f;

            if (MenuItem == null)
            {
                MenuItem = Instantiate(menuObj, pos, Quaternion.identity);
            }

            MenuItem.transform.position = pos;

            var rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            MenuItem.transform.rotation = rotation * Quaternion.Euler(-90, 0, 136);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2)) // switch blade
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

        Camera.main.fieldOfView = _zoomed ? 30 : 82;
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
