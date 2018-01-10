using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Effects;
using System;
using Effects;

public class ChasingBot : Bot {

    private Vector3? playerLastSeenPosition = null;
    private Rigidbody rb;
    public float speed = 2;
    public float jumpSpeed = 20;
    public float frontMaxDistance = 5;
    public float raycastSphereRadius = 0.7f;
    public float groundDistance = 5;
    private Vector3 currentDir = Vector3.zero;
    public float moveTime = 5;
    public float timer = 0;
    private System.Random random = new System.Random();


    public float rollAnimationSpeed = 150;

    public override void aim(Vector3 direction)
    {
        //throw new NotImplementedException();
    }

    public override void Apply(Effect effect, Vector3 origin)
    {
        switch (effect.effectType)
        {
            case EffectType.REDUCE_HP:
                hp -= ((HpReduceEffect)effect).value;
                GetComponent<Animator>().Play("Fadeout");
                if (hp <= 0)
                {
                    Destroy(gameObject);
                    ObjectsSpawner.instance.removeBot(this);
                }
                break;
        }
    }

    public override void move()
    {
        if (currentDir != null)
        {
            Quaternion rotationAngle = Quaternion.LookRotation(currentDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * 5.5f);

        }
        RaycastHit info;
        if (Physics.SphereCast(transform.position, raycastSphereRadius, transform.forward, out info, frontMaxDistance, withoutBullets.value) && timer <= 0)
        {
            timer = moveTime;
            Vector3 tmp = -transform.TransformVector(transform.forward);
            tmp.y = 0;
            currentDir = tmp;
            playerLastSeenPosition = null;
        }
        if (Physics.SphereCast(transform.position, raycastSphereRadius, currentDir, out info, frontMaxDistance, withoutBullets.value))
        {
            rb.velocity = Vector3.zero;
        }
        else if (Vector3.Distance(transform.position, player.transform.position) < 3)
        {
            rb.velocity = Vector3.zero;
        }
        else if (!playerLastSeenPosition.HasValue && timer <= 0)
        {
            timer = moveTime;
            float z = 1f - (float)random.NextDouble() * 2f;
            float x = 1f - (float)random.NextDouble() * 2f;
            currentDir = new Vector3(x,0,z).normalized;

        }
        else
        {
            rb.velocity = transform.forward * speed;
        }
        timer -= Time.deltaTime;
    }


    // Use this for initialization
    void Start () {
        base.Start();
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        base.Update();
        RaycastHit info;
        Vector3 tmp = rb.velocity;
        if (Physics.Raycast(transform.position, -transform.up, out info,float.MaxValue, withoutBullets.value))
        {
            float dst = Vector3.Distance(transform.position, info.point);
            if (Mathf.Abs(dst - groundDistance) < 0.01)
            {
                tmp.y = 0;
            }
            else if (dst < groundDistance)
            {
                tmp.y = groundDistance - Vector3.Distance(transform.position, info.point);
            }
            else
            {
                tmp += Physics.gravity * Time.deltaTime;
            }
        }
        else
        {
            tmp.y = 2;
        }
        rb.velocity = tmp;
        if (CanSeePlayer())
        {
            playerLastSeenPosition = player.transform.position;
            currentDir = playerLastSeenPosition.Value - transform.position;
        }

        //animate
        transform.GetChild(0).Rotate(new Vector3(0, 0, rollAnimationSpeed*Time.deltaTime));
	}
}
