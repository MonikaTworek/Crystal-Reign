using Assets.Scripts.Effects;

namespace Effects
{
	public class HpReduceEffect : Effect {

        public float value;

        public HpReduceEffect()
        {
            effectType = EffectType.REDUCE_HP;
            value = 10;
        }
        public HpReduceEffect(float value)
        {
            effectType = EffectType.REDUCE_HP;
            this.value = value;
        }

    }
}
