using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlTimeBasic : MonoBehaviour
{
    // speed to control time scaling
    public float speed = 5;

    // time scale canvas text holder
    public Text textTimeScale;

    // time scale value holder
    private float keepTimeScale;
	
	// Update is called once per frame
	void Update ()
	{
        // reset scene
        if (Input.GetKey(KeyCode.R))
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(0);
        }

        // stop time
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (Time.timeScale != 0.0f) keepTimeScale = Time.timeScale;
            Time.timeScale = 0.0f;
            Time.fixedDeltaTime = 0.0f;
        }

        // restore time scale
        if (Input.GetKeyDown(KeyCode.G))
        {
            Time.timeScale = keepTimeScale;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

        // delete fragments
	    if (Input.GetKey(KeyCode.D))
	    {
            if (GameObject.Find("DestroyedFragment(Clone)") != null)
                Destroy(GameObject.Find("DestroyedFragment(Clone)"));
        }

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
}
