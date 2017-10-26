using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {
    Vector3 currentRotation;
    public float rotate = 5;

    void Start () {
        currentRotation = transform.rotation.eulerAngles;
    }
	
	void Update () {
        //zakres kamery
        //jak oberwiesz to ucieka kamera
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y") * (-1);
        Vector3 rotateValue = new Vector3(mouseY, mouseX, 0);
        currentRotation += rotateValue * rotate;
        Quaternion rotation = Quaternion.Euler(currentRotation);
        transform.rotation = rotation;
    }
}
