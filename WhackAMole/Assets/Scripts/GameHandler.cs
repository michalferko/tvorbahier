using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;

    [SerializeField] private KeyCode restartButton = KeyCode.R;
    [SerializeField] private UnityEngine.UI.Text scoreCounter = null;

    private int currentScore = 0;

    private void Awake()
    {
        // Initialize instance
        if (instance != this && instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        // Check for restart button press
        if (Input.GetKeyDown(restartButton))
        {
            Restart();
        }
    }

    public void addScore(int amount)
    {
        // Add amount to the score
        currentScore += amount;
        // Update UI score counter
        scoreCounter.text = currentScore.ToString();
    }
    void Restart()
    {
        // Reload scene
        SceneManager.LoadScene(0);
    }
}
