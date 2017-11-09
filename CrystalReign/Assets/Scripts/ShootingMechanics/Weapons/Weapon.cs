using UnityEngine;

public abstract class Weapon : MonoBehaviour
{

    public virtual void Shoot(Vector3 direction)
    {
        Shoot(transform.position, direction);
    }
    
    public abstract void Shoot(Vector3 origin, Vector3 direction);

    public abstract float GetFireRate();
}
