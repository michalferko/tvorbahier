using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float activeTime = 2;
    [SerializeField] private float deactivatedTime = 3;
    [SerializeField] private Transform Spikes = default;
    [SerializeField] private float hiddenSubY = 1;

    private bool isActivated;
    private float timer;

    private float defaultY;
    private float hiddenY;
    private float targetY;
    // Start is called before the first frame update
    void Start()
    {
        // Set timer as deactived at start
        timer = deactivatedTime;

        // Get default and hidden Y position
        defaultY = Spikes.position.y;
        hiddenY = Spikes.position.y - hiddenSubY;
        // Set target Y as hidden Y at start
        targetY = hiddenY;
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease timer
        timer -= Time.deltaTime;

        // If timer is less than 0, call Deactivate if trap is activated and vice versa
        if (timer <= 0)
        {
            if (isActivated)
            {
                Deactivate();
            }
            else
            {
                Activate();
            }
        }

        // Up&Down animation
        if (Spikes.position.y != targetY)
        {
            // smoothly keep updating currentY until it reaches targetY
            float currentY = Mathf.MoveTowards(Spikes.position.y, targetY, 10 * Time.deltaTime);
            // Set traps Y position to the currentY
            Spikes.position = new Vector3(Spikes.position.x, currentY, Spikes.position.z);
        }
    }
    // Activate trap logic
    void Activate()
    {
        // Set targetY position for animation
        targetY = defaultY;
        // Set is activated to true, player dies if stepping on trap
        isActivated = true;
        // Set timer to active
        timer = activeTime;
    }
    // Deactivate trap logic
    void Deactivate()
    {
        // Set targetY position for animation
        targetY = hiddenY;
        // Set is activated to false, player won't die if stepping on trap
        isActivated = false;
        // Set timer to deactivated
        timer = deactivatedTime;
    }
    // Check if someone stepped on trap
    private void OnTriggerStay(Collider other)
    {
        // If trap is activated and player stepped on it, kill the player
        if (isActivated && other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out PacManController pmc))
            {
                pmc.KillPlayer();
            }
        }
    }
}
