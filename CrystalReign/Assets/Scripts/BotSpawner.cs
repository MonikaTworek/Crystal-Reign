using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using System.Linq;

public class BotSpawner : MonoBehaviour {

    public GameObject staticBotPrefab;
    public static BotSpawner instance;

    private List<Bot> bots;
    private Vector3 worldStart = new Vector3(-130.0f, -86.0f, -14.0f);
    private Vector3 worldEnd = new Vector3(121.0f, 116.0f, 220.0f);

    public int startBotCount = 5;


    // Use this for initialization
    void Start()
    {
        instance = this;
        bots = new List<Bot>();
        for (int i = 0; i < startBotCount; i++)
            spawnStatic();
    }

    // Update is called once per frame
    void Update() {

    }

    public void removeBot(Bot bot)
    {
        bots.Remove(bot);
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
        while (Physics.OverlapSphere(newBot.transform.position, 4).Length > 1 || isInsideOtherObject(newBot) || newBot.CanSeePlayer());
        bots.Add(newBot);
    }

    private bool isInsideOtherObject(StaticBot origin)
    {
        Vector3 direction = new Vector3(0, 1, 0);
        int testFrom = Physics.RaycastAll(origin.transform.position, direction).Length;
        List<Collider> listTemp = Physics.RaycastAll(origin.transform.position + 2000 * direction, -direction, 2000).Select(x => x.collider).ToList();
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
