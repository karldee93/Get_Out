using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mannequins : MonoBehaviour
{
    public bool playerHasMoney;
    public bool playerHasBannanas;
    public bool playerHasDog;
    GameObject player;
    public GameObject keyPrize, gunPrize, ammoPrize, healthKitPrize;
    public Transform spawnKey, spawnGun, spawnAmmo, spawnHealth;
    int keyPrizeNum = 1, gunPrizeNum = 1, ammoPrizeNum = 1, healthPrizeNum = 1;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Bill()
    {
        if (playerHasMoney && player.GetComponent<EquipmentManager>().dogUsed == true && player.GetComponent<EquipmentManager>().banannaUsed == true)
        {
            HapticFeedback.Vibrate();
            player.GetComponent<EquipmentManager>().moneyUsed = true;
            if (keyPrizeNum > 0)
            {
                Instantiate(keyPrize, spawnKey.position, Quaternion.identity);
                keyPrizeNum -= 1;
            }
        }
    }

    public void Kingsley()
    {
        if (playerHasDog)
        {
            HapticFeedback.Vibrate();
            player.GetComponent<EquipmentManager>().dogUsed = true;
            if (gunPrizeNum > 0)
            {
                Instantiate(gunPrize, spawnGun.position, Quaternion.identity);
                gunPrizeNum -= 1;
            }
        }
    }

    public void Gary()
    {
        if (playerHasBannanas)
        {
            HapticFeedback.Vibrate();
            player.GetComponent<EquipmentManager>().banannaUsed = true;
            if (ammoPrizeNum > 0)
            {
                Instantiate(ammoPrize, spawnAmmo.position, Quaternion.identity);
                ammoPrizeNum -= 1;
            }
            if (healthPrizeNum > 0)
            {
                Instantiate(healthKitPrize, spawnHealth.position, Quaternion.identity);
                healthPrizeNum -= 1;
            }
        }
    }
}
