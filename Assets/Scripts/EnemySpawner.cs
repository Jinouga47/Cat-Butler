using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject player, normal, big, small, spawnLocation_1, spawnLocation_2, spawnLocation_3;
    float timer = 2f;
    public float timerReset = 2f;
    public bool active = true, random = true;
    public int type = 1, enemyCount = 0;
    private GameObject[] locations = new GameObject[3];
    // Start is called before the first frame update
    void Start()
    {
        locations[0] = spawnLocation_1;
        locations[1] = spawnLocation_2;
        locations[2] = spawnLocation_3;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (timer < 0 && enemyCount < 5)
            {
                if(random)
                    Spawn(Random.Range(-1, 2));
                else
                    Spawn(type);
                timer = timerReset;
            }
            timer -= Time.deltaTime;
        }
    }

    void Spawn(int type)
    {
        GameObject Enemy;
        switch (type)
        {
            case (int)Type.Patrol:
                Enemy = Instantiate(normal, locations[Random.Range(0, 3)].transform.position, transform.rotation);
                Enemy.GetComponent<EnemyAI>().Spawn(false);
                enemyCount++;
                break;

            case (int)Type.Chase:
                Enemy = Instantiate(big, locations[Random.Range(0, 3)].transform.position, transform.rotation);
                Enemy.GetComponent<EnemyAI>().Spawn(true);
                enemyCount++;
                break;

            //case (int)Type.Small:
            //    Enemy = Instantiate<GameObject>(small, transform.position, transform.rotation);
            //    Enemy.GetComponent<EnemyAI>().Spawn(type);
            //    break;
        }
    }

    enum Type
    {
        Patrol,
        Chase
        //Small
    }
}
