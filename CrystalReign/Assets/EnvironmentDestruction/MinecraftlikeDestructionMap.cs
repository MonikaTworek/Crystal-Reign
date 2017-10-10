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

        public Vector3 unit_size = Vector3.zero;

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
                            go.transform.position = new Vector3(i * unit_size.x, j * unit_size.y, k * unit_size.z);
                            go.transform.SetParent(transform);
                            Rigidbody rb = go.GetComponent<Rigidbody>();
                            rb.isKinematic = true;
                            map[i][j].Add(go);
                        }
                        map[i][j].Add(go);
                    }
                }
            }
        }

        public List<GameObject> GetAndRemoveNodesInsideSphere(Vector3 center, float radius)
        {
            List<GameObject> res = new List<GameObject>();

            center = new Vector3(Mathf.Round(center.x / unit_size.x), Mathf.Round(center.y / unit_size.y), Mathf.Round(center.z / unit_size.z));
            radius = Mathf.Round(radius);

            Vector3 box_top_left = center - radius*Vector3.one;
            
            for (int i = 0; i < 2*radius; i++)
            {
                for (int j = 0; j < 2 * radius; j++)
                {
                    for (int k = 0; k < 2 * radius; k++)
                    {
                        if (!(box_top_left.x+i < 0 || box_top_left.y + j<0 || box_top_left.z + k < 0 ||
                            box_top_left.x + i >= map.Count || box_top_left.y + j >= map[0].Count || box_top_left.z + k >= map[0][0].Count))
                        {
                            GameObject go = map[(int)box_top_left.x + i][(int)box_top_left.y + j][(int)box_top_left.z + k];
                            if (go != null && (go.transform.position - center).magnitude < radius)
                            {
                                res.Add(go);
                                map[(int)box_top_left.x + i][(int)box_top_left.y + j][(int)box_top_left.z + k] = null;
                            }
                        }
                    }
                }
            }
            return res;
        }

    }
}
