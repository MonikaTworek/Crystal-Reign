using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    class EffectDestruction: Effect
    {
        public const EffectType _type = EffectType.DESTRUCTION;

        public Vector3 center;
        public float radius;
    }
}
