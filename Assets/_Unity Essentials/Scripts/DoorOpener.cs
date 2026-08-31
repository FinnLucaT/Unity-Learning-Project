using UnityEngine;


public class DoorOpener : MonoBehaviour
{


    [SerializeField] private Animator doorAnimator;


     private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player (or another specified object)
        if (other.CompareTag("Player")) // Make sure the player GameObject has the tag "Player"
        {
            if (doorAnimator != null)
            {
                // Trigger the Door_Open animation
                doorAnimator.SetTrigger("Door_Open");
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        // Check if the object leaving the trigger is the player (or another specified object)
        if (other.CompareTag("Player")) // Make sure the player GameObject has the tag "Player"
        {
            if (doorAnimator != null)
            {
                // Trigger the Door_Close animation
                doorAnimator.SetTrigger("Door_Close");
            }
        }
    }
}