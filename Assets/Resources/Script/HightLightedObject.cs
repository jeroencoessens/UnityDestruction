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

    private List<AudioClip> clips;

    void Start ()
	{
	    renderer = GetComponent<Renderer>();
	    tag = "Highlightable";

        shader1 = Shader.Find("Toon/Lit");
        shader2 = Shader.Find("Toon/Lit Outline");

        clips = new List<AudioClip>();

        //clips.Add(Resources.Load<AudioClip>("Sounds/thud"));
        //clips.Add(Resources.Load<AudioClip>("Sounds/thud2"));
        //clips.Add(Resources.Load<AudioClip>("Sounds/thud3"));
        //clips.Add(Resources.Load<AudioClip>("Sounds/thud4"));
        //clips.Add(Resources.Load<AudioClip>("Sounds/thud5"));
        //
        //gameObject.AddComponent<AudioSource>().volume = 0.8f;
        //gameObject.AddComponent<HitSomething>().clips = clips;

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
