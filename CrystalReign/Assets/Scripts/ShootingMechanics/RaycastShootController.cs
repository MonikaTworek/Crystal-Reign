using UnityEngine;

public class RaycastShootController : MonoBehaviour {

	public float ShotDuration = 0.7f;
    public Transform GunEnd;
    public Transform Camera;
	public WeaponChanger WeaponChanger;

    private Weapon SelectedWeapon;
	private WaitForSeconds shotDuration;
	private float nextFireTime;
    private float FireRate;

    void Start ()
	{
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
		RaycastHit hit;
		bool wasHit = Physics.Raycast(Camera.position, Camera.forward, out hit);
		Vector3 destination = GetBulletDestination(wasHit, hit);
		SelectedWeapon.GetComponent<Weapon>().Shoot(GunEnd.position, destination);
	}

	private void HandleWeaponChange()
	{
		float mouseScrollChange = Input.GetAxisRaw("Mouse ScrollWheel");
		if(mouseScrollChange > 0){
			SelectedWeapon = WeaponChanger.GetNextWeapon();
            FireRate = SelectedWeapon.GetComponent<Weapon>().FireRate;
        }
		if(mouseScrollChange < 0){
			SelectedWeapon = WeaponChanger.GetPreviousWeapon();
            FireRate = SelectedWeapon.GetComponent<Weapon>().FireRate;
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
    

	private bool CanShoot()
	{
		return Time.time > nextFireTime;
	}

	private bool WasFireButtonPressed()
	{
		return Input.GetButton("Fire1");
	}

	private void UpdateNextFireTime()
	{
		nextFireTime = Time.time + FireRate;
	}
    
}
