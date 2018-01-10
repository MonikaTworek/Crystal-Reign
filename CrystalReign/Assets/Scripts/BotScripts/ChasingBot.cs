using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Effects;
using System;

public class ChasingBot : Bot {

    private Vector3? playerLastSeenPosition = null;
    private Rigidbody rb;
    public float speed = 2;
    public float jumpSpeed = 20;
    public float frontMaxDistance = 5;
    public float raycastSphereRadius = 0.7f;
    public float groundDistance = 5;
    private Vector3? currentDir = null;

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
                //throw new NotImplementedException();
                break;
        }
    }

    public override void move()
    {
        if (currentDir != null)
        {
            Quaternion rotationAngle = Quaternion.LookRotation(currentDir.Value);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * 5.5f);

        }
        RaycastHit info;
        if (playerLastSeenPosition == null)
        {
            rb.velocity = Vector3.zero;
        }
        else if (Vector3.Distance(transform.position, player.transform.position) < 3)
        {
            rb.velocity = Vector3.zero;

        }
        else if (Physics.SphereCast(transform.position, raycastSphereRadius, transform.forward, out info, frontMaxDistance, withoutBullets.value))
        {
            rb.velocity = Vector3.zero;

        }
        else
        {
            rb.velocity = transform.forward * speed;
        }

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
            currentDir = playerLastSeenPosition - transform.position;
        }

        //animate
        transform.GetChild(0).Rotate(new Vector3(0, 0, rollAnimationSpeed*Time.deltaTime));
	}
}
