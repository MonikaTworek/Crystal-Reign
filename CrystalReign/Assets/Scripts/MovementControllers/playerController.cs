<<<<<<< HEAD:CrystalReign/Assets/Player/playerController.cs
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float speed = 10;
    public float speedJump = 7;
    private Vector3 velocity = Vector3.zero;
    bool isGrounded = false;
    Rigidbody rb;
    public LayerMask groundRayLayerMask;
    public float groundMaxDistance;
    public Transform camera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, -transform.up, groundMaxDistance, groundRayLayerMask);
        velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        velocity = transform.TransformDirection(velocity).normalized *speed;
        velocity += Vector3.up * (Input.GetButtonDown("Jump") && isGrounded ? speedJump : rb.velocity.y);
        rb.velocity = velocity;
        Vector3 camfwd = camera.forward;
        camfwd.y = 0;
        transform.forward = camfwd;
    }   
    
}


=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float speed = 10;
    public float speedJump = 7;
    private Vector3 velocity = Vector3.zero;
    bool isGrounded = false;
    Rigidbody rb;
    public LayerMask groundRayLayerMask;
    public float groundMaxDistance;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, -transform.up, groundMaxDistance, groundRayLayerMask);
        velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        velocity = transform.TransformDirection(velocity).normalized *speed;
        velocity += Vector3.up * (Input.GetButtonDown("Jump") && isGrounded ? speedJump : rb.velocity.y);
        rb.velocity = velocity;
    }   
    
}


>>>>>>> master:CrystalReign/Assets/Scripts/MovementControllers/playerController.cs
