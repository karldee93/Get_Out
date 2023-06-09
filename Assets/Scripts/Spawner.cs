using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int enemyNum = 0;
    public int fullEnemies = 10;
    public int currentEnemies;
    public int enemyKilled = 0;
    GameObject gameManager;
    public GameObject[] spawners;
    public GameObject enemy;
    float spawnTimer = 5f;
    public bool spawn;
    // Start is called before the first frame update
    void Start()
    {
        spawners = new GameObject[6];

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetChild(i).gameObject; // get the child of the spawner object relative to the i value and set it as that game object
        }
        spawn = true;
        enemyNum = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawn)
        {
            for (int i = 0; i < enemyNum; i++) // if i is less then enemy number then spawn a new enemy until it is == enemy num
            {
                spawnTimer -= 1 * Time.deltaTime;
                if (spawnTimer <= 0)
                {
                    SpawnEnemy();
                    currentEnemies += 1;
                    spawnTimer = 5f;
                }
            }
            if (currentEnemies == fullEnemies)
            {
                spawn = false;
            }
        }
    }
    void SpawnEnemy()
    {
        int spawnerID = Random.Range(0, spawners.Length);
        // this will instantiate an enemy prefab at a random spawn point of the array and at the same rotation
        Instantiate(enemy, spawners[spawnerID].transform.position, spawners[spawnerID].transform.rotation);
    }

    void StartSpawn()
    {
        //spawn = true;
        //enemyNum = 10;

        //for (int i = 0; i < enemyNum; i++) // if i is less then enemy number then spawn a new enemy until it is == enemy num
        //{
        //    SpawnEnemy();
        //}
    }

    public void SpawnControl()
    {

    }
}

