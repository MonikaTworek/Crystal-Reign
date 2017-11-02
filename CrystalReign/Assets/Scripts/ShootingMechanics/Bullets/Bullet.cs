using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour {
    
    public Effect effect { get; set; }
    public bool ShouldBeRemoved = true;
    
    private bool isAfterCollision;
    private static float maxTimer = 96f;
    private int timer = (int) maxTimer;
    
    void Update()
    {
        if (isAfterCollision && ShouldBeRemoved)
        {
            if (timer <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                AddTransparency();
                timer--;
            }
        }
    }

    private void AddTransparency()
    {
        Color currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
        currentColor.a = 1f - (maxTimer - timer) / maxTimer;
        gameObject.GetComponent<MeshRenderer>().material.color = currentColor;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isAfterCollision)
        {
            return;
        }
        isAfterCollision = true;
        foreach (EffectConsumer effectConsumer in GetHitConsumers(other))
        {
            ApplyEffectOnConsumer(effectConsumer);  
        }
        gameObject.GetComponent<Rigidbody>().useGravity = true;

    }
    
    protected abstract List<EffectConsumer> GetHitConsumers(Collision other);

    private void ApplyEffectOnConsumer(EffectConsumer effectConsumer)
    {
        effectConsumer.Apply(effect);
    }
}
