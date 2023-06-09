using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Ammo_UI : MonoBehaviour
{
    public int ammoBagHG;
    public int currentInClipHG;
    public int ammoBagAR;
    public int currentInClipAR;
    // sets variables to hold text objects
    public Text inClipHG;
    public Text bagHG;
    public Text inClipAR;
    public Text bagAR;
    void Update()
    {
        
    }
    public void UpdateHGAmmoUI()
    {
        inClipHG.GetComponent<Text>().text = "Clip: " + currentInClipHG.ToString();
        bagHG.GetComponent<Text>().text = "Ammo Bag: " + ammoBagHG.ToString() + "/90";
    }

    public void UpdateARAmmoUI()
    {
        // updates UI display with current variable values
        inClipAR.GetComponent<Text>().text = "Clip: " + currentInClipAR.ToString();
        bagAR.GetComponent<Text>().text = "Ammo Bag: " + ammoBagAR.ToString() + "/210";
    }

    public void UpdateNoActiveGunUI()
    {
        inClipHG.GetComponent<Text>().text = " ";
        bagHG.GetComponent<Text>().text = " ";
        inClipAR.GetComponent<Text>().text = " ";
        bagAR.GetComponent<Text>().text = " ";
    }
}
