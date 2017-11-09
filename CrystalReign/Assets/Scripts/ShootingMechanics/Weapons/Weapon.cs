using UnityEngine;

public abstract class Weapon : MonoBehaviour
{

    public GameObject Bullet;
    public float BulletSpeed = 6f;
    public float AccuracyRange = 0.5f;
    public float FireRate = 0.4f;
    
    public virtual void Shoot(Vector3 direction)
    {
        Shoot(transform.position, direction);
    }
    
    public abstract void Shoot(Vector3 origin, Vector3 direction);

    public abstract float GetFireRate();
}
