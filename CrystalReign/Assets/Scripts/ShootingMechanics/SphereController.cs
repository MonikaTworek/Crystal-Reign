using UnityEngine;

public class SphereController : MonoBehaviour {
	
	public float StartingSphereSize = 0.1f;
	public float MaxSphereSize = 5f;
	public Material Mat;
	public int ControllerExpirationTime = 10;

	private int timer;
	private float maxTimer;
	private GameObject sphere;

	void Update()
	{         
		if (timer <= 0)
		{
			Destroy(sphere);
			Destroy(gameObject, 3.0f);
		}
		else
		{
			timer--;
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
		currentColor.a = 0.5f - (maxTimer - timer) / maxTimer * 0.6f;
		sphere.gameObject.GetComponentInChildren<MeshRenderer>().material.color = currentColor;
	}
	
	public void Init(Vector3 point)
	{
		maxTimer = ControllerExpirationTime * 12f;
		timer = ControllerExpirationTime * 12;
		sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		Destroy(sphere.GetComponent<Collider>());
		sphere.transform.position = point;
		sphere.transform.localScale = new Vector3(StartingSphereSize, StartingSphereSize, StartingSphereSize);
		sphere.gameObject.GetComponent<MeshRenderer>().material = Mat;
		SetColor();
	}

	// TODO: remove after creating proper material
	private void SetColor()
	{
		Color color = sphere.gameObject.GetComponent<MeshRenderer>().material.color;
		sphere.gameObject.GetComponent<MeshRenderer>().material.color =
			new Color(color.r, color.g, color.b, 0.01f);
	}
}
