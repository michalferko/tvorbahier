using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    [SerializeField] private KeyCode key = KeyCode.Keypad0;
    [SerializeField] private float baseTime = 5;
    [SerializeField] private float dBaseTime = 3.5f;
    [SerializeField] private float hideTime = 1;
    [SerializeField] private int scoreAmount = 1;

    [SerializeField] private float speed = 5;
    [SerializeField] private float hiddenSubY = 1;
    private float defaultY;
    private float hiddenY;
    private float targetY;

    [SerializeField] private ParticleSystem PS = null;

    private bool isVisible;
    private float showTimer;
    private float hideTimer;
    // Start is called before the first frame update
    void Start()
    {
        // Hide mole at start
        showTimer = getRandomShowTime();
        isVisible = false;

        // Get default and hidden Y position
        defaultY = transform.position.y;
        hiddenY = transform.position.y - hiddenSubY;
        // Set target Y as hidden Y at start
        targetY = hiddenY;
    }

    // Update is called once per frame
    void Update()
    {
        // Check mole visibility
        if (!isVisible)
        {
            // If mole isn't visible, decrease showTimer until it reaches 0 and show mole
            showTimer -= Time.deltaTime;
            if (showTimer <= 0)
            {
                Show();
            }
        }
        else
        {
            // If mole is visible, decrease hideTimer until it reaches 0 and hide mole
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0)
            {
                Hide();
                Hit(false);
            }
        }

        // Check for players Smash input
        if (Input.GetKeyDown(key))
        {
            if (isVisible)
            {
                // If player smashed visible mole, hide it and call Hit with success
                Hide();
                Hit(true);
            }
            else
            {
                // If player didn't hit visible mole, call Hit with failure
                Hit(false);
            }
        }

        // Up&Down animation
        if (transform.position.y != targetY)
        {
            // smoothly keep updating currentY until it reaches targetY
            float currentY = Mathf.MoveTowards(transform.position.y, targetY, speed * Time.deltaTime);
            // Set moles Y position to the currentY
            transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
        }
    }

    void Show()
    {
        // Reset hideTimer, mole hides after it reaches 0
        hideTimer = hideTime;
        // Set visibility to true, player can hit mole during this time
        isVisible = true;
        // Set Target Y position to default Y, mole will dig up to the ground
        targetY = defaultY;
    }

    void Hide()
    {
        // Set random show timer, mole shows after it reaches 0
        showTimer = getRandomShowTime();
        // Set visibility to false, if player hits mole he will receive unsuccessful hit
        isVisible = false;
        // Set Target Y position to hidden Y, mole will go under the ground
        targetY = hiddenY;
    }

    float getRandomShowTime()
    {
        // Get random time deviation
        float randomDelta = Random.Range(-dBaseTime, dBaseTime);
        // Return base time with applied deviation
        return baseTime + randomDelta;
    }
    void Hit(bool success)
    {
        // Check if player hit mole with success
        if (success)
        {
            // Log message to the editor console
            Debug.Log("Hit");
            // Add score via GameHandler instance
            GameHandler.instance.addScore(scoreAmount);
            // Player attached Particle System
            PS.Play();
        }
        else
        {
            // Log message to the editor console
            Debug.Log("Miss");
            // Substract score via GameHandler instance
            GameHandler.instance.addScore(-scoreAmount);
        }
    }
}
