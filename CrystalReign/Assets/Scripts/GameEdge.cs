using System;
using UnityEngine;

public class GameEdge : MonoBehaviour {
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            throw new NotImplementedException();
        }
        else
        {
            Destroy(other.gameObject);
        }
        
    }
}
