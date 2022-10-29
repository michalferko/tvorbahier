using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private GameObject player;

    [SerializeField] private float pickupRange = 1;
    [SerializeField] private float score = 1;

    void Start()
    {
        // Get reference to player
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Don't check for distance if player doesn't exist
        if (!player) return;

        // Check distance between collectible and player, if less then pickup range, pick up collectible
        if (Vector3.Distance(transform.position, player.transform.position) <= pickupRange)
            PickUp();
    }

    void PickUp()
    {
        // Check if player exists with given component
        if (player && player.TryGetComponent(out PacManController pmc))
        {
            // Call pickup collectible logic from PacManController with score of collectible
            pmc.PickUpCollectible(score);
        }

        // Destroy collectible
        Destroy(gameObject);
    }
}
