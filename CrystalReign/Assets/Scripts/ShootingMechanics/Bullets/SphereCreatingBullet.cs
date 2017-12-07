using Assets.Scripts.Effects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bullets
{
    public class SphereCreatingBullet : Bullet
    {
        public float radius;
        public SphereController SphereController;

        protected override List<EffectConsumer> GetHitConsumers(Collision other)
        {
            SphereController sphereController = Instantiate(SphereController);
            sphereController.Init(other.contacts[0].point);
            return Physics.OverlapSphere(transform.position, radius).Where(x => x.GetComponent<EffectConsumer>() != null)
                                                        .Select(x => x.GetComponent<EffectConsumer>())
                                                        .ToList(); 
        }
    }
}
