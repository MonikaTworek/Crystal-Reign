using Assets.Scripts.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPick : MonoBehaviour
{
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        EffectConsumer consumer = other.transform.GetComponent<EffectConsumer>();
        if (consumer != null)
        {
            consumer.Apply(new HpRestoreEffect(20), other.transform.position);
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (rb.IsSleeping())
            rb.WakeUp();
    }
}
