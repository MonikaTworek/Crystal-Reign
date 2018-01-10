using System;
using UnityEngine;

public class GameEdge : MonoBehaviour {

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerOverlord>().processMessage(OverlordMessage.CHANGE_PLAYER_HIT_POINTS, float.MaxValue);
        }
        else if (!other.gameObject.CompareTag(gameObject.tag))
        {
            Destroy(other.gameObject);
        }
        
    }

    private void Update()
    {
        if (rb.IsSleeping())
            rb.WakeUp();
    }
}
