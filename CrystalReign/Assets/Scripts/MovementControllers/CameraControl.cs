using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl: MonoBehaviour {
    public bool inverted = false;
    public float zoomOn = 15.0f;
    public float zoomOff = 60.0f;
    public float minAngle = -45;
    public float maxAngle = 45;
    Vector3 currentRotation; 
    public float rotateSensitivity = 5;
    float firstY;

    
    float distance;
    public LayerMask layerMask;

    Transform camera = null;
    public float delta = 0;

    private bool isCursor = false;
    private bool help = false;
    private bool isRightCursor = false;
    private float actuallyZoom;
    public float changeTempOfZoom = 10.0f;

    void Start () {
        if (transform.childCount != 1)
        {
            throw new System.Exception("Missing camera");
        }
        camera = transform.GetChild(0);
        currentRotation = transform.rotation.eulerAngles;
        firstY = currentRotation.x;
        distance = Vector3.Distance(transform.position, camera.position);
        actuallyZoom = camera.gameObject.GetComponent<Camera>().fieldOfView;
    }

    void Update () {
#if UNITY_EDITOR
        isCursor = (Input.GetKeyDown(KeyCode.C) ? !isCursor : isCursor);
#endif
        actuallyZoom = camera.gameObject.GetComponent<Camera>().fieldOfView;
        help = Input.GetMouseButtonDown(1);
        if (help)
        {
            isRightCursor = true;
            rotateSensitivity /= 2;
            transform.parent.GetComponent<PlayerControl>().rotateSensitivity /= 3;
        }
        if (isRightCursor)
        {
            if (Input.GetMouseButtonUp(1))
            {
                isRightCursor = false;
                rotateSensitivity *= 2;
                transform.parent.GetComponent<PlayerControl>().rotateSensitivity *= 3;
            }
        }

        if (!isCursor)
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
            camLocPos.z = -currentDistance + delta;
            camera.localPosition = camLocPos;
            if (isRightCursor)
            {
                camera.gameObject.GetComponent<Camera>().fieldOfView = Mathf.Max(zoomOn, actuallyZoom - changeTempOfZoom);
            }
            else
            {
                camera.gameObject.GetComponent<Camera>().fieldOfView = Mathf.Min(zoomOff, actuallyZoom + changeTempOfZoom);
            }
        }

    }
}
