using System.Collections.Generic;
using Assets.Scripts.Effects;
using Bullets;
using UnityEngine;

public class BotBullets : Bullet {

	protected override List<EffectConsumer> GetHitConsumers(Collision other)
	{
		var effectConsumers = new List<EffectConsumer>();
		if (GameObject.FindGameObjectWithTag("Player") == other.gameObject)
		{
			effectConsumers.Add(other.transform.GetComponent<EffectConsumer>());
		}
		return effectConsumers;
	}
}
