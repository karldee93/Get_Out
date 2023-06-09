using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public GameObject item;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SpawnDroppedItem()
    {
        Vector3 playerPos = new Vector3(player.position.x + 3, player.position.y, player.position.z);

        Instantiate(item, playerPos, Quaternion.identity);
    }
}
