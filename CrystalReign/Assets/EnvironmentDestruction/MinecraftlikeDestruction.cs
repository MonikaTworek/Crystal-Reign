using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.EnvironmentDestruction
{
    public class MinecraftlikeDestruction : MonoBehaviour
    {

        public MinecraftlikeDestructionMap destructionMap;

        public float sphereRadius = 5f;
        public float forceValue = 500f;
        public float forceRandomRange = 100f;
        public float forceAngleRandomRange = 30f;
        public float angularVelocityRandomRange = 1;

        public Material mat;


        // Use this for initialization
        void Start()
        {
        }

        public void Hit(Vector3 point)
        {
            List<GameObject> gos = destructionMap.GetAndRemoveNodesInsideSphere(point, sphereRadius);
            for (int i = 0; i < gos.Count; i++)
            {
                gos[i].GetComponent<MeshRenderer>().material = mat;
                Rigidbody rb = gos[i].GetComponent<Rigidbody>();

                rb.isKinematic = false;
                Vector3 relative = gos[i].transform.position - point;
                Vector3 force = relative.normalized * Random.Range(forceValue - forceRandomRange / 2, forceValue + forceRandomRange / 2);
                force = Quaternion.Euler(
                    Random.Range(-forceAngleRandomRange / 2, forceAngleRandomRange / 2),
                    Random.Range(-forceAngleRandomRange / 2, forceAngleRandomRange / 2),
                    Random.Range(-forceAngleRandomRange / 2, forceAngleRandomRange / 2)) * force;
                rb.angularVelocity = new Vector3(
                    Random.Range(-angularVelocityRandomRange / 2, angularVelocityRandomRange / 2),
                    Random.Range(-angularVelocityRandomRange / 2, angularVelocityRandomRange / 2),
                    Random.Range(-angularVelocityRandomRange / 2, angularVelocityRandomRange / 2));
                rb.AddForce(force);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit info;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out info))
                {
                    Hit(info.point);
                }
            }
        }
    }
}
