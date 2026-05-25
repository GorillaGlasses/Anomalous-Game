using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsScriptable", menuName = "Scriptable Objects/GameSettingsScriptable")]
public class GameSettingsScriptable : ScriptableObject
{
    public bool isDarknessEnabled = true;
    
    private void OnEnable()
    {
        isDarknessEnabled = true;
    }
}
