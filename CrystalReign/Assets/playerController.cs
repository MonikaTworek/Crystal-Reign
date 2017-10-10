using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 3;
    public float speedJump = 5;
    bool isGround = false;
    Vector3 currentRotation;

    // Use this for initialization
    void Start()
    {
        currentRotation = transform.rotation.eulerAngles;
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

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y") * (-1);
        //float velocityX = Input.GetAxis("Horizontal") * speed;
        float velocityY = rb.velocity.y + (isGround ? Input.GetAxis("Jump") * speedJump : 0);
        //float velocityZ = Input.GetAxis("Vertical") * speed;
        rb.velocity = new Vector3(transform.forward.normalized.z*speed, velocityY, transform.forward.normalized.x);
        Vector3 rotateValue = new Vector3(mouseY, mouseX, 0);
        currentRotation += rotateValue;
        Quaternion rotation = Quaternion.Euler(currentRotation);
        transform.rotation = rotation;
    }

    void Lol()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y") * (-1);
        float velocityX = Input.GetAxis("Horizontal") * speed;
        float velocityY = rb.velocity.y + (isGround ? Input.GetAxis("Jump") * speedJump : 0);
        float velocityZ = Input.GetAxis("Vertical") * speed;
        rb.velocity = new Vector3(velocityX, velocityY, velocityZ);
        Vector3 rotateValue = new Vector3(mouseY, mouseX, 0);
        currentRotation += rotateValue;
        Quaternion rotation = Quaternion.Euler(currentRotation);
        transform.rotation = rotation;
    }
}