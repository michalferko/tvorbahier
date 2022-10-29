using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text time = default;
    [SerializeField] private Text killCount = default;
    [SerializeField] private Text collectiblesCount = default;
    [SerializeField] private Text winMessage = default;

    private void OnEnable()
    {
        // Subscribe to all delegate's events from PacManController
        PacManController.onUpdateTime += UpdateTime;
        PacManController.onUpdateKills += UpdateKills;
        PacManController.onUpdateCollectibles += UpdateCollectibles;

        PacManController.onWin += ShowWinMessage;
    }

    private void OnDisable()
    {
        // Unsubscribe from all delegate's events from PacManController
        PacManController.onUpdateTime -= UpdateTime;
        PacManController.onUpdateKills -= UpdateKills;
        PacManController.onUpdateCollectibles -= UpdateCollectibles;

        PacManController.onWin -= ShowWinMessage;
    }

    // Update UI time logic
    void UpdateTime(float value)
    {
        // transform time to desired format
        int minutes = Mathf.FloorToInt(value / 60F);
        int seconds = Mathf.FloorToInt(value - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Set UI time text
        time.text = niceTime;
    }

    // Update UI kills logic
    void UpdateKills(float value)
    {
        // Set UI kill count text
        killCount.text = value.ToString();
    }

    // Update UI collectibles logic
    void UpdateCollectibles(float value)
    {
        // Set UI collectibles count text
        collectiblesCount.text = value.ToString();
    }

    // Show UI win message
    void ShowWinMessage()
    {
        // Set win message gameobject to active (disabled by default)
        winMessage.gameObject.SetActive(true);
    }
}
