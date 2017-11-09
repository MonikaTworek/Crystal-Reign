using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
	public List<Weapon> Weapons;

	private int CurrentWeaponIndex;
	
	public Weapon GetPreviousWeapon()
	{
		CurrentWeaponIndex--;
		if (CurrentWeaponIndex < 0)
		{
			CurrentWeaponIndex = Weapons.Capacity + CurrentWeaponIndex;
		}
		return Weapons.ToArray()[CurrentWeaponIndex];
	}
	
	public Weapon GetNextWeapon()
	{
		CurrentWeaponIndex++;
		return Weapons.ToArray()[CurrentWeaponIndex % Weapons.Capacity];
	}
	
}
