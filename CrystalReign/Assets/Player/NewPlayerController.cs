using UnityEngine;
using System.Collections;

public class NewPlayerController : MonoBehaviour
{
    //Variables
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    public float rotateSensitivity = 5;

    private Transform bodyPivotX;
    private Transform bodyPivotZ;
    private Transform body;
    private float body_radius = 1;

    private void Start()
    {
        body = transform.Find("Body");
        bodyPivotX = transform.Find("BodyPivotX");
        bodyPivotZ = transform.Find("BodyPivotZ");
    }

    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        float old_y = moveDirection.y;
        Vector3 localDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //Multiply it by speed.
        localDirection *= speed;

        // is the controller on the ground?
        if (controller.isGrounded)
        {
            //Jumping
            if (Input.GetButton("Jump"))
            {
                old_y = jumpSpeed;
                localDirection.y = old_y;
            }

        }
        else
            //Applying gravity to the controller
            localDirection.y = old_y - gravity * Time.deltaTime;

        //Feed moveDirection with input.
        moveDirection = transform.TransformDirection(localDirection);

        //Making the character move
        controller.Move(moveDirection * Time.deltaTime);

        //Rotate character
        float mouseX = Input.GetAxis("Mouse X") * rotateSensitivity;
        transform.Rotate(new Vector3(0, mouseX, 0));

        //Animate body
        rotateBodyBall(localDirection, mouseX);
    }

    private void rotateBodyBall(Vector3 localDirection, float rotationY)
    {
        body.Rotate(0, -rotationY, 0, Space.World);
        body.parent = bodyPivotX;
        bodyPivotX.Rotate(new Vector3(localDirection.z*Time.deltaTime / (2 * Mathf.PI * body_radius) * 360, 0, 0));
        body.parent = bodyPivotZ;
        bodyPivotZ.Rotate(new Vector3(0, localDirection.x * Time.deltaTime / (2 * Mathf.PI * body_radius) * 360, 0));
        
    }
}