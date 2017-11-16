using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour
{
    //Variables 
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    public Transform camera;
    public float rotateSensitivity = 5;


    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        float old_y = moveDirection.y;
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //Feed moveDirection with input. 
        moveDirection = transform.TransformDirection(moveDirection);
        //Multiply it by speed. 
        moveDirection *= speed;
        // is the controller on the ground? 
        if (controller.isGrounded)
        {
            //Jumping 
            if (Input.GetButton("Jump"))
            {
                old_y = jumpSpeed;
            }

        }
        //Applying gravity to the controller 
        moveDirection.y = old_y - gravity * Time.deltaTime;
        //Making the character move 
        controller.Move(moveDirection * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * rotateSensitivity;
        transform.Rotate(new Vector3(0, mouseX, 0));

    }
}