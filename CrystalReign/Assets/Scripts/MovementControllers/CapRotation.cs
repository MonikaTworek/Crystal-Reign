using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapRotation : MonoBehaviour {

    public Transform Camera;
    public float CapRadius;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit hit;
        Vector3 shootPoint = Physics.Raycast(Camera.position, Camera.forward, out hit) ? hit.point : Camera.position + Camera.forward.normalized * 100;
        float distance = Vector3.Distance(shootPoint, transform.position);
        float angle = (Mathf.Asin((shootPoint.y - transform.position.y) / distance) + 
            Mathf.Acos(CapRadius / distance))/Mathf.PI*180 - 90;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(-angle, 0.0f, 0.0f), 0.7f);
	}
}
