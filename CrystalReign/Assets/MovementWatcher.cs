using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementWatcher : MonoBehaviour {

	private bool inAir = false;
    private bool seen = false;

    public float seeWidth;
    public double walkTime = 3;
    public float rotateTime = 1;
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
            //Debug.Log(hit.collider.gameObject);
            return false;
        }
            
        return true;
    }

    bool watchForPlayer()
    {
        var player = GameObject.Find("Player");
        var botPos = this.transform.position;

        RaycastHit hit;
        RaycastHit[] tab = Physics.SphereCastAll(botPos, seeWidth, this.transform.forward, 50);
        for(int i = 0; i < tab.Length; i++)
        {
            //Debug.Log(tab[i].collider);
            if(tab[i].collider.tag == "Player")
            {
                //Debug.Log("TAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAG");
                if (Physics.Raycast(botPos, (player.transform.position - botPos).normalized, out hit, 50) && (hit.collider.gameObject == player))
                {
                    Debug.Log("TAGHARD");
                    return true;
                }
                else
                    return false;
            }
        }
        return false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Floor" && inAir == true)
        {
            inAir = false;
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "FLoor" && inAir == true)
        {
            inAir = false;
        }
    }

    void OnCollisionExit(Collision other)
    {
        inAir = true;
    }

    // Update is called once per frame
    void Update () {

        Debug.DrawRay(this.transform.position, (this.transform.forward * 50), Color.white, 0.0f, true);
        //Debug.Log(watchForPlayer());
        if (watchForPlayer()) {
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
                if (inAir == false && jumpTime <= 0) {

                    //Debug.Log("jump");
                    jumpTime = 0.5;
                    Vector3 jump = new Vector3(0.0f, 250, 0.0f);
                    GetComponent<Rigidbody>().AddForce(jump * jumpForce);

                }
            }
        }
        else
        {

            if (walkTime > 0)
            {
                walkTime -= Time.deltaTime;
                //Debug.Log(walkTime);
                this.transform.Translate(Vector3.forward * Time.deltaTime * botSpeed);
            }
            else
            {
                rotateTime -= Time.deltaTime;
                //Debug.Log("taghard");
                if (rotateTime > 0)
                {
                    this.transform.Rotate(Vector3.up * Time.deltaTime * 90, Space.Self);
                }
                else
                {
                    walkTime += 3;
                    rotateTime += 1 + Time.deltaTime;
                }
            }
        }
    }
}
