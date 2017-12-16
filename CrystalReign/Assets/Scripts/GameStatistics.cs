using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStatistics : MonoBehaviour {

    public Text pointsText;
    public static GameStatistics instance;

    private int points = 0;

    // Use this for initialization
    void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addPoint()
    {
        points++;
        pointsText.text = points.ToString();
        Debug.Log(points);
    }
}
