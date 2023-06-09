using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemInteraction : MonoBehaviour
{
    private Inventory inventory;
    public Text interactItemText, interactionText, doorInteraction, kingInteraction, garyInteraction, billInteraction;
    public Text NoteStart, NoteSupermarket, NoteMarketEnterenceL, NoteMarketEnterenceR, NoteMusicStore;
    private float maxCastDistance = 5;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hit, 20))
        {
            if (hit.collider.tag == "Item")
            {
                interactItemText.GetComponent<Text>().enabled = true;
                //Debug.Log(hit.collider.tag);
            }
            if (hit.collider.tag != "Item")
            {
                interactItemText.GetComponent<Text>().enabled = false;
            }

            if (hit.collider.tag == "Interactable")
            {
                interactionText.GetComponent<Text>().enabled = true;
                //Debug.Log(hit.collider.tag);
            }
            if (hit.collider.tag != "Interactable")
            {
                interactionText.GetComponent<Text>().enabled = false;
            }

            if (hit.collider.tag == "Door")
            {
                doorInteraction.GetComponent<Text>().enabled = true;
                //Debug.Log(hit.collider.tag);
            }
            if (hit.collider.tag != "Door")
            {
                doorInteraction.GetComponent<Text>().enabled = false;
            }

            if (hit.collider.tag == "Kingsley")
            {
                kingInteraction.GetComponent<Text>().enabled = true;
                //Debug.Log(hit.collider.tag);
            }
            if (hit.collider.tag != "Kingsley")
            {
                kingInteraction.GetComponent<Text>().enabled = false;
            }

            if (hit.collider.tag == "Gary")
            {
                garyInteraction.GetComponent<Text>().enabled = true;
                //Debug.Log(hit.collider.tag);
            }
            if (hit.collider.tag != "Gary")
            {
                garyInteraction.GetComponent<Text>().enabled = false;
            }

            if (hit.collider.tag == "Bill")
            {
                billInteraction.GetComponent<Text>().enabled = true;
                //Debug.Log(hit.collider.tag);
            }
            if (hit.collider.tag != "Bill")
            {
                billInteraction.GetComponent<Text>().enabled = false;
            }



            if (hit.collider.tag == "Note")
            {
                if(hit.collider.name == "NoteStart")
                {
                    NoteStart.GetComponent<Text>().enabled = true;
                }
                if (hit.collider.name == "NoteSupermarket")
                {
                    NoteSupermarket.GetComponent<Text>().enabled = true;
                }
                if (hit.collider.name == "NoteMarketEnterenceL")
                {
                    NoteMarketEnterenceL.GetComponent<Text>().enabled = true;
                }
                if (hit.collider.name == "NoteMarketEnterenceR")
                {
                    NoteMarketEnterenceR.GetComponent<Text>().enabled = true;
                }
                if (hit.collider.name == "NoteMusicShop")
                {
                    NoteMusicStore.GetComponent<Text>().enabled = true;
                }
            }
            if(hit.collider.tag != "Note")
            {
                NoteStart.GetComponent<Text>().enabled = false;
                NoteSupermarket.GetComponent<Text>().enabled = false;
                NoteMarketEnterenceL.GetComponent<Text>().enabled = false;
                NoteMarketEnterenceR.GetComponent<Text>().enabled = false;
                NoteMusicStore.GetComponent<Text>().enabled = false;
            }

        }
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);

            if (Physics.Raycast(ray, out hit, maxCastDistance))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    if (interactable.tag == "Item")
                    {
                        hit.collider.SendMessageUpwards("PickUp", SendMessageOptions.DontRequireReceiver);
                    }
                    if (hit.collider.tag == "Interactable")
                    {
                        hit.collider.SendMessageUpwards("PickUp", SendMessageOptions.DontRequireReceiver);
                    }
                    if (hit.collider.tag == "Door")
                    {
                        hit.collider.SendMessageUpwards("OpenDoor", SendMessageOptions.DontRequireReceiver);
                    }
                    if (hit.collider.tag == "Bill")
                    {
                        hit.collider.SendMessageUpwards("Bill", SendMessageOptions.DontRequireReceiver);
                    }
                    if (hit.collider.tag == "Kingsley")
                    {
                        hit.collider.SendMessageUpwards("Kingsley", SendMessageOptions.DontRequireReceiver);
                    }
                    if (hit.collider.tag == "Gary")
                    {
                        hit.collider.SendMessageUpwards("Gary", SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
        //if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);

        //    if (Physics.Raycast(ray, out hit, maxCastDistance))
        //    {
        //        Interactable interactable = hit.collider.GetComponent<Interactable>();
        //        if (interactable != null)
        //        {
        //            if (interactable.tag == "Item")
        //            {
        //                interactText.enabled = true;
        //                for (int i = 0; i < inventory.slots.Length; i++)
        //                {
        //                    if (inventory.isFull[i] == false)
        //                    {
        //                        ui_Inventory.SetActive(true);
        //                        inventory.isFull[i] = true;
        //                        Destroy(hit.collider.gameObject);
        //                        Instantiate(gun, inventory.slots[i].transform, false);
        //                        ui_Inventory.SetActive(false);
        //                        break; // once item is found break loop
        //                    }
        //                }
        //            }
        //            Debug.Log("item touched");
        //        }
        //    }
        //}
    }
}
