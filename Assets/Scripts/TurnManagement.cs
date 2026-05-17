using TMPro;
using UnityEngine;

public class TurnManagement : MonoBehaviour
{
    public int turnCount = 0;
    public bool playerTurn = true;
    public int finishedEnemies = 0;
    public PlayerController playerStats;
    public LevelGeneration levelStats;
    public GameObject turnCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        finishedEnemies = 0;
        levelStats = GameObject.FindGameObjectWithTag("LevelGen").GetComponent<LevelGeneration>();
        UnityEngine.Debug.Log("Turn " + turnCount);
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerStats || !levelStats)
        {
            playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            levelStats = GameObject.FindGameObjectWithTag("LevelGen").GetComponent<LevelGeneration>();
        }

        if (playerTurn && playerStats.actionCount >= 100)
        {
            turnCount++;
            turnCounter.GetComponent<TextMeshProUGUI>().text = "Turn " + turnCount;
            playerStats.actionCount = 0;
            playerTurn = false;
        }

        if (!playerTurn && levelStats.placedEnemies <= finishedEnemies)
        {
            playerTurn = true;
            finishedEnemies = 0;
        }
    }
}
