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

        public EffectDestruction()
        {
            effectType = _type;
        }
        
        public float radius;
    }
}
