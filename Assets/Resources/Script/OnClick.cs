using UnityEngine;
using System.Collections;

public class OnClick : MonoBehaviour {

    // speed to go to in slow motion
    public float slowMotionSpeed = 0.05f;

    // bool to check if time should be slowed
    private bool changeTime;

    // scale of object spawned ( fractured )
    public float scale = 1.0f;
    
    // scale of collider sphere to control fragment spread
    public float scaleSphere = 4.0f;

    // path to grab prefab from
    public string brokenPath = "Prefabs/DestroyedTeapot";

    // path to hover material
    private Material _newMaterial;

    private Material _thisMaterial;
    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _thisMaterial = _renderer.material;
        _newMaterial = Resources.Load<Material>("Materials/smooth 5");
    }
	
	// Update is called once per frame
	void Update () {

        // slow time when click event is called
	    if (changeTime)
	    {
	        if (Time.timeScale > slowMotionSpeed)
	        {
                Time.timeScale -= Time.deltaTime * 2.5f;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
            }
	        else
                changeTime = false;
        }
    }

    void OnMouseOver()
    {
        _renderer.material = _newMaterial;
    }

    void OnMouseExit()
    {
        _renderer.material = _thisMaterial;
    }

    void OnMouseDown()
    {
        // spawn fractured object and scale to correct size
        var fractured = Instantiate(Resources.Load(brokenPath), transform.position, transform.rotation) as GameObject;
        fractured.transform.localScale = new Vector3(scale, scale, scale);

        // spawn collider sphere and scale to correct size
        var sphere = Instantiate(Resources.Load("Prefabs/ColliderSphere"), transform.position, transform.rotation) as GameObject;
        sphere.transform.localScale = new Vector3(scaleSphere, scaleSphere, scaleSphere);

        // hide current object
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;

        // bool to change time
        changeTime = true;

        // destroy objects in order
        Destroy(sphere, 0.1f);
        Destroy(fractured, 1.8f);
        Destroy(gameObject, 2.0f);
    }
}
