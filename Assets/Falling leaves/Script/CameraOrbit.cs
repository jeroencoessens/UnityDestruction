using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class CameraOrbit : MonoBehaviour {
	
	public Transform target;
	public float distance = 5.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	
	public float distanceMin = .5f;
	public float distanceMax = 15f;


	public bool  animate = true;
	
	float x = 0.0f;
	float y = 0.0f;
	
	// Use this for initialization
	void Start () 
	{
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;		
	
	}
	public void setOrbit()
	{
		animate = !animate;
	}
	void LateUpdate () 
	{
		if (target) 
		{
			if(animate)
				x += Time.deltaTime* xSpeed * distance* 0.02f;
			if(Input.GetMouseButton(0))
				x += Input.GetAxis("Mouse X") * xSpeed * distance* 0.02f;

			//x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			
			Quaternion rotation = Quaternion.Euler(30, x, 0);
			
			distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, distanceMin, distanceMax);
	
			Vector3 negDistance = new Vector3(0.0f, 1.0f, -distance);
			Vector3 position = rotation * negDistance;// + target.position;
			
			transform.rotation = rotation;
			transform.position = position;
		}
	}
	
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}