using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentControl : MonoBehaviour {

    public GameObject target;
    NavMeshAgent agent;

	void Start () {
        target = GameObject.Find("Player");
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(target.transform.position);
	}
    void Update () {
        target = GameObject.Find("Player");
        agent.SetDestination(target.transform.position);
    }
	

}
