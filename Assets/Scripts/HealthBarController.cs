using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // this is required to load a new scene
public class HealthBarController : MonoBehaviour
{
    public Slider healthSlider; // reference to slider component on the game object
    public GameObject player; // reference so can call functions from scripts attached to player
    public GameObject HUD; // same but so can call scripts from canvas

    void Update()
    {
        //if (player.GetComponent<PlayerMovement>().currentHP <= 0)
        //{
        //    SceneManager.LoadScene(0);
        // }
    }
    public void SetMaxHealth(float health) // gets health value from passed in value of maxHealth from movement script
    {
        healthSlider.maxValue = health; // Sets value of health bar slider to value of health
        healthSlider.value = health; // sets the actual visual health as the value of the passed in variable
    }

    public void AddHealth(string tag)
    {
        if (player.GetComponent<PlayerController>().currentHP > 50)
        {
            player.GetComponent<PlayerController>().currentHP = 100; // ensure player does not get over 100 hp
            healthSlider.value = player.GetComponent<PlayerController>().currentHP;
        }
        else
        {
            player.GetComponent<PlayerController>().currentHP += 50;
            healthSlider.value = player.GetComponent<PlayerController>().currentHP;
        }
    }

    void SelfTerminate()
    {
        Destroy(gameObject);
    }
}
