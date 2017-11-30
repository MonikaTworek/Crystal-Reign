using Assets.Scripts.Effects;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.EnvironmentDestruction
{
    class DestructableObject : EffectConsumer
    {
        private DestructableObjectMap chunks_data;

        public string level_name;

        public float forceValue = 10f;
        public float forceRandomRange = 2f;
        public float forceAngleRandomRange = 30f;

        public Material mat;

        private bool spawn_tree = true;

        public ChunksSpawner spawner;
        public Transform container;


        public Vector3 force_to_add;
        public float dist;

        private void Start()
        {
            ReloadData();
        }

        public void ReloadData()
        {
            TextAsset mapJsonFile = Resources.Load(level_name + '/' + gameObject.name + "_description") as TextAsset;
            if (mapJsonFile == null)
            {
                spawn_tree = false;
            }
            else
            {
                chunks_data = JsonConvert.DeserializeObject<DestructableObjectMap>(mapJsonFile.text);
            }
        }

        public override void Apply(Effect effect)
        {
            switch (effect.effectType)
            {
                case EffectDestruction._type:

                    EffectDestruction effectDestruction = (EffectDestruction)effect;

                    if (spawn_tree)
                    {
                        if (transform.childCount == 0)
                        {
                            List<DestructableObjectMapNode> chunks_to_spawn = new List<DestructableObjectMapNode>(chunks_data.chunks);
                            for (int i = 0; i < chunks_to_spawn.Count; i++)
                            {
                                chunks_to_spawn[i].relative_position += transform.position;
                            }
                            for (int i = 0; i < chunks_to_spawn.Count; i++)
                            {
                                if (Vector3.Distance(chunks_to_spawn[i].relative_position, effectDestruction.center) <= effectDestruction.radius)
                                {
                                    TextAsset jsonFile = Resources.Load(level_name + '/' + chunks_to_spawn[i].name + "_description") as TextAsset;
                                    if (jsonFile != null)
                                    {
                                        List<DestructableObjectMapNode> chunks_to_add = JsonConvert.DeserializeObject<DestructableObjectMap>(jsonFile.text).chunks;
                                        chunks_to_add.ForEach(x => x.relative_position += chunks_to_spawn[i].relative_position);
                                        chunks_to_spawn.AddRange(chunks_to_add);
                                        chunks_to_spawn.RemoveAt(i);
                                        i--;
                                    }
                                }
                            }
                            List<GameObject> gos = container.GetComponentsInChildren<Transform>(true).Select(x => x.gameObject).ToList();
                            for (int i = 0; i < chunks_to_spawn.Count; i++)
                            {
                                GameObject go = gos.Find(x => x.name == chunks_to_spawn[i].name);
                                if (go != null)
                                {
                                    go.SetActive(true);
                                    go.AddComponent<MeshCollider>();
                                    go.transform.position = chunks_to_spawn[i].relative_position;

                                    if (Vector3.Distance(chunks_to_spawn[i].relative_position, effectDestruction.center) <= effectDestruction.radius)
                                    {
                                        go.transform.localScale = Vector3.one * 0.8f;
                                        go.GetComponent<MeshCollider>().convex = true;
                                        Vector3 relative = go.transform.position - effectDestruction.center;
                                        Vector3 force = relative.normalized * Random.Range(forceValue - forceRandomRange / 2, forceValue + forceRandomRange / 2);
                                        force = Quaternion.Euler(
                                            Random.Range(-forceAngleRandomRange / 2, forceAngleRandomRange / 2),
                                            Random.Range(-forceAngleRandomRange / 2, forceAngleRandomRange / 2),
                                            Random.Range(-forceAngleRandomRange / 2, forceAngleRandomRange / 2)) * force;

                                        Rigidbody rb = go.GetComponent<Rigidbody>();
                                        if (rb == null) rb = go.AddComponent<Rigidbody>();
                                        go.GetComponent<MeshRenderer>().material = mat;

                                        rb.isKinematic = false;
                                        rb.AddForce(force);
                                    }
                                    else
                                    {
                                        DestructableObject fob = go.AddComponent<DestructableObject>();
                                        fob.level_name = level_name;
                                        fob.mat = mat;
                                        fob.forceValue = forceValue;
                                        fob.forceRandomRange = forceRandomRange;
                                        fob.forceAngleRandomRange = forceAngleRandomRange;
                                        fob.spawner = spawner;
                                        fob.container = container;
                                        fob.ReloadData();

                                    }
                                }
                            }
                            Destroy(gameObject);
                        }
                    }
                    else
                    {
                        transform.localScale = Vector3.one * 0.8f;
                        GetComponent<MeshCollider>().convex = true;
                        Vector3 relative = transform.position - effectDestruction.center;
                        Vector3 force = relative.normalized * Random.Range(forceValue - forceRandomRange / 2, forceValue + forceRandomRange / 2);
                        force = Quaternion.Euler(
                            Random.Range(-forceAngleRandomRange / 2, forceAngleRandomRange / 2),
                            Random.Range(-forceAngleRandomRange / 2, forceAngleRandomRange / 2),
                            Random.Range(-forceAngleRandomRange / 2, forceAngleRandomRange / 2)) * force;
                        force_to_add = force;
                        dist = Vector3.Distance(transform.position, effectDestruction.center);

                        Rigidbody rb = GetComponent<Rigidbody>();
                        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
                        GetComponent<MeshRenderer>().material = mat;

                        rb.isKinematic = false;
                        rb.AddForce(force);
                    }



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
                    List<EffectConsumer> affected = Physics.OverlapSphere(info.point, 1f)
                                                .Where(x => x.GetComponent<EffectConsumer>() != null)
                                                .Select(x => x.GetComponent<EffectConsumer>())
                                                .ToList();
                    foreach (EffectConsumer e in affected)
                    {
                            e.Apply(new EffectDestruction() { center = info.point, radius = 1f });
                    }
                }
            }
        }
    }
}
