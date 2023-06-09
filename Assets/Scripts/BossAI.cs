using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BossAI : MonoBehaviour
{
    public RaycastHit hitObject;
    public float distence;
    public float angle = 120f;
    public float radius = 10f;
    float timer = 5f;
    float searchTimer = 15f;
    public int damage = 30;

    public NavMeshAgent navAgent; // ref to nav mesh agent
    public Transform player; // ref player transform to locate player
    public LayerMask ground, playerLayer;
    public GameObject raycastStartPoint;
    public Vector3 walkPoint;
    bool walkPointSet; // check is there is a walk point currently
    public float walkPointRange = 50; // control walk point range
    public float searchRange = 3; // search for player within this range


    public float attackCooldown; // time till next attack
    bool hasAttacked; // check if already attacked

    public float sightRange = 20, attackRange = .5f; // cache values for optimisation
    public bool playerInSight, playerInAttackRange, searching, underAttack, hasDied;
    int maxHealth = 70;
    public int currentHP;
    float underAttackTimer = 5f;
    float deathTimer = 5f;
    Vector3 distToPlayer;
    public GameObject exitDoor;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // find player transform by tag and cache the result for optimisation
        navAgent = GetComponent<NavMeshAgent>(); // assign nav agent
        currentHP = maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        GetState();
        if (hasDied)
        {
            Die();
        }
    }

    private void GetState()
    {
        // check for sight and attack range
        //playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        GetComponent<EnemyAnimations>().AnimateEnemy();
        distToPlayer = transform.position - player.position;
        if(distToPlayer.magnitude < 4)
        {
            playerInAttackRange = true;
        }
        else
        {
            playerInAttackRange = false;
        }
        if (playerInSight && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if (playerInAttackRange && playerInSight)
        {
            AttackPlayer();
        }

        Vector3 directionVector = (transform.position - player.transform.position).normalized;
        Vector3 direction = transform.TransformDirection(Vector3.forward); // sets the direction of the variable to vector3 forward to travel along the z axis
        // if raycast is shot on the transform position  in the direction of travel (sets hit object as peraminter) distance of 20 units
        // draws line from current position (forwards) to the value of 20 units if something is hit in that period then period will be output as a hit object else object will be NULL
        float dotProduct = Vector3.Dot(directionVector, transform.forward);
        float damageCooldown = 1.5f;
        distence = Vector3.Distance(transform.position, player.transform.position);
        if (distence <= 30 && dotProduct < -0.5)
        {
            Physics.Raycast(raycastStartPoint.transform.position, player.transform.position - raycastStartPoint.transform.position, out hitObject, 20);
            Debug.DrawRay(transform.position, hitObject.point, Color.magenta); // draws line to hit object
            if (hitObject.collider.tag == "Player")
            {
                Debug.DrawLine(transform.position, hitObject.point, Color.magenta); // draws line to hit object
                if (distence <= 4)
                {
                    hitObject.collider.gameObject.GetComponent<PlayerController>().ApplyDamageToPlayer(damage, damageCooldown);
                }
                playerInSight = true;
            }
        }
    }

    void Die()
    {
        timer -= 1 * Time.deltaTime;
        if (timer <= 0f)
        {
            Destroy(gameObject);
            exitDoor.SetActive(false);
        }
    }

    private void ChasePlayer()
    {
        GetComponent<EnemyAnimations>().patrol = false;
        GetComponent<EnemyAnimations>().playerSpotted = true;
        GetComponent<EnemyAnimations>().attackSlam = false;
        navAgent.SetDestination(player.position); // go to player position
    }
    private void AttackPlayer()
    {
        Debug.Log("working");
        GetComponent<EnemyAnimations>().patrol = false;
        GetComponent<EnemyAnimations>().playerSpotted = false;
        navAgent.SetDestination(transform.position);
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z); // lock the rotation to stop enemy facing down
        transform.LookAt(targetPosition);

        if (!hasAttacked)
        {
            // attack code
            //Debug.Log("attack player");
            GetComponent<EnemyAnimations>().attackSlam = true;

            hasAttacked = true;
            Invoke(nameof(ResetAttack), attackCooldown); // attack cooldown
        }
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }

    public void ApplyDamageToEnemy(int damage) // damage is taken in a float and will apply the damage if object has more than 0 HP
    {
        if (!underAttack)
        {
            underAttack = true;
        }
        if (currentHP < 0f)
        {
            return;
        }
        currentHP -= damage;
        if (currentHP <= 0)
        {
            GetComponent<BoxCollider>().enabled = false;
            navAgent.isStopped = true;
            GetComponent<EnemyAnimations>().patrol = false;
            GetComponent<EnemyAnimations>().playerSpotted = false;
            GetComponent<EnemyAnimations>().attackSlam = false;
            GetComponent<EnemyAnimations>().dead = true;
            hasDied = true;
        }
    }
}
