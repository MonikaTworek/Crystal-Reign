﻿using System.Collections;
using UnityEngine;

public class RaycastShootController : MonoBehaviour {

	public float ShotDuration = 0.7f;
	public Transform GunEnd;
	public WeaponChanger WeaponChanger;

	private Weapon SelectedWeapon;
	private WaitForSeconds shotDuration;
	private LineRenderer laserLine;
	private float nextFireTime;
	private float FireRate = .25f;

	void Start ()
	{
		laserLine = GetComponentInParent<LineRenderer>();
		shotDuration = new WaitForSeconds(ShotDuration);
		SelectedWeapon = WeaponChanger.GetNextWeapon();
		FireRate = SelectedWeapon.GetComponent<Weapon>().FireRate;
	}

	void Update () {
		if (WasFireButtonPressed() && CanShoot()) {
			UpdateNextFireTime();
			Shoot();
		}
		HandleWeaponChange();
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

	private void HandleWeaponChange()
	{
		float mouseScrollChange = Input.GetAxisRaw("Mouse ScrollWheel");
		if(mouseScrollChange > 0){
			SelectedWeapon = WeaponChanger.GetNextWeapon();
		}
		if(mouseScrollChange < 0){
			SelectedWeapon = WeaponChanger.GetPreviousWeapon();
		}   
	}

	private Vector3 GetBulletDestination(bool wasHit, RaycastHit hit)
	{
		Vector3 destination;
		if (wasHit) {
			destination = hit.point;
            Debug.Log(hit.transform.name);
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