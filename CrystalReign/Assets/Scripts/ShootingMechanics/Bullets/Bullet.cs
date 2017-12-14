using Assets.Scripts.Effects;
using System.Collections.Generic;
using UnityEngine;

namespace Bullets
{
    public abstract class Bullet : MonoBehaviour
    {

        public bool ShouldBeRemovedImmediately = true;

        protected bool isAfterCollision;
        protected static float maxTimer = 96f;
        protected int timer = (int)maxTimer;

        void Update()
        {
            checkForRemoval();
        }

        protected void checkForRemoval()
        {
            if (isAfterCollision)
            {
                if (timer <= 0)
                {
                    Destroy(gameObject);
                }
                else
                {
                    AddTransparency();
                    timer--;
                }
            }
        }

        private void AddTransparency()
        {
            Color currentColor = gameObject.GetComponentInChildren<MeshRenderer>().material.color;
            currentColor.a = 1f - (maxTimer - timer) / maxTimer;
            gameObject.GetComponentInChildren<MeshRenderer>().material.color = currentColor;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (isAfterCollision)// || other.transform.CompareTag("Player"))
            {
                return;
            }
            isAfterCollision = true;
            foreach (EffectConsumer effectConsumer in GetHitConsumers(other))
            {
                ApplyEffectOnConsumer(effectConsumer);
            }
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            if(ShouldBeRemovedImmediately)
            {
                Destroy(gameObject);
            }
        }

        protected abstract List<EffectConsumer> GetHitConsumers(Collision other);

        private void ApplyEffectOnConsumer(EffectConsumer effectConsumer)
        {
            foreach (Effect effect in gameObject.GetComponents<Effect>())
            {
                effectConsumer.Apply(effect, transform.position);
            }
        }
    }
}
