using System;
using Assets.Scripts.Effects;
using UnityEngine;
using Effects;

namespace AI
{
	public class StaticBot : Bot
    {
        public override void Apply(Effect effect, Vector3 origin)
        {
            switch (effect.effectType)
            {
                case EffectType.REDUCE_HP:
                    hp -= ((HpReduceEffect)effect).value;
                    GetComponent<Animator>().Play("Fadeout");
                    if (hp <= 0)
                    {
                        Destroy(gameObject);
                        BotSpawner.instance.removeBot(this);
                    }
                    break;
            }
        }

        public override void move(Vector3 destination)
		{
			Debug.Log("no co ty");
		}
	}
}
