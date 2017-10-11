using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 10;
    public float speedJump = 3;
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
        float help = Input.GetAxis("Horizontal");
        float velocityX = (help == 0 ? 0 : (help > 0 ? rb.velocity.x + transform.forward.normalized.x * speed * (-1) : rb.velocity.x + transform.forward.normalized.x * speed));//TODO: nie przeleciec przez podloge
        float velocityY = rb.velocity.y + (isGround ? Input.GetAxis("Jump") * speedJump : 0);
        help = Input.GetAxis("Vertical");
        float velocityZ = (help == 0 ? 0 : (help > 0 ? rb.velocity.z + transform.forward.normalized.z * speed : transform.forward.normalized.z * speed * (-1)));
        rb.velocity = new Vector3(velocityX, velocityY, velocityZ);
        Vector3 rotateValue = new Vector3(mouseY, mouseX, 0);
        currentRotation += rotateValue;
        Quaternion rotation = Quaternion.Euler(currentRotation);
        transform.rotation = rotation;
    }
}