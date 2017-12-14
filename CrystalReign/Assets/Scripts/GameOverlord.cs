using UnityEngine;
using UnityEngine.UI;

public class GameOverlord : MonoBehaviour
{

	public double PlayerHitPoints = 10;
	public double AmmunitionLeft = 100;
	public Weapon SelectedPlayerWeapon;
	public WeaponChanger WeaponChanger;
	public Text GUIPlayerHitPoints;

	void Start()
	{
		GUIPlayerHitPoints.text = PlayerHitPoints.ToString("00.00");
	}
	
	public void processMessage(OverlordMessage message, double value)
	{
		switch (message)
		{
			case OverlordMessage.CHANGE_PLAYER_HIT_POINTS:
			{
				PlayerHitPoints += value;
				GUIPlayerHitPoints.text = PlayerHitPoints.ToString("00.00");
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
