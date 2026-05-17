using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsScriptableObject", menuName = "Scriptable Objects/PlayerStatsScriptableObjectScript")]
public class PlayerStatsScriptableObject : ScriptableObject
{
    public int statPoints = 3;
    public int strength = 1;
    public int dexterity = 1;
    public int constitution = 1;

    private void OnEnable()
    {
        statPoints = 3;
        strength = 1;
        dexterity = 1;
        constitution = 1;
    }
}
