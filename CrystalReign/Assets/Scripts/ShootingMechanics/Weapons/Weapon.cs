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

    public void Shoot(Vector3 origin, Vector3 direction)
    {
        GameObject bullet = Instantiate(Bullet);
        foreach (Transform trans in bullet.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = gameObject.layer;
        }
        bullet.layer = gameObject.layer;
        bullet.transform.position = origin;
        bullet.transform.LookAt(direction);
        bullet.GetComponent<Rigidbody>().velocity = BulletVelocity(origin, Randomized(direction));
    }

    private Vector3 BulletVelocity(Vector3 origin, Vector3 direction)
    {
        return (direction - origin).normalized * BulletSpeed;
    }

    private Vector3 Randomized(Vector3 direction)
    {
        return new Vector3(
            direction.x + Accuracy,
            direction.y + Accuracy,
            direction.z + Accuracy);
    }

    private float Accuracy
    {
        get { return Random.Range(-AccuracyRange / 2, AccuracyRange / 2); }
    }
}
