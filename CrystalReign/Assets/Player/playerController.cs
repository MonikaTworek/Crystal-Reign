using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float speed = 10;
    public float speedJump = 7;
    public float gravity = 20;
    private Vector3 moveDirection = Vector3.zero;
    bool isGround = false;
    Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ground"))
        {
            isGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("ground"))
            isGround = false;
    }

    void Update()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection).normalized;
        moveDirection *= speed;
        if (Input.GetButtonDown("Jump") && isGround)
            moveDirection.y = speedJump;
        //moveDirection.y -= gravity;
        moveDirection.y += rb.velocity.y;
        rb.velocity = moveDirection;
        


    }   
    
    void check()
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection).normalized;
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = speedJump;
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
