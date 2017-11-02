using System.Collections;
using UnityEngine;

public class RaycastShootController : MonoBehaviour {

	public float FireRate = .25f;
	public float ShotDuration = 0.7f;
	public Transform GunEnd;	
	public GameObject SelectedWeapon;
	
	private WaitForSeconds shotDuration;
	private LineRenderer laserLine;
	private float nextFireTime;

	void Start ()
	{
		laserLine = GetComponentInParent<LineRenderer>();
		shotDuration = new WaitForSeconds(ShotDuration);
	}

	void Update () {
		if (WasFireButtonPressed() && CanShoot()) {
			UpdateNextFireTime();
			Shoot();
		}
	}

	private void Shoot()
	{
		StartCoroutine(EnableRaycastForShotDuration());
		SetRaycastStart();
		RaycastHit hit;
		bool wasHit = Physics.Raycast(GunEnd.position, GunEnd.forward, out hit);
		Vector3 destination = GetBulletDestination(wasHit, hit);
		SetRaycastEnd(destination);
		SelectedWeapon.GetComponent<Weapon>().Shoot(GunEnd.position, destination);
	}

	private Vector3 GetBulletDestination(bool wasHit, RaycastHit hit)
	{
		Vector3 destination;
		if (wasHit) {
			destination = hit.point;
		} else {
			destination = GunEnd.position + GunEnd.forward;
		}
		return destination;
	}

	private IEnumerator EnableRaycastForShotDuration()
	{
		laserLine.enabled = true;
		yield return shotDuration;
		laserLine.enabled = false;
	}

	private bool CanShoot()
	{
		return Time.time > nextFireTime;
	}

	private bool WasFireButtonPressed()
	{
		return Input.GetButtonDown ("Fire1");
	}

	private void UpdateNextFireTime()
	{
		nextFireTime = Time.time + FireRate;
	}

	private void SetRaycastStart()
	{
		laserLine.SetPosition(0, GunEnd.position);
	}

	private void SetRaycastEnd(Vector3 position)
	{
		laserLine.SetPosition(1, position);
	}
}
