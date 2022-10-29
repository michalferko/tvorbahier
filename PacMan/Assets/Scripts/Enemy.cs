using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent agent;

    [SerializeField] private float speed = 2;
    [SerializeField] private float killRadius = 1;

    // Start is called before the first frame update
    void Start()
    {
        // Get reference to player
        player = GameObject.FindGameObjectWithTag("Player");
        // Get reference to NavMeshAgent
        agent = GetComponent<NavMeshAgent>();

        // Set agents speed
        if (agent)
        {
            agent.speed = speed;
        }
    }

    private void Update()
    {
        // Check if player exists
        if (!player) return;

        // Check distance between enemy and player, if less then kill radius, kill both player and enemy
        if (Vector3.Distance(transform.position, player.transform.position) <= killRadius)
        {
            KillPlayer();
            KillEnemy();
        }
    }

    void FixedUpdate()
    {
        // Check if player exists
        if (!player) return;

        // Check if NavMeshAgent is attached to enemy
        if (!agent) return;

        // Set agents destination to player's destination
        agent.destination = player.transform.position;
    }

    // Kill player logic
    void KillPlayer()
    {
        // Check if player exists with given component
        if (player && player.TryGetComponent(out PacManController pmc))
        {
            // Call kill player from PacManController
            pmc.KillPlayer();
        }
    }

    // Kill Enemy logic
    public void KillEnemy()
    {
        // Destroy enemy
        Destroy(gameObject);
    }
}
