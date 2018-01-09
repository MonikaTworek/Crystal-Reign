using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class HpRestoreEffect : Effect
    {

        public float value;

        public HpRestoreEffect()
        {
            effectType = EffectType.RESTORE_HP;
            value = 20;
        }
        public HpRestoreEffect(float value)
        {
            effectType = EffectType.RESTORE_HP;
            this.value = value;
        }
    }
}
