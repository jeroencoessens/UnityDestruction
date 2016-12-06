using System;
using UnityEngine;
using System.Collections;
using BLINDED_AM_ME;

public class ToolUser : MonoBehaviour {

	private Material _capMaterial;
    private GameObject _shuriken;

    // should make mesh collider ( max 255 poly convex hull )
    public Vector3 forceRightObj = Vector3.zero;
    public bool AddMeshCollider = false;

    public bool UseShuriken = false;

    void Start()
    {
        _capMaterial = Resources.Load<Material>("Materials/matte 10");
        _shuriken = Resources.Load<GameObject>("Prefabs/Shuriken");
    }
	
	void Update(){

        if (Input.GetMouseButtonDown(0))
        {
            if (UseShuriken)
            {
                var shuriken = Instantiate(_shuriken, transform.position, transform.rotation);
                shuriken.transform.Translate(0.075f, 0, 0);
                shuriken.transform.Rotate(0, 0, 90);
                shuriken.GetComponent<Rigidbody>().velocity = transform.forward * 80;
            }
            StartCoroutine(CutMesh());
        }
    }

    private IEnumerator CutMesh()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            // object to slice
            GameObject victim = hit.collider.gameObject;

            //conditions
            if (UseShuriken)
            {
                if (hit.distance > 150) yield break;
                yield return new WaitForSeconds(hit.distance / 200);
            }
            else if (!UseShuriken)
            {
                if (hit.distance > 6) yield break;
            }

            if (victim.tag == "NoSlicing") yield break;

            string victimName = victim.name;

            // create array with 2 sides
            // pieces[0] is left side and is modified original
            // pieces[1] is right side and will bounce of 
            //--------------------------------------------------------------

            // ++ PROFILE ++
            //float timeStored = DateTime.Now.Second;
            //Profiler.BeginSample("Mesh Cutter Profiled");
            //System.GC.Collect();

            // -- MESH CUT --
            GameObject[] pieces = null;
            yield return MeshCut.Cut(victim, transform.position, transform.right, _capMaterial, (obj) =>
            {
                pieces = obj;
            });

            // ++ PROFILE ++
            //float timeReal = DateTime.Now.Second - timeStored;
            //Debug.Log(victimName + " took " + Mathf.Abs(timeReal).ToString("F0") + " seconds to calculate");
            //Profiler.EndSample();

            //--------------------------------------------------------------

            // translate?
            pieces[1].transform.Translate(forceRightObj * 0.01f);
            
            // scale correctly
            pieces[0].transform.localScale = victim.transform.localScale;
            pieces[1].transform.localScale = victim.transform.localScale;

            // change name ( convenient )
            pieces[0].name = victimName + " - L-Side";
            pieces[1].name = victimName + " - R-Side";

            // Add rigidbodies and colliders
            if (pieces[0].GetComponent<Rigidbody>())
            {
                pieces[0].GetComponent<Rigidbody>().isKinematic = false;
                pieces[0].GetComponent<Rigidbody>().useGravity = true;
            }

            if (!pieces[1].GetComponent<Rigidbody>())
                pieces[1].AddComponent<Rigidbody>();
            
            // move right piece
            if(forceRightObj != Vector3.zero)
                pieces[1].GetComponent<Rigidbody>().AddForce(forceRightObj);

            // Delete box collider from original ( modified ) left side
            if (pieces[0].GetComponent<BoxCollider>())
                Destroy(pieces[0].GetComponent<BoxCollider>());

            if (pieces[0].GetComponent<MeshCollider>())
                Destroy(pieces[0].GetComponent<MeshCollider>());

            var skinWidth = 0.1f;

            // Add Colliders
            if (AddMeshCollider)
            {
                pieces[0].AddComponent<MeshCollider>().convex = true;
                pieces[1].AddComponent<MeshCollider>().convex = true;

                var rightMc = pieces[1].GetComponent<MeshCollider>();
                rightMc.inflateMesh = true;
                rightMc.skinWidth = skinWidth;
            }
            else
            {
                pieces[0].AddComponent<BoxCollider>();
                pieces[1].AddComponent<BoxCollider>();
            }

            // Set as highlightable object
            pieces[1].AddComponent<HightLightedObject>();

            //Debug.Log("Components " + Resources.FindObjectsOfTypeAll(typeof(Rigidbody)).Length);
        }
    }

	void OnDrawGizmosSelected()
    {
		Gizmos.color = Color.red;

		Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5.0f);
		Gizmos.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.5f + transform.forward * 5.0f);
		Gizmos.DrawLine(transform.position + -transform.up * 0.5f, transform.position + -transform.up * 0.5f + transform.forward * 5.0f);

		Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.5f);
		Gizmos.DrawLine(transform.position,  transform.position + -transform.up * 0.5f);
	}
}
