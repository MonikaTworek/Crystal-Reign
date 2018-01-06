using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class SphereController : MonoBehaviour {
	
	public float StartingSphereSize = 0.1f;
	public float MaxSphereSize = 5f;
	public Material Mat;
	public float ControllerExpirationTime = 10;
    public float slowdown;
    public float startTransparency;

	private float timer;
	private float maxTimer;
	private GameObject sphere;
    private float timerTick;

    private bool used = false;

	void Update()
	{
        if (Input.GetKeyDown(KeyCode.E) && !used && sphere != null)
        {
            used = true;
            List<Transform> affected = Physics.OverlapSphere(sphere.transform.position, sphere.transform.localScale.x).Select(x => x.GetComponent<Transform>()).ToList();
            foreach (Transform a in affected)
                if (a.GetComponent<Rigidbody>() != null)
                    Destroy(a.GetComponent<Rigidbody>());
        }

		if (timer <= 0)
		{
			Destroy(sphere);
            Destroy(gameObject, 3.0f);
		}
		else
		{
			timer -= timerTick;
            timerTick -= slowdown;
            AddTransparency();
			sphere.transform.localScale = 
				new Vector3(MaxSphereSize*(maxTimer-timer)/maxTimer, 
					MaxSphereSize*(maxTimer-timer)/maxTimer, 
					MaxSphereSize*(maxTimer-timer)/maxTimer);
		}
	}
	
	private void AddTransparency()
	{
		Color currentColor = sphere.gameObject.GetComponentInChildren<MeshRenderer>().material.color;
        currentColor.a = startTransparency - (maxTimer - timer) / maxTimer * startTransparency;
		sphere.gameObject.GetComponentInChildren<MeshRenderer>().material.color = currentColor;
	}
	
	public void Init(Vector3 point)
	{
		maxTimer = ControllerExpirationTime * 12f;
		timer = ControllerExpirationTime * 12;
        timerTick = 1;
		sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		Destroy(sphere.GetComponent<Collider>());
		sphere.transform.position = point;
		sphere.transform.localScale = new Vector3(StartingSphereSize, StartingSphereSize, StartingSphereSize);
		sphere.gameObject.GetComponent<MeshRenderer>().material = Mat;
    }

}
