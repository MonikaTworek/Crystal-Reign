using System.Collections.Generic;
using UnityEngine;

public class SphereCreatingBullet : Bullet
{
    public float SphereRadius;
    public SphereController SphereController;

    protected override List<EffectConsumer> GetHitConsumers(Collision other)
    {
        SphereController sphereController = Instantiate(SphereController);
        sphereController.Init(other.contacts[0].point);
        return new List<EffectConsumer>{other.gameObject.GetComponent<EffectConsumer>()};
    }
}
