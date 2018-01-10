using Assets.Scripts.Effects;
using Effects;
using UnityEngine;
using System.Collections.Generic;


namespace AI
{
	public abstract class Bot : EffectConsumer
    {
        public string playerTag = "Player";
        protected GameObject player;
        private RaycastHit hit;
        public LayerMask withoutBullets;

        private Vector3 oldPlayerPosition;
        protected List<Vector3> rememberedPlayerVelocities;
        private int memorySize = 30;

        public Weapon weapon;
        public Transform gunEnd;
		public float FireRate = .35f;
        public const float maxHP = 100;

        protected float hp = maxHP;
		private float nextFireTime;
        public float accurancyRange = 1.25f;
        public float playerSphereCastRadius = 1f;

        public abstract void move();
        public abstract void aim(Vector3 direction);

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
			return Time.time > nextFireTime && weapon != null && gunEnd != null;
		}


    

        protected void Start()
        {
            findPlayer();
            rememberedPlayerVelocities = new List<Vector3>();
        }

        public void findPlayer()
        {
            if (player == null)
                player = GameObject.FindGameObjectWithTag(playerTag);
        }

        protected void Update()
        {
            move();
            aim(player.gameObject.transform.position);
            if (CanShoot() && CanSeePlayer())
                shoot(Randomized(SpeculatedHit())); //shoot(Randomized(player.transform.position));
            UpdateMemory();
        }

        public bool CanSeePlayer()
        {
            Physics.SphereCast(transform.position, playerSphereCastRadius, player.transform.position - transform.position, out hit);
            return hit.collider.gameObject.tag.Equals(playerTag);
        }

        protected Vector3 SpeculatedHit()
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

        protected void UpdateMemory()
        {
            rememberedPlayerVelocities.Insert(0, (player.transform.position - oldPlayerPosition) / Time.deltaTime);
            if (rememberedPlayerVelocities.Count == memorySize)
                rememberedPlayerVelocities.RemoveAt(memorySize - 1);
            oldPlayerPosition = player.transform.position;
        }

        protected Vector3 Randomized(Vector3 vector)
        {
            return new Vector3(
                vector.x + Random.Range(-accurancyRange, accurancyRange),
                vector.y + Random.Range(-accurancyRange, accurancyRange),
                vector.z + Random.Range(-accurancyRange, accurancyRange));
        }

        /*private Vector3 direction(Vector3 botPosition)
        {
            return (RandomizedPlayerPosition() - botPosition).normalized;
        }*/

        /*private bool PlayerWasHit(RaycastHit hit)
        {
            
        }*/
    }
}
