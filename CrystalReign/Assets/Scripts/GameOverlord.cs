using UnityEngine;

public class GameOverlord : MonoBehaviour
{

	public double PlayerHitPoints = 10;
	public double AmmunitionLeft = 100;
	public Weapon SelectedPlayerWeapon;
	public WeaponChanger WeaponChanger;
	
	
	public void processMessage(OverlordMessage message, double value)
	{
		switch (message)
		{
			case OverlordMessage.CHANGE_PLAYER_HIT_POINTS:
			{
				PlayerHitPoints += value;
				break;
			}
			case OverlordMessage.CHANGE_AMMUNITION:
			{
				AmmunitionLeft += value;
				break;
			}
			case OverlordMessage.CHANGE_WEAPON:
			{
				SelectedPlayerWeapon = value > 0 ? 
					WeaponChanger.GetNextWeapon() : 
					WeaponChanger.GetPreviousWeapon();
				break;
			}
		}	
	}
}

public enum OverlordMessage
{
	CHANGE_PLAYER_HIT_POINTS,
	CHANGE_AMMUNITION,
	CHANGE_WEAPON
}
