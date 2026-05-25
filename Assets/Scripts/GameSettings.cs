using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class GameSettings : MonoBehaviour
{
    // Character Creation Variables
    public int statPoints = 3;
    public TextMeshProUGUI pointDisplay;
    public int strength;
    public TextMeshProUGUI strengthDisplay;
    public int dexterity;
    public TextMeshProUGUI dexterityDisplay;
    public int constitution;
    public TextMeshProUGUI constitutionDisplay;
    public PlayerStatsScriptableObject playerStats;

    // Game Settings Variables
    public bool darknessEnabled;
    public Toggle darkToggle;
    public GameSettingsScriptable gameSettings;

    void Start()
    {
        statPoints = playerStats.statPoints;
        strength = playerStats.strength;
        dexterity = playerStats.dexterity;
        constitution = playerStats.constitution;
        if (gameSettings.isDarknessEnabled)
        {
            darkToggle.isOn = true;
        }
        else
        {
            darkToggle.isOn = false;
        }
        darknessEnabled = gameSettings.isDarknessEnabled;
    }

    void Update()
    {
        pointDisplay.text = "Remaining Points: " + statPoints;
        strengthDisplay.text = strength.ToString();
        dexterityDisplay.text = dexterity.ToString();
        constitutionDisplay.text = constitution.ToString();
    }

    public void increaseStat(string stat)
    {
        if (stat == "strength" && statPoints > 0)
        {
            strength += 1;
            statPoints -= 1;
        }
        else if (stat == "dexterity" && statPoints > 0)
        {
            dexterity += 1;
            statPoints -= 1;
        }
        else if (stat == "constitution" && statPoints > 0)
        {
            constitution += 1;
            statPoints -= 1;
        }
    }

    public void decreaseStat(string stat)
    {
        if (stat == "strength" && strength > 1)
        {
            strength -= 1;
            statPoints += 1;
        }
        else if (stat == "dexterity" && dexterity > 1)
        {
            dexterity -= 1;
            statPoints += 1;
        }
        else if (stat == "constitution" && constitution > 1)
        {
            constitution -= 1;
            statPoints += 1;
        }
    }

    public void saveStats()
    {
        playerStats.statPoints = statPoints;
        playerStats.strength = strength;
        playerStats.dexterity = dexterity;
        playerStats.constitution = constitution;
    }

    public void toggleDarkness()
    {
        if (darknessEnabled)
        {
            darknessEnabled = false;
        }
        else
        {
            darknessEnabled = true;
        }
    }

    public void saveSettings()
    {
        gameSettings.isDarknessEnabled = darknessEnabled;
    }
}
