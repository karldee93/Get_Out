using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    public Animator animator;
    public bool patrol, playerSpotted, attackSlam, dead;
    public void AnimateEnemy()
    {
        if (patrol)
        {
            animator.SetBool("Patrol", true);
        }
        else
        {
            animator.SetBool("Patrol", false);
        }
        if (playerSpotted)
        {
            animator.SetBool("PlayerSpotted", true);
        }
        else
        {
            animator.SetBool("PlayerSpotted", false);
        }
        if (attackSlam)
        {
            animator.SetBool("SlamAttack", true);
        }
        else
        {
            animator.SetBool("SlamAttack", false);
        }
        if (dead)
        {
            animator.SetBool("Dead", true);
        }
    }
}
