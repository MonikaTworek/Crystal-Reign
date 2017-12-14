using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour {

    private RectTransform rt;
    private RectTransform rtParent;


    // Use this for initialization
    void Start () {
        rt = transform.GetChild(0).GetComponent<RectTransform>();
        rtParent = GetComponent<RectTransform>();

        setHP(1);
	}
	
	public void setHP(float value)
    {
        rt.sizeDelta = new Vector2(rtParent.sizeDelta.x * value, rt.sizeDelta.y);
    }
}
