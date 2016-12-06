using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using UnityStandardAssets.Water;

public class ControlTimeBasic : MonoBehaviour
{
    // speed to control time scaling
    public float speed = 5;

    // time scale canvas text holder
    public Text textTimeScale;

    // time scale value holder
    private float keepTimeScale;

    //camera
    public GameObject Camera;
    public GameObject Water;

    private bool _enabler = true;
    private bool enableEdgeDetection = false;

    private int textureSizeWater = 64;

    void Start()
    {
        textureSizeWater = Water.GetComponent<Water>().textureSize;
    }

    void DisableAllCameraScripts(bool enabler)
    {
        Camera.GetComponent<ScreenSpaceAmbientOcclusion>().enabled = enabler;
        Camera.GetComponent<DepthOfField>().enabled = enabler;
        Camera.GetComponent<CameraMotionBlur>().enabled = enabler;
        Camera.GetComponent<MotionBlur>().enabled = enabler;
        Camera.GetComponent<ContrastStretch>().enabled = enabler;
        Camera.GetComponent<Antialiasing>().enabled = enabler;
        Camera.GetComponent<Fisheye>().enabled = enabler;
        Camera.GetComponent<BloomOptimized>().enabled = enabler;
        Camera.GetComponent<PostEffectsBase>().enabled = enabler;

        var cachedWater = Water.GetComponent<Water>();

        if (enabler)
        {
            cachedWater.waterMode = UnityStandardAssets.Water.Water.WaterMode.Refractive;
            cachedWater.textureSize = 1024;
        }
        else
        {
            cachedWater.waterMode = UnityStandardAssets.Water.Water.WaterMode.Reflective;
            cachedWater.textureSize = 128;
        }
    }

    // Update is called once per frame
    void Update ()
	{
        InputHandler();

        // spawn new normal teapot
        if (Input.GetKeyDown(KeyCode.T))
            Instantiate(Resources.Load("Prefabs/NormalTeapot"), new Vector3(0, 0.4f, 0), Quaternion.Euler(-90, 0, 0));

        // time scale
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Time.timeScale < 1)
	    {
	        Time.timeScale += Time.deltaTime * speed / 2;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && Time.timeScale > 0)
	    {
	        Time.timeScale -= Time.deltaTime * speed;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

        // update text
	    textTimeScale.text = "Time Scale: " + Time.timeScale.ToString("F3") + " -- use mouse wheel to change";
    }

    void InputHandler()
    {
        // reset scene
        if (Input.GetKey(KeyCode.R))
        {
            Time.timeScale = 1.0f;
            Application.LoadLevel(0);
        }

        // stop time
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (Time.timeScale != 0.0f) keepTimeScale = Time.timeScale;
            Time.timeScale = 0.0f;
            Time.fixedDeltaTime = 0.0f;

            Camera.GetComponent<SepiaTone>().enabled = true;
        }

        // restore time scale
        if (Input.GetKeyDown(KeyCode.K))
        {
            Time.timeScale = keepTimeScale;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            Camera.GetComponent<SepiaTone>().enabled = false;
        }

        // delete fragments
        if (Input.GetKey(KeyCode.L))
        {
            if (GameObject.Find("DestroyedFragment(Clone)") != null)
                Destroy(GameObject.Find("DestroyedFragment(Clone)"));
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            _enabler = !_enabler;
            DisableAllCameraScripts(_enabler);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            enableEdgeDetection = !enableEdgeDetection;
            Camera.GetComponent<EdgeDetection>().enabled = enableEdgeDetection;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            if (textureSizeWater < 2048)
                textureSizeWater *= 2;

            Water.GetComponent<Water>().textureSize = textureSizeWater;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (textureSizeWater > 32)
                textureSizeWater /= 2;

            Water.GetComponent<Water>().textureSize = textureSizeWater;
        }
    }
}
