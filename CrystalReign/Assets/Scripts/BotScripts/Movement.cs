using System;
using System.Collections.Generic;
using AI;
using Assets.Scripts.Effects;
using Effects;
using UnityEngine;

public class Movement : Bot
{

    private bool InAir;
    private float timePlayerNotSeen = 0;
    private double currentJumpTime;

    public enum states { chasingPlayer, followingLastTrack, returning };
    public states state;
    public float speed = 6.0F;
    public float gravity = 20.0F;
    public float jumpForce;
    public float botSpeed;
    public double jumpTime;
    public Rigidbody rb;

    void Start()
    {
        findPlayer();
        rememberedPlayerVelocities = new List<Vector3>();
        rb = GetComponent<Rigidbody>();
        currentJumpTime = jumpTime;
    }

    bool CollideFree()
    {
        var player = GameObject.Find("BB7");
        var botPos = transform.position;

        RaycastHit hit;
        if (Physics.Raycast(botPos, (player.transform.position - botPos).normalized, out hit, 300)
            && (hit.collider.gameObject != player))
        {
            return false;
        }

        return true;
    }

    bool CollideClose()
    {
        var player = GameObject.Find("BB7");
        var botPos = transform.position;

        RaycastHit hit;
        if (Physics.Raycast(botPos, (player.transform.position - botPos).normalized, out hit, 2) 
            && (hit.collider.gameObject != player))
        {
            Debug.Log(hit.collider.gameObject);
            return true;
        }

        return false;
    }

    bool isPlayerClose()
    {
        var player = GameObject.Find("BB7");
        var botPos = transform.position;
        
        RaycastHit hit;
        if (Physics.Raycast(botPos, (player.transform.position - botPos).normalized, out hit, 25) 
            && (hit.collider.gameObject == player))
        {
            return true;
        }
        return false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player" && InAir)
        {
            InAir = false;
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag != "Player" && InAir)
        {
            InAir = false;
        }
    }

    void Update()
    {
        if (currentJumpTime > 0)
        {
            currentJumpTime -= Time.deltaTime;
        }


        var playerTrans = GameObject.Find("BB7").transform;
        var toPlayerRotation = playerTrans.position - transform.position;
        Vector3 rotationAngle = Quaternion.LookRotation(toPlayerRotation).eulerAngles;
        rotationAngle.x = 0;
        transform.rotation = Quaternion.Slerp ( transform.rotation, Quaternion.Euler(rotationAngle), Time.deltaTime * 5.5f);
        
        
        if (!isPlayerClose())
        {
            transform.Translate(Vector3.forward * Time.deltaTime * botSpeed);    
            if (!CollideFree() || CollideClose())
            {
                Debug.Log("not free");
                if (InAir == false && currentJumpTime <= 0)
                {
                    InAir = true;
                    Debug.Log("jump");
                    currentJumpTime = jumpTime;
                    Vector3 jump = new Vector3(1.0f, 250, 1.0f);
                    GetComponent<Rigidbody>().AddForce(jump * jumpForce);
                }
            }
        }

        if (CanShoot() && CanSeePlayer()) ;
            //shoot(Randomized(SpeculatedHit())); //shoot(Randomized(player.transform.position));
        UpdateMemory();
    }

    public override void Apply(Effect effect, Vector3 origin)
    { 
        switch (effect.effectType)
        {
            case EffectType.REDUCE_HP:
                hp -= ((HpReduceEffect)effect).value;
//                GetComponent<Animator>().Play("Fadeout");
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
        throw new System.NotImplementedException();
    }

    public override void aim(Vector3 direction)
    {
        throw new NotImplementedException();
    }
}