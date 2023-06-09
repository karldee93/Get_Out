using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float mouseSpeed = 100f;

    public Transform playerBody;
    public FloatingJoystick joystick;
    public Camera camera;
    public ParticleSystem muzzleFlashHG;
    public ParticleSystem muzzleFlashAR;
    public GameObject impactEffect;
    public GameObject gameManager;
    public GameObject shootButton, reloadButton;
    float xRotation = 0f;
    public float lookX;
    public float lookY;
    public bool handGunActive = false;
    public bool ARActive = false;
    private bool shoot = false;
    public bool isHgAmmo;
    public bool isArAmmo;
    public bool ammoAdded;
    private bool cameraLook = false;
    private float nextTimeToFire = 0f;
    public float fireDelay = 1f;
    public int hgAmmo = 20;
    public int hgMaxAmmo = 20;
    public int hgAmmoCap = 70;

    public int arAmmo = 32;
    public int arMaxAmmo = 32;
    public int arAmmoCap = 170;
    private Touch theTouch;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked; // locks the cursor to the centre of the screen
    }

    // Update is called once per frame
    void Update()
    {
        TouchCheck(); // check for touch input
        if (handGunActive)
        {
            shootButton.SetActive(true);
            reloadButton.SetActive(true);
            gameManager.GetComponent<Ammo_UI>().currentInClipHG = hgAmmo;
            gameManager.GetComponent<Ammo_UI>().ammoBagHG = hgAmmoCap;
            gameManager.GetComponent<Ammo_UI>().UpdateHGAmmoUI();
            FireHandgun(); // check shoot bool
        }
        if (ARActive)
        {
            shootButton.SetActive(true);
            reloadButton.SetActive(true);
            gameManager.GetComponent<Ammo_UI>().currentInClipAR = arAmmo;
            gameManager.GetComponent<Ammo_UI>().ammoBagAR = arAmmoCap;
            gameManager.GetComponent<Ammo_UI>().UpdateARAmmoUI();
            FireAR();
        }
        if (!handGunActive && !ARActive)
        {
            shootButton.SetActive(false);
            reloadButton.SetActive(false);
            gameManager.GetComponent<Ammo_UI>().UpdateNoActiveGunUI();
        }
    }

    public void AddAmmo()
    {
        if (isHgAmmo)
        {
            if (hgAmmoCap <= 70)
            {
                hgAmmoCap += 10;
                isHgAmmo = false;
                ammoAdded = true;
            }
        }
        if (isArAmmo)
        {
            if (arAmmoCap <= 170)
            {
                arAmmoCap += 20;
                isArAmmo = false;
                ammoAdded = true;
            }
        }
    }
    void TouchCheck()
    {
        if (cameraLook) // if true call CameraLook()
        {
            CameraLook();
        }
        // provides the number of current touches... if > 0 run
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0); // get the first touch
            switch (theTouch.phase) // check touch phases
            {
                // When a touch has first been detected set camera look to true
                case TouchPhase.Began:
                    cameraLook = true;
                    break;

                case TouchPhase.Ended:
                    cameraLook = false; // stop cameralook script
                    break;
            }
        }
    }
    void CameraLook()
    {
        lookX = joystick.Horizontal * mouseSpeed * Time.deltaTime; // move horizontal
        lookY = joystick.Vertical * mouseSpeed * Time.deltaTime; // move vertical

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // limit the rotation of the camrea stop it looking behind

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * lookX);
    }
    public void Reload()
    {
        if (ARActive)
        {
            if (arAmmoCap >= 30 && arAmmo < 30)
            {
                int ammoDiff;
                ammoDiff = arAmmo - arMaxAmmo;
                arAmmo = arMaxAmmo;
                gameManager.GetComponent<Ammo_UI>().currentInClipAR = arMaxAmmo;
                arAmmoCap += ammoDiff;
                gameManager.GetComponent<Ammo_UI>().ammoBagAR += ammoDiff;
            }
            else if (arAmmoCap < 30 && arAmmo < 30)
            {
                int remainingAmmo = arAmmoCap;
                arAmmo += remainingAmmo;
                gameManager.GetComponent<Ammo_UI>().currentInClipAR += arAmmoCap;
                arAmmoCap = 0;
                gameManager.GetComponent<Ammo_UI>().ammoBagAR = arAmmoCap;
            }
            else
            {
                //nothing
            }
        }
        if (handGunActive)
        {
            if (hgAmmoCap >= 30 && hgAmmo < 30)
            {
                int ammoDiff;
                ammoDiff = hgAmmo - hgMaxAmmo;
                hgAmmo = hgMaxAmmo;
                gameManager.GetComponent<Ammo_UI>().currentInClipHG = hgMaxAmmo;
                hgAmmoCap += ammoDiff;
                gameManager.GetComponent<Ammo_UI>().ammoBagHG += ammoDiff;
            }
            else if (hgAmmoCap < 30 && hgAmmo < 30)
            {
                int remainingAmmo = hgAmmoCap;
                hgAmmo += remainingAmmo;
                gameManager.GetComponent<Ammo_UI>().currentInClipHG += hgAmmoCap;
                arAmmoCap = 0;
                gameManager.GetComponent<Ammo_UI>().ammoBagHG = hgAmmoCap;
            }
            else
            {
                //nothing
            }
        }
    }

    void FireAR()
    {
        if (shoot && Time.time >= nextTimeToFire && arAmmo > 0) // if shoot is true fire weapon 
        {
            nextTimeToFire = Time.time + .2f / fireDelay; // if fire rate is 4 this will add 1 / 4 = .25 onto the current time meaning shooting will occure every .25 seconds
            Debug.Log("fire AR");
            arAmmo -= 1;
            muzzleFlashAR.Play();
            Vector3 direction = transform.TransformDirection(Vector3.forward); // sets the direction of the variable to vector3 forward to travel along the z axis
            RaycastHit hit;

            float range = 100f;
            int damage = 1; // set damage to 10
                            // if raycast is shot on the transform position  in the direction of travel (sets hit object as peramanter) distance of 20 units
                            // draws line from current position (forwards) to the value of 20 units if something is hit in that period then period will be output as a hit object else object will be NULL
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range))
            {
                if (hit.collider.tag == "Enemy")
                {
                    hit.collider.SendMessageUpwards("ApplyDamageToEnemy", damage, SendMessageOptions.DontRequireReceiver); // this will call apply damage sub routine in the hit object 

                    GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impactGO, 2f);
                }
            }
        }
    }
    void FireHandgun()
    {
        if (shoot && Time.time >= nextTimeToFire && hgAmmo > 0) // if shoot is true fire weapon 
        {
            nextTimeToFire = Time.time + 1.3f / fireDelay; // if fire rate is 4 this will add 1 / 4 = .25 onto the current time meaning shooting will occure every .25 seconds
            Debug.Log("fire hg");
            hgAmmo -= 1;
            muzzleFlashHG.Play();
            Vector3 direction = transform.TransformDirection(Vector3.forward); // sets the direction of the variable to vector3 forward to travel along the z axis
            RaycastHit hit;

            float range = 70f;
            int damage = 5; // set damage to 10
                            // if raycast is shot on the transform position  in the direction of travel (sets hit object as peramanter) distance of 20 units
                            // draws line from current position (forwards) to the value of 20 units if something is hit in that period then period will be output as a hit object else object will be NULL
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range))
            {
                if (hit.collider.tag == "Enemy")
                {
                    hit.collider.SendMessageUpwards("ApplyDamageToEnemy", damage, SendMessageOptions.DontRequireReceiver); // this will call apply damage sub routine in the hit object 

                    GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impactGO, 2f);
                }
            }
        }
    }


    public void Shoot() // called via button press
    {
        if (!ARActive && !handGunActive)
        {
            // ignore button press
        }
        else
        {
            if (!shoot)
            {
                shoot = true;
            }
            else
            {
                shoot = false;
            }
        }

    }
}
