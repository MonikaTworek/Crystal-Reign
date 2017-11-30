using UnityEngine;

namespace AI
{
	public abstract class Bot : MonoBehaviour
	{
		public Weapon weapon;
		
		private float nextFireTime;
		private float FireRate = .25f;
		
		public abstract void move(Vector3 destination);

		void Start()
		{
			weapon.transform.position = transform.position +
			                            transform.forward.normalized 
			                            + new Vector3(0, 0, 1);
		}
		
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
