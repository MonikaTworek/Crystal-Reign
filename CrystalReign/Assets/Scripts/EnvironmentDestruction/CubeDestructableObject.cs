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
        private CubeDestructableObjectData map;

        public string level_name;

        private void Start()
        {
            TextAsset mapJsonFile = (TextAsset) Resources.Load(level_name + '/' + gameObject.name);
            map = JsonConvert.DeserializeObject<CubeDestructableObjectData>(mapJsonFile.text);
        }

        public override void Apply(Effect effect)
        {
            switch (effect.effectType)
            {
                case EffectType.DESTRUCTION:


                    DestroyObject(gameObject);

                    break;
                default:
                    break;
            }
        }
    }
}
