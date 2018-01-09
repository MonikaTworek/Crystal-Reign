﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using System.Linq;

public class ObjectsSpawner : MonoBehaviour {

    public GameObject staticBotPrefab;
    public GameObject healthCapsulePrefab;
    public static ObjectsSpawner instance;

    private Vector3 worldStart = new Vector3(-130.0f, -86.0f, -14.0f);
    private Vector3 worldEnd = new Vector3(121.0f, 116.0f, 220.0f);

    public int startBotCount = 5;


    // Use this for initialization
    void Start()
    {
        instance = this;
        //staticBots = new List<StaticBot>();
        for (int i = 0; i < startBotCount; i++)
            spawnStatic();

        //healthCapsules = new List<Transform>();
        for (int i = 0; i < 3; i++)
            spawnHealth();
    }

    // Update is called once per frame
    void Update() {

    }

    public void removeBot(Bot bot)
    {
        //staticBots.Remove(bot);
        for(int i = 0; i < 3; i++)
            spawnStatic();
        GameStatistics.instance.addPoint();
    }

    private void spawnStatic()
    {
        StaticBot newBot = Instantiate(staticBotPrefab).GetComponent<StaticBot>();
        newBot.findPlayer();
        do
        {
            newBot.transform.position = randomValidPosition();
        }
        while (Physics.OverlapSphere(newBot.transform.position, 4).Length > 1 || isInsideOtherObject(newBot.transform) || newBot.CanSeePlayer());
        //staticBots.Add(newBot);

        if (Random.Range(0, 10) == 0)
            spawnHealth();
    }

    private void spawnHealth()
    {
        Transform newHealth = Instantiate(healthCapsulePrefab).transform;
        do
        {
            newHealth.transform.position = randomValidPosition();
        }
        while (Physics.OverlapSphere(newHealth.transform.position, 1).Length > 1 || isInsideOtherObject(newHealth.GetChild(0)));

        RaycastHit hit;
        Physics.Raycast(newHealth.transform.position, new Vector3(0, -1, 0), out hit);
        newHealth.transform.position = hit.point + new Vector3(0, Random.Range(3.0f, 8.0f), 0);

        //healthCapsules.Add(newHealth);
    }

    private bool isInsideOtherObject(Transform origin)
    {
        Vector3 direction = new Vector3(0, 1, 0);
        int testFrom = Physics.RaycastAll(origin.position, direction, 2000).Length;
        List<Collider> listTemp = Physics.RaycastAll(origin.position + 2000 * direction, -direction, 2000).Select(x => x.collider).ToList();
        listTemp.Remove(origin.GetComponent<Collider>());
        int testTo = listTemp.Count;
        return testFrom != testTo;
    }

    private Vector3 randomValidPosition()
    {
        Vector3 result;
        do
        {
            result = new Vector3(Random.Range(worldStart.x, worldEnd.x), Random.Range(worldStart.y, worldEnd.y), Random.Range(worldStart.z, worldEnd.z));
        } while ((result.x > 16 && result.z < 8) || (result.x > 78 && result.z < 96));
        return result;
    }
}
