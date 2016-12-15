using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using BLINDED_AM_ME;

public class ToolUser : MonoBehaviour {

	private Material _capMaterial;
    private GameObject _shuriken;

    public Vector3 forceRightObj = Vector3.zero;
    public bool AddMeshCollider = false;

    public bool UseShuriken = false;

    //public values
    public float MaxDistanceShuriken = 180.0f;
    public float MaxDistanceLightSaber = 6.0f;
    public float WaitingTimeShuriken = 400.0f;
    public float WaitingTimeThreshold = 50.0f;
    public float ShurikenSpeed = 80.0f;
    public float ShurikenXPosition = 0.075f;
    public float MaxDistanceRayCast = 300.0f;
    public float SkinWidth = 0.1f;

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
                shuriken.transform.Translate(ShurikenXPosition, 0, 0);

                var shurikenObj = shuriken.transform.Find("Object");
                shurikenObj.GetComponent<Rigidbody>().velocity = transform.forward * ShurikenSpeed;

                StartCoroutine(CutMesh(shuriken.transform));
            }
            else
                StartCoroutine(CutMesh(transform));
        }
    }

    private IEnumerator CutMesh(Transform bladeTransform)
    {
        var maxDistance = MaxDistanceRayCast;
        if (!UseShuriken)
            maxDistance = MaxDistanceLightSaber * 2;

        var hits = Physics.RaycastAll(transform.position, transform.forward, maxDistance);
        hits.Reverse();

        foreach (var hit in hits)
        {
            // object to slice
            if (hit.collider == null) continue;
            var victim = hit.collider.gameObject;

            //conditions
            if (UseShuriken)
            {
                if (hit.distance > MaxDistanceShuriken) continue;

                if(hit.distance > WaitingTimeThreshold)
                    yield return new WaitForSeconds(hit.distance / WaitingTimeShuriken);
            }
            else if (!UseShuriken)
            {
                if (hit.distance > MaxDistanceLightSaber) continue;
            }

            if (victim.tag == "NoSlicing") continue;
            var victimName = victim.name;

            // -- MESH CUT --
            GameObject[] pieces = null;
            yield return MeshCut.Cut(victim, bladeTransform.position, bladeTransform.right, _capMaterial, (obj) =>
            {
                pieces = obj;
            });

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
            if (forceRightObj != Vector3.zero)
                pieces[1].GetComponent<Rigidbody>().AddForce(forceRightObj);

            // Delete collider from original ( modified ) left side
            if (pieces[0].GetComponent<BoxCollider>())
                Destroy(pieces[0].GetComponent<BoxCollider>());

            if (pieces[0].GetComponent<MeshCollider>())
                Destroy(pieces[0].GetComponent<MeshCollider>());

            // Add Colliders
            if (AddMeshCollider)
            {
                pieces[0].AddComponent<MeshCollider>().convex = true;
                pieces[1].AddComponent<MeshCollider>().convex = true;

                var rightMc = pieces[1].GetComponent<MeshCollider>();
                rightMc.inflateMesh = true;
                rightMc.skinWidth = SkinWidth;
            }
            else
            {
                pieces[0].AddComponent<BoxCollider>();
                pieces[1].AddComponent<BoxCollider>();
            }

            // Set as highlightable object
            pieces[1].AddComponent<HightLightedObject>();

            yield return null;
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