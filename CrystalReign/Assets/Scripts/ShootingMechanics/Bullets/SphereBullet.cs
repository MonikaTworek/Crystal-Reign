using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bullets
{
    public class SphereBullet : Bullet
    {
        public float SphereRadius;
        
        protected override List<EffectConsumer> GetHitConsumers(Collision other)
        {
            Vector3 average = Vector3.zero;
            other.contacts.ToList().ForEach(x => average += x.point);
            average /= other.contacts.Length;
            return Physics.OverlapSphere(average, SphereRadius)
                .Where(x => x.GetComponent<EffectConsumer>() != null)
                .Select(x => x.GetComponent<EffectConsumer>())
                .ToList();
        }
    }
}