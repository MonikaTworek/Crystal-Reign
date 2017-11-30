using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class BotController : MonoBehaviour
	{
		public List<Bot> bots;
		public string playerTag = "Player";

		private float nextFireTime;
		private float FireRate = .25f;
		private GameObject player;

		void Start()
		{
			player = GameObject.FindGameObjectWithTag(playerTag);
		}
		
		void Update()
		{
			foreach (Bot bot in bots)
			{
				bot.rotate(player.gameObject.transform.position);
				RaycastHit hit;
				Vector3 botPosition = bot.transform.position;
				bool wasHit = Physics.Raycast(botPosition,
					(player.transform.position - botPosition).normalized, out hit);
				if (CanShoot() && wasHit && PlayerWasHit(hit))
				{
					UpdateNextFireTime();
					bot.shoot(hit.point);
				}
			}
		}

		private void UpdateNextFireTime()
		{
			nextFireTime = Time.time + FireRate;
		}

		private bool CanShoot()
		{
			return Time.time > nextFireTime;
		}

		private bool PlayerWasHit(RaycastHit hit)
		{
			return hit.collider.gameObject.tag.Equals(playerTag);
		}
	}
}
