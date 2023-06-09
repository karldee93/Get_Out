using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAccess : MonoBehaviour
{
    public bool playerHasKey;
    public bool playerHasMarketKey;
    public bool hasSecurityKey;
    public GameObject player, boss;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void OpenDoor()
    {
        if (playerHasKey)
        {
            HapticFeedback.Vibrate();
            player.GetComponent<EquipmentManager>().keyUsed = true;
            //player.GetComponent<EquipmentManager>().keyUsed = true;
            this.gameObject.SetActive(false);
        }
        else
        {
            // key required message
        }
        if (playerHasMarketKey)
        {
            HapticFeedback.Vibrate();
            player.GetComponent<EquipmentManager>().keyUsed = true;
            this.gameObject.SetActive(false);
        }
        if (hasSecurityKey)
        {
            HapticFeedback.Vibrate();
            player.GetComponent<EquipmentManager>().keyUsed = true;
            this.gameObject.SetActive(false);
            boss.SetActive(true);
        }
    }
}
