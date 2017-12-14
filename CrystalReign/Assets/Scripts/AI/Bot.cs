using Assets.Scripts.Effects;
using Effects;
using UnityEngine;

namespace AI
{
	public abstract class Bot : EffectConsumer
    {

        public string playerTag = "Player";
        private GameObject player;
        private RaycastHit hit;

        public Weapon weapon;
        public Transform gunEnd;
		public float FireRate = .35f;
        public const float maxHP = 100;

        private float hp = maxHP;
		private float nextFireTime;

		public abstract void move(Vector3 destination);

		public virtual void shoot(Vector3 direction)
		{
			UpdateNextFireTime();
			weapon.Shoot(gunEnd.position, direction);
		}
		
		private void UpdateNextFireTime()
		{
			nextFireTime = Time.time + FireRate;
		}

		public bool CanShoot()
		{
			return Time.time > nextFireTime;
		}


		public virtual void rotate(Vector3 direction)
		{
			transform.LookAt(direction);
		}
    
		public override void Apply(Effect effect, Vector3 origin)
        {
            switch (effect.effectType)
            {
                case EffectType.REDUCE_HP:
                    hp -= ((HpReduceEffect)effect).value;
                    if (hp <= 0)
                    {
                        Destroy(gameObject);
                    }
                    break;
            }
        }

        void Start()
        {
            player = GameObject.FindGameObjectWithTag(playerTag);
        }

        void Update()
        {
            rotate(player.gameObject.transform.position);
            if (CanShoot())
            {
                Vector3 botPosition = transform.position;
                Physics.Raycast(botPosition, direction(botPosition), out hit);
                if (PlayerWasHit(hit))
                {
                    shoot(hit.point);
                }
            }
        }

        private Vector3 direction(Vector3 botPosition)
        {
            return (RandomizedPlayerPosition() - botPosition).normalized;
        }

        private Vector3 RandomizedPlayerPosition()
        {
            Vector3 playerPosition = player.transform.position;
            return new Vector3(
                playerPosition.x + Random.Range(-1.25f, 1.25f),
                playerPosition.y + Random.Range(-1.25f, 1.25f),
                playerPosition.z + Random.Range(-1.25f, 1.25f));
        }

        private bool PlayerWasHit(RaycastHit hit)
        {
            return hit.collider.gameObject.tag.Equals(playerTag);
        }
    }
}
