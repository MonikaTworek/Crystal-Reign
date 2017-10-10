using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.EnvironmentDestruction
{
    public class MinecraftlikeDestruction : MonoBehaviour
    {

        public MinecraftlikeDestructionMap destructionMap;

        public float sphereRadius = 5f;
        public float forceValue = 1f;

        public Material mat;
        public GameObject debugDot;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit info;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out info))
                {
                    debugDot.transform.position = info.point;
                    List<GameObject> gos = destructionMap.GetAndRemoveNodesInsideSphere(info.point, sphereRadius);
                    for (int i  = 0; i < gos.Count; i++)
                    {
                        gos[i].GetComponent<MeshRenderer>().material = mat;
                        Rigidbody rb = gos[i].GetComponent<Rigidbody>();
                        rb.isKinematic = false;
                        rb.AddForce((gos[i].transform.position - info.point).normalized * forceValue);
                    }
                }
            }
        }
    }
}
