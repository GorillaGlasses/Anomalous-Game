using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StatBlock : MonoBehaviour
{
    public string charName;
    public int health;
    public int maxHealth;
    public int strength;
    public int dexterity;
    public int constitution;
    public int defense;
    public bool isPlayer;
    public TextMeshProUGUI combatLog;
    public TextMeshProUGUI healthMeter;
    public LevelGeneration levelStats;
    public MenuManager levelMenu;
    public MenuManager HUD;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        combatLog = GameObject.FindGameObjectWithTag("LogText").GetComponent<TextMeshProUGUI>();
        levelStats = GameObject.FindGameObjectWithTag("LevelGen").GetComponent<LevelGeneration>();
        levelMenu = GameObject.FindGameObjectWithTag("LevelComplete").GetComponent<MenuManager>();
        HUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<MenuManager>();
        if (isPlayer)
        {
            healthMeter = GameObject.FindGameObjectWithTag("HealthMeter").GetComponent<TextMeshProUGUI>();
            healthMeter.text = "HP: " + health + "/" + maxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            if (isPlayer)
            {
                this.gameObject.GetComponent<PlayerController>().finishedLevel = true;
                levelMenu.setMenuActive();
                levelMenu.changeText(levelMenu.menuObjects[1].GetComponent<TextMeshProUGUI>(), "Game Over!");
                HUD.setMenuInactive();
            }
            else
            {
                combatLog.text += charName + " slain!\n";
                levelStats.placedEnemies--;
                Destroy(this.gameObject);
            }
        }

        if (isPlayer)
        {
            healthMeter.text = "HP: " + health + "/" + maxHealth;
        }
    }
}