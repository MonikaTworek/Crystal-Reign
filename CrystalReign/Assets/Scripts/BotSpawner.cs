using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using System.Linq;

public class BotSpawner : MonoBehaviour {

    public StaticBot staticBotPrefab;
    public static BotSpawner instance;

    private List<StaticBot> staticBots;
    private Vector3 worldStart = new Vector3(-130.0f, -86.0f, -14.0f);
    private Vector3 worldEnd = new Vector3(121.0f, 116.0f, 208.0f);


    // Use this for initialization
    void Start()
    {
        instance = this;
        staticBots = new List<StaticBot>();
        for (int i = 0; i < 5; i++)
            spawnStatic();
    }

    // Update is called once per frame
    void Update() {

    }

    public void removeBot(StaticBot bot)
    {
        staticBots.Remove(bot);
        for(int i = 0; i < 5; i++)
            spawnStatic();
        GameStatistics.instance.addPoint();
    }

    private void spawnStatic()
    {
        StaticBot newBot = Instantiate(staticBotPrefab);
        do
        {
            newBot.transform.position = randomValidPosition();
        }
        while (Physics.OverlapSphere(newBot.transform.position, 4).Select(x => x.GetComponent<Transform>()).ToList().Count > 1);
        staticBots.Add(newBot);
    }

    private Vector3 randomValidPosition()
    {
        return new Vector3(Random.Range(worldStart.x, worldEnd.x), Random.Range(worldStart.y, worldEnd.y), Random.Range(worldStart.z, worldEnd.z));
    }
}
