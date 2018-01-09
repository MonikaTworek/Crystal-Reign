using Assets.Scripts.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPick : MonoBehaviour
{
    private bool used = false;
    public bool wasUsed() { return used; }
    public void use() { used = true;  }

    /*void OnCollisionStay(Collision other)
    {
        Debug.Log("WESZLO MOCNO");
        EffectConsumer consumer = other.transform.GetComponent<EffectConsumer>();
        if (consumer != null)
            consumer.Apply(new HpRestoreEffect(20), other.contacts[0].point);
    }	*/
}
