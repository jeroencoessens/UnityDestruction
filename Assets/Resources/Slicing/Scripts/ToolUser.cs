using System;
using UnityEngine;
using System.Collections;
using BLINDED_AM_ME;
using UnityEngine.UI;
using System.Threading;
using CielaSpike;

public class ToolUser : MonoBehaviour {

	public Material capMaterial;
    public Text SecondsToCalculate;
    public GameObject FollowUpGameObject;

    // Multi-threading... -> no transform :/

    // should make mesh collider ( max 255 poly convex hull )
    public Vector3 forceRightObj = Vector3.zero;
    public bool AddMeshCollider = false;

    void Start()
    {
        if(FollowUpGameObject != null)FollowUpGameObject.SetActive(false);
    }
	
	void Update(){

		if(Input.GetMouseButtonDown(0))
        {
            // I want this function on a different thread
            // but I need transform
            CutMesh();
        }
	}

    private void CutMesh()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            // object to slice
            GameObject victim = hit.collider.gameObject;
            string victimName = victim.name;

            // create array with 2 sides
            // pieces[0] is left side and is modified original
            // pieces[1] is right side and will bounce of 
            //--------------------------------------------------------------

            // ++ PROFILE ++
            float timeStored = DateTime.Now.Second;

            // -- MESH CUT --
            GameObject[] pieces = MeshCut.Cut(victim, transform.position, transform.right, capMaterial);

            // ++ PROFILE ++
            float timeReal = DateTime.Now.Second - timeStored;
            Debug.Log(victimName + " took " + Mathf.Abs(timeReal).ToString("F0") + " seconds to calculate");
            if (SecondsToCalculate != null) SecondsToCalculate.text = Mathf.Abs(timeReal).ToString("F0") + " sec.";

            //--------------------------------------------------------------

            // translate?
            pieces[1].transform.Translate(forceRightObj * 0.01f);
            
            // scale correctly
            pieces[0].transform.localScale = victim.transform.localScale;
            pieces[1].transform.localScale = victim.transform.localScale;

            // change name ( convenient )
            pieces[0].name = victimName + " - left-side";
            pieces[1].name = victimName + " - right-side";

            // Add rigidbodies and colliders
            if (!pieces[1].GetComponent<Rigidbody>())
                pieces[1].AddComponent<Rigidbody>();
            
            // move right piece
            if(forceRightObj != Vector3.zero)
                pieces[1].GetComponent<Rigidbody>().AddForce(forceRightObj);

            // Delete box collider from original ( modified ) left side
            if (pieces[0].GetComponent<BoxCollider>())
                Destroy(pieces[0].GetComponent<BoxCollider>());

            // Add Coliiders
            if (AddMeshCollider)
            {
                pieces[0].AddComponent<MeshCollider>().convex = true;

                if (!pieces[1].GetComponent<MeshCollider>())
                    pieces[1].AddComponent<MeshCollider>().convex = true;
            }
            else
            {
                pieces[0].AddComponent<BoxCollider>();

                if (!pieces[1].GetComponent<BoxCollider>())
                    pieces[1].AddComponent<BoxCollider>();
            }

            // Enable FollowUpGameObject
            if (FollowUpGameObject != null) FollowUpGameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

	void OnDrawGizmosSelected() {

		Gizmos.color = Color.yellow;

		Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5.0f);
		Gizmos.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.5f + transform.forward * 5.0f);
		Gizmos.DrawLine(transform.position + -transform.up * 0.5f, transform.position + -transform.up * 0.5f + transform.forward * 5.0f);

		Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.5f);
		Gizmos.DrawLine(transform.position,  transform.position + -transform.up * 0.5f);
	}

    // Threading with Ciela - Ninja
    IEnumerator CutMeshRoutine()
    {
        Task task;
        this.StartCoroutineAsync(Cutting(), out task);
        yield return StartCoroutine(task.Wait());
    }

    IEnumerator Cutting()
    {
        yield return Ninja.JumpBack;
        CutMesh();
        yield return Ninja.JumpToUnity;
    }
}
