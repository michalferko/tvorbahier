using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PacManController : MonoBehaviour
{
    private Rigidbody RB;
    private Vector3 direction;

    [SerializeField] private float defaultSpeed = 2;
    private float currentSpeed;

    private bool isBoosted;
    private float boostTimer;

    /* Create delegate for stats updating
     * with event for each stat (time, kills, collectibles),
     * now anyone interested can subscribe to these events
    */

    public delegate void OnUpdateStats(float value);
    public static event OnUpdateStats onUpdateTime;
    public static event OnUpdateStats onUpdateKills;
    public static event OnUpdateStats onUpdateCollectibles;

    // Another delegate when player wins
    public delegate void OnWin();
    public static event OnWin onWin;

    private float currentTime;
    private float currentKillCount;
    private float currentCollectiblesCount;
    private int allNormalCollectibles;
    private bool gameFinished;

    [Header("Gun and Bullet")]
    [SerializeField] private Transform gun = default;
    [SerializeField] private GameObject bullet = default;
    [SerializeField] private float gunFireRate = 0.5f;
    private float currentGunCooldown;

    [Header("Teleport Ability")]
    [SerializeField] private KeyCode teleportKey = KeyCode.F;
    [SerializeField] private float teleportDistance = 3;
    [SerializeField] private float teleportCooldown = 5;
    private float currentTeleportCooldown;
    private bool canUseTeleport;

    void Start()
    {
        // Get reference to rigidbody and default speed
        RB = GetComponent<Rigidbody>();
        currentSpeed = defaultSpeed;

        // Get count of all normal collectibles in game
        allNormalCollectibles = GameObject.FindGameObjectsWithTag("Collectible").Length;
    }

    void Update()
    {
        // Increase game timer if player didn't finish yet
        if (!gameFinished)
        {
            currentTime = Time.time;
            onUpdateTime?.Invoke(currentTime);
        }

        // Get player's movement direction based on input and normalize to prevent double speed when moving diagonal
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        // If player is boosted  decrease boost duration until it reaches 0 and reset player's speed back to defaul
        if (isBoosted)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0)
            {
                currentSpeed = defaultSpeed;
                isBoosted = false;
            }
        }

        // Get a Ray from Camera to player's mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // If hit is successful, rotate player towards mouse position (skip Y axis rotation)
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }

        // If gun is on cooldown, decrease it until it reaches 0, then Shoot and reset cooldown again
        currentGunCooldown -= Time.deltaTime;
        if (currentGunCooldown <= 0)
        {
            Shoot();
            currentGunCooldown = gunFireRate;
        }

        // If teleport is on cooldown, decrease it until it reaches 0, then allow player to teleport again
        if (!canUseTeleport)
        {
            currentTeleportCooldown -= Time.deltaTime;
            if (currentTeleportCooldown <= 0)
            {
                canUseTeleport = true;
            }
        }

        // If player wants to teleport and he can, teleport him
        if (Input.GetKeyDown(teleportKey) && canUseTeleport)
        {
            Teleport();
        }
    }

    private void FixedUpdate()
    {
        // Set velocity based on direction * currentSpeed (skip Y velocity to not override default rigidbody gravity)
        RB.velocity = new Vector3(direction.x * currentSpeed, RB.velocity.y, direction.z * currentSpeed);
    }

    // Shooting logic
    void Shoot()
    {
        // Instantiate a bullet when player shoots from his gun's position and his rotation
        Instantiate(bullet, gun.position, transform.rotation);
    }

    // Called when player picked up all normal collectibles
    void Win()
    {
        // Output message to the console
        Debug.Log("You've picked up all collectibles, good job!");
        // Invoke delegate event, all subcribers will get notified
        onWin?.Invoke();
        // Set gameFinished to true, prevent match timer from continuing
        gameFinished = true;
    }

    // Teleport ability logic
    void Teleport()
    {
        // Get a Ray from Camera to player's mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // If hit was successful, get direction between mouse position and player
            Vector3 direction = hit.point - transform.position;
            // Move rigidbody in given direction multiplied by teleport distance
            RB.MovePosition(transform.position + (direction.normalized * teleportDistance));

            // Set teleport on cooldown
            canUseTeleport = false;
            // Set teleport's timer
            currentTeleportCooldown = teleportCooldown;
        }
    }

    // Normal collectible logic
    public void PickUpCollectible(float score)
    {
        // Increase current collectibles count
        currentCollectiblesCount += score;
        // Invoke delegate event, all subcribers will get notified
        onUpdateCollectibles?.Invoke(currentCollectiblesCount);

        // Check if player already picked up all collectibles, if yes show Win message
        if (currentCollectiblesCount == allNormalCollectibles)
        {
            Win();
        }
    }

    // Boost collectible logic
    public void BoostSpeed(float value, float duration)
    {
        // Set player's speed based on boost collectible value
        currentSpeed = value;
        // Set player's boosted speed duration based on boost collectible duration
        boostTimer = duration;
        // Set isBoosted to true, so duration will start decreasing
        isBoosted = true;
    }

    // Kill player logic
    public void KillPlayer()
    {
        // Reload scene
        SceneManager.LoadScene(0);
    }

    // Kill enemy logic
    public void KillEnemy()
    {
        // Increase current kill count
        currentKillCount++;
        // Invoke delegate event, all subcribers will get notified
        onUpdateKills?.Invoke(currentKillCount);
    }
}
