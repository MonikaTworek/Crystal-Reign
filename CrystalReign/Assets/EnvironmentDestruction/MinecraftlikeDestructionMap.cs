using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.EnvironmentDestruction
{
    public class MinecraftlikeDestructionMap : MonoBehaviour
    {
        public TextAsset booleanMapJson;

        public GameObject node;

        public float unit_size = 1;

        List<List<List<GameObject>>> map;

        private void Start()
        {
            List<List<List<bool>>> boolMap = JsonConvert.DeserializeObject<List<List<List<bool>>>>(booleanMapJson.text);
            map = new List<List<List<GameObject>>>();
            for (int i = 0; i < boolMap.Count; i++)
            {
                map.Add(new List<List<GameObject>>());
                for (int j = 0; j < boolMap[i].Count; j++)
                {
                    map[i].Add(new List<GameObject>());
                    for (int k = 0; k < boolMap[i][j].Count; k++)
                    {
                        GameObject go = null;
                        if (boolMap[i][j][k])
                        {
                            go = Instantiate(node);
                            go.transform.position = new Vector3(i, j, k) * unit_size;
                            go.transform.SetParent(transform);
                            Rigidbody rb = go.GetComponent<Rigidbody>();
                            rb.isKinematic = true;
                        }
                        map[i][j].Add(go);
                    }
                }
            }
        }

        public List<GameObject> GetAndRemoveNodesInsideSphere(Vector3 center, float radius)
        {
            List<GameObject> res = new List<GameObject>();

            center = new Vector3(Mathf.Round(center.x / unit_size), Mathf.Round(center.y / unit_size), Mathf.Round(center.z / unit_size));
            int iradius = (int)Mathf.Round(radius/unit_size);
;
            
            for (int i = -iradius; i < iradius; i++)
            {
                for (int j = -iradius; j < iradius; j++)
                {
                    for (int k = -iradius; k < iradius; k++)
                    {
                        if (!(center.x+i < 0 || center.y + j<0 || center.z + k < 0 ||
                            center.x + i >= map.Count || center.y + j >= map[0].Count || center.z + k >= map[0][0].Count))
                        {
                            GameObject go = map[(int)center.x + i][(int)center.y + j][(int)center.z + k];
                            if (go != null && (go.transform.position - center).magnitude < radius)
                            {
                                res.Add(go);
                                map[(int)center.x + i][(int)center.y + j][(int)center.z + k] = null;
                            }
                        }
                    }
                }
            }
            return res;
        }

    }
}
