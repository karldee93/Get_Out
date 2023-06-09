using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f; // how close player needs to be to interact

    void OnDrawGizmosSelected()
    {
        // visualise interaction zone for debugging ######COMMENT THIS CODE OUT ON BUILD######
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
