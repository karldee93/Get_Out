using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    public RaycastHit hitObject;
    public float distence;
    public float angle = 120f;
    public float radius = 10f;
    float timer = 5f;
    float searchTimer = 15f;
    public int damage = 10;

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
    public bool playerInSight, playerInAttackRange, searching, underAttack, hasDied, inArcade, inBakery, inJewelry, inClothing;
    public Transform arcade, bakery, jewelry, clothing;
    int maxHealth = 25;
    public int currentHP;
    float underAttackTimer = 5f;
    float deathTimer = 5f;

    GameObject currentWaypoint; // great a game object to store current and previous waypoints
    GameObject previousWaypoint, playerObj;
    GameObject[] allWaypoints;
    public GameObject spawn, headstone;
    Vector3 distToArcade, distToBakery, distToJewelry, distToClothing;
    
    private void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = GameObject.FindGameObjectWithTag("Player").transform; // find player transform by tag and cache the result for optimisation
        navAgent = GetComponent<NavMeshAgent>(); // assign nav agent
        spawn = GameObject.FindGameObjectWithTag("Spawner");
        allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint"); // this will set the value of allWaypoints to any gameobjects from with the tag "waypoint" 
        currentWaypoint = GetRandomWaypoint(); // this will set current way point to a random waypoint using the created function
        currentHP = maxHealth;
        arcade = GameObject.FindWithTag("Arcade").transform;
        bakery = GameObject.FindWithTag("Bakery").transform;
        jewelry = GameObject.FindWithTag("Jewelry").transform;
        clothing = GameObject.FindWithTag("Clothing").transform;
    }
    // Update is called once per frame
    void Update()
    {
        GetState();
        if (hasDied)
        {
            Die();
        }
        GetRoom();
        if(player.GetComponent<PlayerController>().numShopsRemaining <= 0)
        {
            spawn.GetComponent<Spawner>().enabled = false;
        }
    }

    void GetRoom()
    {
        distToArcade = transform.position - arcade.position;
        distToBakery = transform.position - bakery.position;
        distToJewelry = transform.position - jewelry.position;
        distToClothing = transform.position - clothing.position;
        if (distToArcade.magnitude < 25)
        {
            inArcade = true;
        }
        else
        {
            inArcade = false;
        }

        if (distToBakery.magnitude < 15)
        {
            inBakery = true;
        }
        else
        {
            inBakery = false;
        }

        if (distToJewelry.magnitude < 15)
        {
            inJewelry = true;
        }
        else
        {
            inJewelry = false;
        }

        if (distToClothing.magnitude < 25)
        {
            inClothing = true;
        }
        else
        {
            inClothing = false;
        }
    }

    GameObject GetRandomWaypoint() // this sets the function to a game object cant use void as it is not as it is typed out above must have a return script
    {
        if (allWaypoints.Length == 0)
        {
            return null; // if there are no waypoints in the array return null
        }
        else
        {
            int index = Random.Range(0, allWaypoints.Length); // create a random number between 0 and the length of the array
            return allWaypoints[index]; // this returns the index number to set which waypoint should go to
        }
    }

    private void GetState()
    {
        // check for sight and attack range
        //playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        GetComponent<EnemyAnimations>().AnimateEnemy();
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSight && !playerInAttackRange && !searching)
        {
            Patroling();
        }
        if (!playerInSight && !playerInAttackRange && searching)
        {
            SearchForPlayer();
        }
        if (playerInSight && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if (playerInAttackRange && playerInSight)
        {
            AttackPlayer();
        }
        if (underAttack || underAttack && searching)
        {
            searching = false;
            playerInSight = true;
            underAttackTimer -= 1f * Time.deltaTime;
            if (underAttackTimer <= 0)
            {
                underAttack = false;
                underAttackTimer = 5f;
                playerInSight = false;
                searching = true;
            }
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
                if (distence <= 3)
                {
                    hitObject.collider.gameObject.GetComponent<PlayerController>().ApplyDamageToPlayer(damage, damageCooldown);
                }
                playerInSight = true;
                if (searching)
                {
                    searchTimer = 15f;
                    searching = false;
                }
            }
        }
    }

    void Die()
    {
        timer -= 1 * Time.deltaTime;
        if (timer <= 0f)
        {
            spawn.GetComponent<Spawner>().currentEnemies -= 1;
            spawn.GetComponent<Spawner>().spawn = true;
            if (inArcade)
            {
                Instantiate(headstone, this.transform.position, Quaternion.identity);
                player.GetComponent<PlayerController>().numShopsRemaining -= 1;
            }
            if (inBakery)
            {
                Instantiate(headstone, this.transform.position, Quaternion.identity);
                player.GetComponent<PlayerController>().numShopsRemaining -= 1;
            }
            if (inJewelry)
            {
                Instantiate(headstone, this.transform.position, Quaternion.identity);
                player.GetComponent<PlayerController>().numShopsRemaining -= 1;
            }
            if (inClothing)
            {
                Instantiate(headstone, this.transform.position, Quaternion.identity);
                player.GetComponent<PlayerController>().numShopsRemaining -= 1;
            }
            Destroy(gameObject);
        }
    }

    private void Patroling()
    {
        GetComponent<EnemyAnimations>().patrol = true;
        GetComponent<EnemyAnimations>().playerSpotted = false;
        GetComponent<EnemyAnimations>().attackSlam = false;
        GetWalkPoint();

        if(navAgent.remainingDistance <= 1f)
        {
            walkPointSet = false;
        }
        //Vector3 distanceToWalkPoint = transform.position - walkPoint;
        //// walkpoint reached
        //if (distanceToWalkPoint.magnitude < 1f)
        //{
        //    walkPointSet = false;
        //}
    }

    private void GetWalkPoint()
    {
        Vector3 targetVector = currentWaypoint.transform.position; // sets the target vector to be the position of the current way point current waypoint is not where it is and the terget vector is the vector3 for it rather than the game objects
        if (!walkPointSet)
        {
            previousWaypoint = currentWaypoint; // sets the value of the previous waypoint to be the current one 
            currentWaypoint = GetRandomWaypoint(); // sets the new current waypoint value using the getrandomwaypoint function
            walkPointSet = true;
        }
        if (walkPointSet)
        {
            navAgent.SetDestination(targetVector);
        }


        //// get random point
        //float randomZ = Random.Range(-walkPointRange, walkPointRange); // find number between - range and + range
        //float randomX = Random.Range(-walkPointRange, walkPointRange);

        //walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        //if (Physics.Raycast(walkPoint, -transform.up, 2f))
        //{
        //    walkPointSet = true;
        //}
    }

    private void ChasePlayer()
    {
        GetComponent<EnemyAnimations>().patrol = false;
        GetComponent<EnemyAnimations>().playerSpotted = true;
        GetComponent<EnemyAnimations>().attackSlam = false;
        navAgent.speed = 3.5f; // increase speed to match run
        navAgent.SetDestination(player.position); // go to player position
        if (distence > sightRange)
        {
            //Debug.Log("Timer had started" + timer);
            timer -= 1f * Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 5f;
                searching = true;
                playerInSight = false;
                walkPointSet = false;

            }
        }
    }

    private void SearchForPlayer()
    {
        GetComponent<EnemyAnimations>().patrol = true;
        GetComponent<EnemyAnimations>().playerSpotted = false;
        GetComponent<EnemyAnimations>().attackSlam = false;
        float randomZ = Random.Range(-searchRange, searchRange); // find number between - range and + range
        float randomX = Random.Range(-searchRange, searchRange);
        if (!walkPointSet)
        {
            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            if (Physics.Raycast(walkPoint, -transform.up, 2f))
            {
                walkPointSet = true;
            }
        }
        if (walkPointSet)
        {
            navAgent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
        searchTimer -= 1f * Time.deltaTime;
        //Debug.Log("Timer: " + searchTimer);
        if (searchTimer <= 0f)
        {
            searchTimer = 0f;
            searching = false;
            searchTimer = 15f;
        }
    }

    private void AttackPlayer()
    {
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
