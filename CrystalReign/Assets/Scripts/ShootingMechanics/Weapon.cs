using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ShootingMechanics
{
    public abstract class Gun : MonoBehaviour
    {

        public virtual void Shoot(Vector3 direction)
        {
            Shoot(transform.position, direction);
        }

        public abstract void Shoot(Vector3 origin, Vector3 direction);
    }
}
