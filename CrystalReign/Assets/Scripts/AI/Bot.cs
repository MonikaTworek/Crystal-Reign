using Assets.Scripts.Effects;
using Effects;
using UnityEngine;
using System.Collections.Generic;


namespace AI
{
	public abstract class Bot : EffectConsumer
    {
        public string playerTag = "Player";
        private GameObject player;
        private RaycastHit hit;

        private Vector3 oldPlayerPosition;
        private List<Vector3> rememberedPlayerVelocities;
        private int memorySize = 30;

        public Weapon weapon;
        public Transform gunEnd;
		public float FireRate = .35f;
        public const float maxHP = 100;

        protected float hp = maxHP;
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
    

        void Start()
        {
            player = GameObject.FindGameObjectWithTag(playerTag);
            rememberedPlayerVelocities = new List<Vector3>();
        }

        void Update()
        {
            rotate(player.gameObject.transform.position);
            if (CanShoot())
            {
                Vector3 botPosition = transform.position;
                Vector3 whereToShoot = speculatedHit();//Randomized(speculatedHit());// Randomized(player.transform.position);
                Physics.Raycast(botPosition, player.transform.position - botPosition, out hit);
                if (PlayerWasHit(hit))
                {
                    shoot(whereToShoot);
                }
            }
            updateMemory();
        }

        void updateMemory()
        {
            rememberedPlayerVelocities.Insert(0, (player.transform.position - oldPlayerPosition) / Time.deltaTime);
            if (rememberedPlayerVelocities.Count == memorySize)
                rememberedPlayerVelocities.RemoveAt(memorySize - 1);
            oldPlayerPosition = player.transform.position;
        }

        Vector3 speculatedHit()
        {
            Vector3 playerVelocity = new Vector3(0, 0, 0);
            foreach (Vector3 playerV in rememberedPlayerVelocities)
            {
                playerVelocity.x += playerV.x;
                playerVelocity.z += playerV.z;
            }
            if (rememberedPlayerVelocities.Count > 0)
                playerVelocity /= rememberedPlayerVelocities.Count;

            if (rememberedPlayerVelocities.Count > 0 && rememberedPlayerVelocities[rememberedPlayerVelocities.Count - 1].y != 0)
            playerVelocity.y = rememberedPlayerVelocities[rememberedPlayerVelocities.Count - 1].y - player.GetComponent<PlayerControl>().gravity*Time.deltaTime;

            if (playerVelocity == new Vector3(0, 0, 0))
                return player.transform.position;

            Vector3 dVector = transform.position - player.transform.position;
            float d = dVector.magnitude;
            Vector3 dProjection = Vector3.Project(dVector, playerVelocity);
            Vector3 velocityUnit = playerVelocity.normalized;
            float dParallel = (velocityUnit.x != 0 ? dProjection.x / velocityUnit.x : (velocityUnit.y != 0 ? dProjection.y / velocityUnit.y : dProjection.z / velocityUnit.z));

            float v = playerVelocity.magnitude;
            float vp = weapon.BulletSpeed;
            float vFactor = Mathf.Pow(vp / v, 2) - 1;

            float x = (Mathf.Sqrt(dParallel * dParallel + vFactor * d * d) - dParallel) / vFactor;
            return player.transform.position + velocityUnit * x;
        }

        /*private Vector3 direction(Vector3 botPosition)
        {
            return (RandomizedPlayerPosition() - botPosition).normalized;
        }*/

        private Vector3 Randomized(Vector3 vector)
        {
            return new Vector3(
                vector.x + Random.Range(-1.25f, 1.25f),
                vector.y + Random.Range(-1.25f, 1.25f),
                vector.z + Random.Range(-1.25f, 1.25f));
        }

        private bool PlayerWasHit(RaycastHit hit)
        {
            return hit.collider.gameObject.tag.Equals(playerTag);
        }
    }
}
