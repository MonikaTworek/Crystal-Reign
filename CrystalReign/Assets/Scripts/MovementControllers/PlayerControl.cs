using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    //Variables
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    public float rotateSensitivity = 5;
    public float maxCeilDistance = 4.5f;
    
    private Transform body;
    public float bodyRadius = 1;
    private bool isCursor = false;

    private void Start()
    {
        body = transform.Find("Body");
    }

    void Update()
    {
        isCursor = (Input.GetKeyDown(KeyCode.C) ? !isCursor : isCursor);
        if (!isCursor){
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            CharacterController controller = GetComponent<CharacterController>();
            float old_y = moveDirection.y;
            float help = old_y;
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
            {
                //Applying gravity to the controller
                localDirection.y = old_y - gravity * Time.deltaTime;
            }
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.up, out hit, maxCeilDistance))
            {
                localDirection.y = help - maxCeilDistance /2 - gravity * Time.deltaTime * 2;
            }
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
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
    }

    private void rotateBodyBall(Vector3 localDirection, float rotationY)
    {
        body.Rotate(0, -rotationY, 0, Space.World);
        Quaternion rotation = Quaternion.AngleAxis(localDirection.z * Mathf.Rad2Deg * Time.deltaTime / bodyRadius, transform.right) * 
            Quaternion.AngleAxis(-localDirection.x * Mathf.Rad2Deg * Time.deltaTime / bodyRadius, transform.forward);
        body.rotation = rotation * body.rotation;
        
    }
}