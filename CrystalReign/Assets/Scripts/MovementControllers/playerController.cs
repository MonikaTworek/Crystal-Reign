using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float speed = 10;
    public float radiusBB7;
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
        radiusBB7 = GetComponents < SphereCollider >()[0].bounds.size.x/2;
    }

    void Update()
    {
        RaycastHit hitInfo;
        isGrounded = Physics.SphereCast(transform.position, radiusBB7, - transform.up, out hitInfo, groundMaxDistance, groundRayLayerMask);
        velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        velocity = transform.TransformDirection(velocity).normalized * speed; 
        velocity += Vector3.up * (Input.GetButtonDown("Jump") && isGrounded ? speedJump : rb.velocity.y);
        rb.velocity = velocity;
        Vector3 camfwd = camera.forward;
        camfwd.y = 0;
        transform.forward = camfwd;
    }
}


