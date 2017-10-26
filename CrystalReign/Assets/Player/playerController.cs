using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 1;
    public float speedJump = 2;
    bool isGround = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ground"))
            isGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("ground"))
            isGround = false;
    }

    void Update()
    {
        Vector3 vector = transform.right * Input.GetAxis("Horizontal") * (-1) + transform.forward * Input.GetAxis("Vertical") * (-1);
        vector.y = 0;
        transform.Translate(vector);

        if(isGround && Input.GetAxis("Jump")!=0)
            rb.AddForce(new Vector3(0, speedJump, 0), ForceMode.Impulse);
        
    }
}