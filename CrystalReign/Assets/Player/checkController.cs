using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkController : MonoBehaviour
{
    public float speed = 10000000;
    public float speedJump = 3000000;
    bool isGround = false;

    void Start()
    {
 
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
        Vector3 vector = transform.right * Input.GetAxis("Horizontal") * (-1) + transform.forward * Input.GetAxis("Vertical")*(-1);
        vector.y = 0;
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            transform.Translate(vector);
        vector.x = 0;
        vector.z = 0;
        vector.y = (isGround ? Input.GetAxis("Jump") * speedJump : 0);
        transform.Translate(vector);
    }
}
