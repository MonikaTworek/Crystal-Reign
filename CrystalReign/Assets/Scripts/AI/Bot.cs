using UnityEngine;

namespace AI
{
	public abstract class Bot : MonoBehaviour
	{
		public Weapon weapon;
		public float FireRate = .35f;
		
		private float nextFireTime;
		
		public abstract void move(Vector3 destination);
		
		public virtual void shoot(Vector3 direction)
		{
			UpdateNextFireTime();
			weapon.Shoot(direction);
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
		
	}
}
