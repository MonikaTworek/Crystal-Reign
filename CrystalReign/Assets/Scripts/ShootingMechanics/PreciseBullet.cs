using Assets.Scripts.Effects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ShootingMechanics
{
    public class PreciseBullet : Bullet
    {
        protected override List<EffectConsumer> GetHitConsumers(Collision other)
        {
            EffectConsumer effectConsumer = other.transform.GetComponent<EffectConsumer>();
            if (effectConsumer != null)
            {
                return new List<EffectConsumer>{ effectConsumer };
            }
            return new List<EffectConsumer>();
        }
    }
}