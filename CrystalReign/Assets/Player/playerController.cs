using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 10000000;
    public float speedJump = 3000000;
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
        //Jak ruszamy mysza trza go przy pojsciu do przodku zrotowac
        Vector3 vector = transform.right  * Input.GetAxis("Vertical") + transform.forward  * Input.GetAxis("Horizontal")*(-1); 
        vector.y = 0;
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            //rb.velocity = vector * speed;
            transform.Translate(vector);
            //rb.AddForce(vector.normalized, ForceMode.VelocityChange);
        vector.x = 0;
        vector.z = 0;
        vector.y = (isGround ? Input.GetAxis("Jump") * speedJump : 0);
        //rb.AddForce(vector, ForceMode.VelocityChange);
        //rb.velocity = vector;
        transform.Translate(vector);
    }
}