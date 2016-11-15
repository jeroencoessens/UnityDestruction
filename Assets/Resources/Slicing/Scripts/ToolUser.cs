using UnityEngine;
using System.Collections;
using BLINDED_AM_ME;

public class ToolUser : MonoBehaviour {

	public Material capMaterial;
    public float Scale = 0.041f;
	
	void Update(){

		if(Input.GetMouseButtonDown(0))
        {
			RaycastHit hit;

			if(Physics.Raycast(transform.position, transform.forward, out hit))
            {
				GameObject victim = hit.collider.gameObject;

				GameObject[] pieces = MeshCut.Cut(victim, transform.position, transform.right, capMaterial);
                pieces[1].transform.localScale = new Vector3(Scale, Scale, Scale);
                pieces[0].transform.localScale = new Vector3(Scale, Scale, Scale);

                Rigidbody rigid0 = new Rigidbody();

                if (!pieces[1].GetComponent<Rigidbody>())
                    pieces[1].AddComponent<Rigidbody>();

                if (!pieces[0].GetComponent<Rigidbody>())
                    rigid0 = pieces[0].AddComponent<Rigidbody>();

                if (rigid0 != null)
                {
                    rigid0.useGravity = false;
                    rigid0.isKinematic = true;
                }

                BoxCollider meshCol0 = new BoxCollider();
                BoxCollider meshCol1 = new BoxCollider();

                if (!pieces[1].GetComponent<BoxCollider>())
                    meshCol1 = pieces[1].AddComponent<BoxCollider>();

                Destroy(pieces[1], 5);
			}
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
}
