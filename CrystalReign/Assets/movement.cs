using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 3;
    public float speedjump = 5;
    bool ground = false;
    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ground"))
            ground = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("ground"))
            ground = false;
    }

    // Update is called once per frame
    void Update ()
    {
        //transform.right;
        float h=Input.GetAxis("Horizontal")*speed;
        float v=Input.GetAxis("Vertical")*speed;
        float j = rb.velocity.y +(ground ? Input.GetAxis("Jump") * speedjump : 0);
        rb.velocity = new Vector3(h, j, v);
    }
}
