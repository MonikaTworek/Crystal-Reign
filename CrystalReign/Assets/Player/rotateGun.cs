using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateGun : MonoBehaviour {
    public Transform cameraTarget;
    public Transform head;
    public GameObject playerTarget;
    public float distance;
	void Start () {
		
	}
	
	void Update () {
        Vector3 hitPoint = cameraTarget.position + cameraTarget.forward*1000;
        RaycastHit hitPlace;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)),out hitPlace))
        {
            hitPoint = hitPlace.point;
        }
        //hitPoint.y = 0;
        hitPoint.x = 0;
        head.LookAt(hitPoint);
    }
}
