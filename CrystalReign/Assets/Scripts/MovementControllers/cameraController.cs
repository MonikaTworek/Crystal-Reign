using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class cameraController : MonoBehaviour
{
    public bool inverted = false;
    public float minAngle = -45;
    public float maxAngle = 45;
    Vector3 currentRotation;
    public float rotateSensitivity = 5;
    float firstY;

    float distance;
    public LayerMask layerMask;

    Transform camera = null;


    void Start()
    {
        if (transform.childCount != 1)
        {
            throw new System.Exception("Missing camera");
        }
        camera = transform.GetChild(0);
        currentRotation = transform.rotation.eulerAngles;
        firstY = currentRotation.x;
        distance = Vector3.Distance(transform.position, camera.position);
    }
    void Update()
    {
        if (camera == null) return;
        float mouseY = (inverted ? 1 : -1) * Input.GetAxis("Mouse Y");
        if (mouseY * rotateSensitivity + currentRotation.x > firstY + maxAngle || mouseY * rotateSensitivity + currentRotation.x < firstY + minAngle)
        {
            mouseY = 0;
        }
        Vector3 rotateValue = new Vector3(mouseY, 0, 0);
        currentRotation += rotateValue * rotateSensitivity;
        Quaternion rotation = Quaternion.Euler(currentRotation);
        transform.localRotation = rotation;

        RaycastHit hit;
        float currentDistance = distance;
        if (Physics.Raycast(transform.position, camera.position - transform.position, out hit, distance, layerMask))
        {
            currentDistance = Vector3.Distance(transform.position, hit.point);
        }
        Vector3 camLocPos = camera.localPosition;
        camLocPos.z = -currentDistance;
        camera.localPosition = camLocPos;


    }
}