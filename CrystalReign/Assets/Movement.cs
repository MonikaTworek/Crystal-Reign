using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    private bool InAir = false;

    public float jumpForce;
    public float botSpeed;
    public double jumpTime = 0;

    // Use this for initialization
    void Start () {
		
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

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Floor" && InAir == true)
        {
            InAir = false;
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "FLoor" && InAir == true)
        {
            InAir = false;
        }
    }

    void OnCollisionExit(Collision other)
    {
        InAir = true;
    }

    // Update is called once per frame
    void Update () {

        if (jumpTime > 0)
            jumpTime -= Time.deltaTime;

        var playerTrans = GameObject.Find("Player").transform;

        var botPos = this.transform.position;
        this.transform.LookAt(playerTrans);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        this.transform.Translate(Vector3.forward * Time.deltaTime * botSpeed);

        if (CollideFree())
        {
            //Debug.Log("free");
        }
        else
        {
            //Debug.Log("not free");
            if (InAir == false && jumpTime <= 0) {

                Debug.Log("jump");
                jumpTime = 0.5;
                Vector3 jump = new Vector3(0.0f, 250, 0.0f);
                GetComponent<Rigidbody>().AddForce(jump * jumpForce);

            }
        }

    }
}