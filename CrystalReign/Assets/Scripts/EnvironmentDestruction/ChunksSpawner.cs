using Assets.Scripts.EnvironmentDestruction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunksSpawner : MonoBehaviour {

    public List<GameObject> chunks;

    bool spawn = false;

    public Material mat;
    public int toSpawnNr = 10;

    void Spawn(List<GameObject> to_spawn)
    {
        foreach (GameObject go in to_spawn)
        {

            Rigidbody rb = go.GetComponent<Rigidbody>();
            if (rb == null) rb = go.AddComponent<Rigidbody>();
            go.GetComponent<MeshCollider>().convex = true;
            go.GetComponent<MeshRenderer>().material = mat;

            rb.isKinematic = false;
            rb.AddForce(go.GetComponent<DestructableObject>().force_to_add);
            Destroy(go.GetComponent<DestructableObject>());
        }
    }


    private void LateUpdate()
    {
        if (spawn)
        {
            if (chunks.Count >= toSpawnNr)
            {
                Spawn(chunks.GetRange(0, toSpawnNr));
                chunks.RemoveRange(0, toSpawnNr);
            }
            else
            {
                Spawn(chunks);
                chunks.Clear();
            }
            if (chunks.Count == 0) spawn = false;
        }
        if (chunks.Count > 0)
        {
            Debug.Log(chunks.Count);
            chunks.Sort((x, y) =>
            {
                float r = x.GetComponent<DestructableObject>().dist - y.GetComponent<DestructableObject>().dist;
                if (r == 0) return 0;
                else if (r < 0) return -1;
                return 1;
            });
            spawn = true;
        }
    }


}
