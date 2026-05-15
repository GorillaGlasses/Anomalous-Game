using NUnit.Framework.Constraints;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    public GameObject levelCompleteScreen;
    public GameObject HUD;

    void Start()
    {
        levelCompleteScreen = GameObject.FindGameObjectWithTag("LevelComplete");
        HUD = GameObject.FindGameObjectWithTag("HUD");
    }

    void Update(){
        if (gameObject.GetComponent<TileVacancy>().occupant != null && gameObject.GetComponent<TileVacancy>().occupant.CompareTag("Player"))
        {
            levelCompleteScreen.GetComponent<MenuManager>().setMenuActive();
            HUD.GetComponent<MenuManager>().setMenuInactive();
            gameObject.GetComponent<TileVacancy>().occupant.GetComponent<PlayerController>().finishedLevel = true;
        }
    }
}
