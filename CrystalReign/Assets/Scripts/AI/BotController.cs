using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class BotController : MonoBehaviour
	{
		public List<Bot> bots;
		public string playerTag = "Player";

		private GameObject player;
		private RaycastHit hit;

		void Start()
		{
			player = GameObject.FindGameObjectWithTag(playerTag);
		}
		
		void Update()
		{
			foreach (Bot bot in bots)
			{
				bot.rotate(player.gameObject.transform.position);
				if (bot.CanShoot())
				{	
					Vector3 botPosition = bot.transform.position;
					Physics.Raycast(botPosition,direction(botPosition), out hit);
					if (PlayerWasHit(hit))
					{
						bot.shoot(hit.point);
					}
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
