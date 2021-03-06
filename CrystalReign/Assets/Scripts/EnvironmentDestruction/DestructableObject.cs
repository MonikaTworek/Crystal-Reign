﻿
using Assets.Scripts.Effects;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Assets.Scripts.EnvironmentDestruction
{
    public class DestructableObject : EffectConsumer
    {
        private DestructableObjectMap chunks_data;
        public const string CHUNKS_FOLDER = "Chunks";
        public float forceValue = 10f;
        public float forceRandomRange = 2f;
        public float forceAngleRandomRange = 30f;

        public Material mat;

        private bool spawn_tree = true;

        private RandomSoundPlayer soundsPlayer;

        private void Start()
        {
            ReloadData();
        }

        public void ReloadData()
        {
            soundsPlayer = GetComponent<RandomSoundPlayer>();
            TextAsset mapJsonFile = Resources.Load(CHUNKS_FOLDER + '/' + gameObject.name + "_description") as TextAsset;
            if (mapJsonFile == null)
            {
                spawn_tree = false;
            }
            else
            {
                chunks_data = JsonConvert.DeserializeObject<DestructableObjectMap>(mapJsonFile.text);
            }
        }

        public override void Apply(Effect effect, Vector3 origin)
        {
            switch (effect.effectType)
            {
                case EffectDestruction._type:

                    if (soundsPlayer != null) soundsPlayer.Play(true);

                    EffectDestruction effectDestruction = (EffectDestruction)effect;

                    if (spawn_tree)
                    {
                        if (transform.childCount == 0)
                        {
                            GameObject chunks = Instantiate(Resources.Load(CHUNKS_FOLDER + '/' + gameObject.name + "_chunks")) as GameObject;
                            while (chunks.transform.childCount > 0)
                            {
                                chunks.transform.GetChild(0).SetParent(transform);
                            }
                            DestroyObject(chunks);
                            foreach (Transform chunk in transform)
                            {
                                chunk.position = transform.position + chunks_data.chunks.Find(x => x.name == chunk.gameObject.name).relative_position;
                                chunk.gameObject.AddComponent<MeshCollider>();
                                chunk.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
                                DestructableObject fob = chunk.gameObject.AddComponent<DestructableObject>();
                                fob.mat = mat;
                                fob.forceValue = forceValue;
                                fob.forceRandomRange = forceRandomRange;
                                fob.forceAngleRandomRange = forceAngleRandomRange;

                                chunk.gameObject.AddComponent<RandomSoundPlayer>();
                                foreach (AudioSource audiosrc in GetComponents<AudioSource>())
                                {
                                    AudioSource audio = chunk.gameObject.AddComponent<AudioSource>();
                                    audio.playOnAwake = false;
                                    audio.clip = audiosrc.clip;
                                    audio.spatialBlend = audiosrc.spatialBlend;
                                    audio.maxDistance = audiosrc.maxDistance;
                                    audio.rolloffMode = AudioRolloffMode.Custom;
                                    audio.SetCustomCurve(AudioSourceCurveType.CustomRolloff,audiosrc.GetCustomCurve(AudioSourceCurveType.CustomRolloff));

                                }
                                fob.ReloadData();
                            }
                            List<EffectConsumer> affectedt = Physics.OverlapSphere(origin, effectDestruction.radius)
                                                        .Where(x => x.GetComponent<EffectConsumer>() != null)
                                                        .Select(x => x.GetComponent<EffectConsumer>())
                                                        .ToList();
                            List<EffectConsumer> affected = new List<EffectConsumer>();
                            foreach (Transform child in transform)
                            {
                                if (child.GetComponent<EffectConsumer>() != null && affectedt.Contains(child.GetComponent<EffectConsumer>()))
                                {
                                    affected.Add(child.GetComponent<EffectConsumer>());
                                }
                            }
                            foreach (EffectConsumer e in affected)
                            {
                                if (e.transform.parent == transform)
                                {
                                    e.Apply(effectDestruction, origin);
                                }
                            }

                        }
                        Destroy(GetComponent<MeshFilter>());
                        Destroy(GetComponent<MeshCollider>());
                        Destroy(GetComponent<MeshRenderer>());
                        Destroy(this);
                    }
                    else
                    {
                        GetComponent<MeshCollider>().convex = true;
                        Vector3 relative = transform.position - origin;
                        Vector3 force = relative.normalized * Random.Range(forceValue - forceRandomRange / 2, forceValue + forceRandomRange / 2);
                        force = Quaternion.Euler(
                            Random.Range(-forceAngleRandomRange / 2, forceAngleRandomRange / 2),
                            Random.Range(-forceAngleRandomRange / 2, forceAngleRandomRange / 2),
                            Random.Range(-forceAngleRandomRange / 2, forceAngleRandomRange / 2)) * force;

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

    }
}
