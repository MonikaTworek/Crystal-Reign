using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {
    public GameObject target;
    public float minAngle = -45;
    public float maxAngle = 45;
    Vector3 currentRotation; 
    float rotate = 5;
    float firstY;

    void Start () {
        currentRotation = transform.rotation.eulerAngles;
        firstY = currentRotation.x;
    }
	
	void Update () {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y")*(-1);
        if (mouseY*rotate+currentRotation.x > firstY + maxAngle || mouseY * rotate + currentRotation.x < firstY + minAngle)
            mouseY = 0;
        target.transform.Rotate(0, mouseX * rotate, 0);
        Vector3 rotateValue = new Vector3(mouseY, mouseX, 0);
        currentRotation += rotateValue * rotate;
        Quaternion rotation = Quaternion.Euler(currentRotation);
        transform.rotation = rotation;
    }
}
