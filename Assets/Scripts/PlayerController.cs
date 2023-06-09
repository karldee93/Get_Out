using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    public FixedJoystick joystick;

    public HealthBarController healthBar;
    int maxHealth = 100;
    public int currentHP;
    public float currentCooldown = 20;

    public float speed = 15f;
    public float gravity = 9.81f;
    public float jumpHeight = 3f;
    private Vector3 velocity;
    private Vector3 move;

    public Transform groundCheck; // checks if player is colliding with the ground
    public LayerMask groundLayer;
    public float groundDistence = 0.4f; // radius of the sphere being used to check for ground
    private bool isGrounded;
    public bool healthAdded;
    public GameObject enemy, securityKey;
    public Transform spawnSecurityKey;
    public int numOfSecurityKeys = 1;
    public int numShopsRemaining = 4;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentHP = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        SpawnSecurityKey();
    }

    void SpawnSecurityKey()
    {
        if (numShopsRemaining <= 0)
        {
            if (numOfSecurityKeys > 0)
            {
                Instantiate(securityKey, spawnSecurityKey.position, Quaternion.identity);
                numOfSecurityKeys -= 1;
            }
        }
    }
    void Movement()
    {
        float x = joystick.Horizontal;
        float z = joystick.Vertical;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistence, groundLayer);

        move = transform.right * x + transform.forward * z;

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(move * speed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2 * gravity);
        }
    }
    public void ApplyDamageToPlayer(int damage, float damageCooldown) // damage is taken in a float and will apply the damage if object has more than 0 HP
    {
        if (currentHP < 0f)
        {
            return;
        }
        currentCooldown -= damageCooldown;
        if (currentCooldown <= 0)
        {
            currentHP -= damage;
            healthBar.GetComponent<HealthBarController>().healthSlider.value = currentHP;
            currentCooldown = 200;
        }


        if (currentHP <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void AddHealth()
    {
        if (currentHP > 50 && currentHP < 100)
        {
            healthAdded = true;
            currentHP = 100; // ensure player does not get over 100 hp
            healthBar.GetComponent<HealthBarController>().healthSlider.value = currentHP;
        }
        if (currentHP <= 50)
        {
            healthAdded = true;
            currentHP += 50;
            healthBar.GetComponent<HealthBarController>().healthSlider.value = currentHP;
        }
    }
}
