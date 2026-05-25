using UnityEngine;

public class DarknessToggle : MonoBehaviour
{
    public GameObject darknessCover;
    public bool isDiscovered = false;
    public GameSettingsScriptable gameSettings;

    // Start is called before the first frame update
    void Start()
    {
        if (gameSettings.isDarknessEnabled)
        {
            darknessCover.SetActive(true);
        }
        else
        {
            darknessCover.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDiscovered)
        {
            darknessCover.SetActive(false);
        }
    }
}
