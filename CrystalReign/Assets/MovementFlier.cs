using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementFlier : MonoBehaviour
{

    public float botSpeed;
    public double floatTime = 0;
    public double dashTime, dashWait, dashPower;

    private double currentDashWait;
    private double currentDashTime;
    private double fallTime = 0;
    public bool InAir = true;

    // Use this for initialization
    void Start()
    {
        currentDashWait = dashWait;
        currentDashTime = dashTime;
    }

    bool CollideFree()
    {
        var player = GameObject.Find("Player");
        var botPos = this.transform.position;
        var playerPos = player.transform.position;

        Vector3 pointer = new Vector3(playerPos.x, botPos.y, playerPos.z);
        RaycastHit hit;
        Debug.DrawRay(botPos, (pointer - botPos).normalized, Color.white, 0.0f, true);
        if (Physics.Raycast(botPos, (pointer - botPos).normalized, out hit, 10) && (hit.collider.gameObject != player))
        {
            //Debug.Log(hit.collider.gameObject);
            return false;
        }

        return true;
    }

    bool FreeRoad()
    {
        var player = GameObject.Find("Player");
        var botPos = this.transform.position;
        var playerPos = player.transform.position;

        RaycastHit hit;
        Debug.DrawRay(botPos, (player.transform.position - botPos).normalized, Color.white, 0.0f, true);
        if (Physics.Raycast(botPos, (player.transform.position - botPos).normalized, out hit, 10) && (hit.collider.gameObject != player))
        {
            //Debug.Log(hit.collider.gameObject);
            return false;
        }

        return true;
    }

    bool CollideNear()
    {
        var player = GameObject.Find("Player");
        var botPos = this.transform.position;
        var playerPos = player.transform.position;
        RaycastHit hit;
        Vector3 pointer = new Vector3(playerPos.x, botPos.y, playerPos.z);

        if (Physics.Raycast(botPos, (pointer - botPos).normalized, out hit, 3) && (hit.collider.gameObject != player))
        {
            //Debug.Log(hit.collider.gameObject);
            return true;
        }
        return false;
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
        if (other.gameObject.tag == "Floor" && InAir == true)
        {
            InAir = false;
        }
    }

    void OnCollisionExit(Collision other)
    {
        InAir = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (currentDashWait > 0)
            currentDashWait -= Time.deltaTime;
        else
        {
            
            if(currentDashTime == dashTime)
            {
                botSpeed *= (float)dashPower;
            }
            currentDashTime -= Time.deltaTime;
            if (currentDashTime <= 0)
            {
                currentDashTime = dashTime;
                currentDashWait = dashWait;
                botSpeed /= (float)dashPower;
            }
            
        }
        
        var playerTrans = GameObject.Find("Player").transform;

        var botPos = this.transform.position;

        if (!CollideNear())
        {
            this.transform.LookAt(playerTrans);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            this.transform.Translate(Vector3.forward * Time.deltaTime * botSpeed);
        }

        if (CollideFree())
        {
            if (floatTime > 0)
            {
                floatTime -= Time.deltaTime;
                this.transform.Translate(Vector3.up * Time.deltaTime * botSpeed * Mathf.Ceil((float)floatTime));
            }
            else
                floatTime = 0;
            Debug.Log(InAir);
            Debug.Log(playerTrans.position.y);
            Debug.Log(botPos.y);
            if (playerTrans.position.y < botPos.y && InAir == true && FreeRoad())
            {
                Debug.Log("WHAAAT");
                fallTime += Time.deltaTime;
                this.transform.Translate(Vector3.down * Time.deltaTime * botSpeed * Mathf.Ceil((float)fallTime));
            }
            else
                fallTime = 0;
            //Debug.Log("free");
        }
        else
        {
            //Debug.Log("not free");
            floatTime += Time.deltaTime;
            this.transform.Translate(Vector3.up * Time.deltaTime * botSpeed * Mathf.Ceil((float)floatTime));

        }
    }
}