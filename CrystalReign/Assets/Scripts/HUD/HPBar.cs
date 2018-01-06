using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour {

    private RectTransform rt;
    private RectTransform rtParent;

    private float targetWidth;
    [Range(0,1)]
    public float speed = 0.7f;

    // Use this for initialization
    void Start () {
        rt = transform.GetChild(0).GetComponent<RectTransform>();
        rtParent = GetComponent<RectTransform>();
        targetWidth = rt.sizeDelta.x;
	}
	
	public void setHP(float value)
    {
        targetWidth = rtParent.sizeDelta.x * value;
    }

    public void FixedUpdate()
    {
        rt.sizeDelta = new Vector2(Mathf.Lerp(rt.sizeDelta.x, targetWidth, speed), rt.sizeDelta.y);
    }

}
