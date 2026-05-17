using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class GameSettings : MonoBehaviour
{
    public int statPoints = 3;
    public TextMeshProUGUI pointDisplay;
    public int strength;
    public TextMeshProUGUI strengthDisplay;
    public int dexterity;
    public TextMeshProUGUI dexterityDisplay;
    public int constitution;
    public TextMeshProUGUI constitutionDisplay;
    public PlayerStatsScriptableObject playerStats;

    void Start()
    {
        statPoints = playerStats.statPoints;
        strength = playerStats.strength;
        dexterity = playerStats.dexterity;
        constitution = playerStats.constitution;
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
}
