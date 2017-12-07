using UnityEngine;

public class GameOverlord : MonoBehaviour
{

	public double playerHitPoints;
	public double ammunitionLeft;
	public Weapon selectedWeapon;
	public WeaponChanger WeaponChanger;
	
	
	public void processMessage(OverlordMessage message, double value)
	{
		switch (message)
		{
			case OverlordMessage.CHANGE_PLAYER_HIT_POINTS:
			{
				playerHitPoints += value;
				break;
			}
			case OverlordMessage.CHANGE_AMMUNITION:
			{
				ammunitionLeft += value;
				break;
			}
			case OverlordMessage.CHANGE_WEAPON:
			{
				selectedWeapon = value > 0 ? 
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
