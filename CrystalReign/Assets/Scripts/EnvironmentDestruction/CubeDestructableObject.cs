using Assets.Scripts.Effects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.EnvironmentDestruction
{
    class CubeDestructableObject : EffectConsumer
    {
        private CubeDestructableObjectMap map;

        private GameObject cubePrefab;

        public string level_name;

        private void Start()
        {
            TextAsset mapJsonFile = Resources.Load(level_name + '/' + gameObject.name) as TextAsset;
            map = JsonConvert.DeserializeObject<CubeDestructableObjectMap>(mapJsonFile.text);
            cubePrefab = Resources.Load(level_name + '/' + "cube") as GameObject;
        }

        public override void Apply(Effect effect)
        {
            switch (effect.effectType)
            {
                case EffectType.DESTRUCTION:

                    EffectDestruction effectDestruction = (EffectDestruction)effect;
                    
                    if (transform.childCount == 0)
                    {
                        GameObject cubes = Instantiate(Resources.Load(level_name + '/' + gameObject.name + "_d1")) as GameObject;
                        while (cubes.transform.childCount > 0)
                        {
                            cubes.transform.GetChild(0).SetParent(transform);
                        }
                        DestroyObject(cubes);
                        for (int i = 0; i < map.map.Length; i++)
                        {
                            for (int j = 0; j < map.map[0].Length; j++)
                            {
                                for (int k = 0; k < map.map[0][0].Length; k++)
                                {
                                    string node = map.map[i][j][k];
                                    Transform fragment = null;
                                    switch (node)
                                    {
                                        case "cube":
                                            fragment = Instantiate(cubePrefab).transform;
                                            fragment.SetParent(transform);
                                            break;
                                        default:
                                            fragment = transform.Find(node);
                                            break;
                                    }
                                    if (fragment != null)
                                    {
                                        fragment.position = transform.position + map.origin + new Vector3(i*map.unit_size.x, j*map.unit_size.y, k*map.unit_size.z);
                                    }
                                }
                            }
                        }

                    }

                    Destroy(GetComponent<MeshFilter>());
                    Destroy(GetComponent<MeshCollider>());
                    Destroy(GetComponent<MeshRenderer>());
                    Destroy(this);

                    break;
                default:
                    break;
            }
        }

        private void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit info;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out info))
                {
                    Apply(new EffectDestruction() { center = info.point, radius = 4});
                }
            }
        }
    }
}
