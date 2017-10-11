using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    Rigidbody rb;
    public float rotate = 5;
    public float speed = 1;
    public float speedJump = 3000000;
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
        Vector3 vector = transform.right  * Input.GetAxis("Horizontal") + transform.forward  * Input.GetAxis("Vertical");
        vector.y = 0;
        rb.AddForce(vector.normalized* speed, ForceMode.VelocityChange);
        vector.x = 0;
        vector.z = 0;
        vector.y = (isGround ? Input.GetAxis("Jump") * speedJump : 0);
        rb.AddForce(vector, ForceMode.VelocityChange);
        Vector3 rotateValue = new Vector3(mouseY, mouseX, 0);
        currentRotation += rotateValue*rotate;
        Quaternion rotation = Quaternion.Euler(currentRotation);
        transform.rotation = rotation;
    }
}