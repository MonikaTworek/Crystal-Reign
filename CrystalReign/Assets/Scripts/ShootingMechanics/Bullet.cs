using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour {
    
    public Effect effect { get; set; }
    public bool ShouldBeRemoved = true;
    
    private void OnCollisionEnter(Collision other)
    {
        foreach (EffectConsumer effectConsumer in GetHitConsumers(other))
        {
            ApplyEffectOnConsumer(effectConsumer);  
        }
        if (ShouldBeRemoved)
        {
           Destroy(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    protected abstract List<EffectConsumer> GetHitConsumers(Collision other);

    private void ApplyEffectOnConsumer(EffectConsumer effectConsumer)
    {
        effectConsumer.Apply(effect);
    }
}
