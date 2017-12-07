using UnityEngine;

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

	void Update()
	{         
		if (timer <= 0)
		{
			Destroy(sphere);
<<<<<<< HEAD
			Destroy(gameObject, 3.0f);
=======
            Destroy(gameObject, 3.0f);
>>>>>>> 6c04055bd8df578d707db50233c6dca86e6c7154
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
