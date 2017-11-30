using UnityEngine;

namespace Assets.Scripts.Effects
{
    public abstract class EffectConsumer : MonoBehaviour
    {
        public abstract void Apply(Effect effect, Vector3 origin);
    }
}
