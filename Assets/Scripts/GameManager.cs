using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject inventory, pauseMenu;
    public GameObject handGun, AR, aRAmmo, hGAmmo, medKit, securityDoorKey,  marketKey, key, dog, money, banannas;

    void Update()
    {

    }

    public void IsGamePaused()
    {
        if (gameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        inventory.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    public void Inventory()
    {
        inventory.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    public void Restart()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
