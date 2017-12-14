using UnityEngine;

public class RaycastDebugDraw : MonoBehaviour {

	public float weaponRange = 50f;
	
	private Camera cam;

	void Start()
	{
		cam = Camera.main;
	}

	void Update () {
		Vector3 lineOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
		Debug.DrawRay(lineOrigin, cam.transform.forward * weaponRange, Color.green);
	}
}
