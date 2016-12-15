using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class HightLightedObject : MonoBehaviour
{
    public bool Highlighted = false;
    public bool ShouldUseGravity = true;

    private Renderer renderer;
    private Shader shader1;
    private Shader shader2;

    void Start ()
	{
	    renderer = GetComponent<Renderer>();
	    tag = "Highlightable";

        shader1 = Shader.Find("Toon/Lit");
        shader2 = Shader.Find("Toon/Lit Outline");

        // Add colliders
	    if (!GetComponent<MeshCollider>())
	    {
	        gameObject.AddComponent<MeshCollider>().convex = true;
	        GetComponent<MeshCollider>().inflateMesh = true;
	    }

	    if (!GetComponent<Rigidbody>())
	    {
	        gameObject.AddComponent<Rigidbody>().useGravity = ShouldUseGravity;
            GetComponent<Rigidbody>().isKinematic = !ShouldUseGravity;
        }
    }

    void LateUpdate()
    {
        //if (Highlighted)
        //{
        //    if (renderer.material.shader != shader2)
        //        renderer.material.shader = shader2;
        //}
        //else
        //{
        //    if (renderer.material.shader != shader1)
        //        renderer.material.shader = shader1;
        //}

        Highlighted = false;
    }
}
