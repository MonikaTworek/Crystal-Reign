using UnityEngine;

namespace Assets.Scripts.Effects
{
    public abstract class Effect : MonoBehaviour
    {
        public EffectType effectType { get; protected set; }
        
    }
}