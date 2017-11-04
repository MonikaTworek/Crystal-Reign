using UnityEngine;

public class DestructorWeapon : Weapon
{
    public float SphereRadius = 5f;
    public float ForceValue = 1f;
    public float BulletSpeed = 6f;
    public GameObject SphereBullet;

    public override void Shoot(Vector3 origin, Vector3 direction)
    {
        GameObject bullet = Instantiate(SphereBullet);
        bullet.transform.position = origin;
        bullet.transform.LookAt(direction);
        bullet.GetComponent<Rigidbody>().velocity = 
            BulletVelocity(origin, Randomized(direction));
    }

    private Vector3 Randomized(Vector3 direction)
    {
        return new Vector3(
            direction.x + Accuracy,
            direction.y + Accuracy,
            direction.z + Accuracy);
    }

    private Vector3 BulletVelocity(Vector3 origin, Vector3 direction)
    {
        return (direction - origin).normalized * BulletSpeed;
    }

    private static float Accuracy
    {
        get { return Random.Range(-0.25f, 0.25f); }
    }
}