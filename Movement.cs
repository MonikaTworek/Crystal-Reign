using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void FacePlayer()
    {
        var playerTrans = GameObject.Find("Player").transform;
        var botPos = this.transform.position;

        this.transform.LookAt(playerTrans);

        if(playerTrans.position.y != botPos.y)
        {

        }
    }

    bool CollideFree()
    {
        var player = GameObject.Find("Player");
        var botPos = this.transform.position;

        RaycastHit hit;
        if(Physics.Raycast(botPos, (player.transform.position - botPos).normalized, out hit, 3) && (hit.collider.gameObject != player))
        {
            Debug.Log(hit.collider.gameObject);
            return false;
        }
            
        return true;
    }


    // Update is called once per frame
    void Update () {

        var playerTrans = GameObject.Find("Player").transform;
        var botPos = this.transform.position;

        if (CollideFree())
        {
            Debug.Log("free");
            this.transform.LookAt(playerTrans);
            this.transform.Translate(Vector3.forward * Time.deltaTime);
        }
        else
        {
            Debug.Log("not free");

        }
            
	}
}