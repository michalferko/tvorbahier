using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private GameObject player;

    [SerializeField] private float pickupRange = 1;
    [SerializeField] private float boostedSpeed = 5;
    [SerializeField] private float boostedDuration = 3;

    // Start is called before the first frame update
    void Start()
    {
        // Get reference to player
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Don't check for distance if player doesn't exist
        if (!player) return;

        // Check distance between power up collectible and player, if less then pickup range, pick up collectible
        if (Vector3.Distance(transform.position, player.transform.position) <= pickupRange)
            PickUp();
    }

    void PickUp()
    {
        // Check if player exists with given component
        if (player && player.TryGetComponent(out PacManController pmc))
        {
            // Call boost speed from PacManController with boosted speed and duration of power up collectible
            pmc.BoostSpeed(boostedSpeed, boostedDuration);
        }
        // Destroy power up collectible
        Destroy(gameObject);
    }
}
