using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    private bool InAir = false;
    private Vector3 moveDirection = Vector3.zero;
    private float timePlayerNotSeen = 0;

    public enum states { chasingPlayer, followingLastTrack, returning };
    public states state;
    public float speed = 6.0F;
    public float gravity = 20.0F;
    public float jumpForce;
    public float botSpeed;
    public double jumpTime = 0;
    public Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    bool CollideFree()
    {
        var player = GameObject.Find("BB7");
        var botPos = this.transform.position;

        RaycastHit hit;
        if (Physics.Raycast(botPos, (player.transform.position - botPos).normalized, out hit, 3) && (hit.collider.gameObject != player))
        {
            //Debug.Log(hit.collider.gameObject);
            return false;
        }

        return true;
    }

    bool CeilingCollideFree()
    {
        var botPos = this.transform.position;

        RaycastHit hit;
        Vector3 movedBotPos = botPos + (this.transform.forward * 3);
        Vector3 ceilingVector = new Vector3(botPos.x, 99999, botPos.z) + (this.transform.forward * 3);
        Debug.Log(this.transform.forward + " " + ceilingVector);
        if (Physics.Raycast(botPos, (this.transform.up - botPos), out hit))
        {
            Debug.DrawLine(movedBotPos, this.transform.up);
            Debug.Log(hit.collider.gameObject + "false");
            return false;
        }
        return true;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "ground" && InAir == true)
        {
            InAir = false;
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "ground" && InAir == true)
        {
            InAir = false;
        }
    }

    void OnCollisionExit(Collision other)
    {
        InAir = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpTime > 0)
            jumpTime -= Time.deltaTime;

        var playerTrans = GameObject.Find("BB7").transform;

        var botPos = this.transform.position;
        this.transform.LookAt(playerTrans);

        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        this.transform.Translate(Vector3.forward * Time.deltaTime * botSpeed);

        CharacterController controller = GetComponent<CharacterController>();
        float old_y = moveDirection.y;
        moveDirection = new Vector3(0, 0, rb.transform.forward.z);
        //Feed moveDirection with input. 
        moveDirection = transform.TransformDirection(moveDirection);
        //Multiply it by speed. 
        moveDirection *= speed;
        //Applying gravity to the controller 
        moveDirection.y = old_y - gravity * Time.deltaTime;
        //Making the character move 
        controller.Move(moveDirection * Time.deltaTime);

        if (CollideFree())
        {
            //Debug.Log("free");
        }
        else
        {
            //Debug.Log("not free");
            if (InAir == false && jumpTime <= 0)
            {
                //Debug.Log("jump");
                jumpTime = 0.5;
                Vector3 jump = new Vector3(0.0f, 250, 0.0f);
                GetComponent<Rigidbody>().AddForce(jump * jumpForce);
            }
        }
    }
    void FixedUpdate()
    {

    }
}