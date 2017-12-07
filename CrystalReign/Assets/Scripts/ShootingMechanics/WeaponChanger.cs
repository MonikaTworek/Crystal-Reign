using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
	public List<Weapon> Weapons;

	private int CurrentWeaponIndex = -1;
	
	public Weapon GetPreviousWeapon()
	{
		CurrentWeaponIndex--;
		if (CurrentWeaponIndex < 0)
		{
			CurrentWeaponIndex = Weapons.Capacity + CurrentWeaponIndex;
		}
		return Weapons[CurrentWeaponIndex];
	}
	
	public Weapon GetNextWeapon()
	{
		CurrentWeaponIndex = (CurrentWeaponIndex + 1) % Weapons.Capacity;
		return Weapons[CurrentWeaponIndex];
	}
	
}
