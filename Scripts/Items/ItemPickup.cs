using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Adjustable Variables \\
    private bool didPickup = false;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag != "Player") { return; }

        if (didPickup == false)
        {
            didPickup = true;

        }
    }
}
