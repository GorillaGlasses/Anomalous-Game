using NUnit.Framework.Constraints;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    public GameObject levelCompleteScreen;

    void Start()
    {
        levelCompleteScreen = GameObject.FindGameObjectWithTag("LevelComplete");
    }

    void Update(){
        if (gameObject.GetComponent<TileVacancy>().occupant != null && gameObject.GetComponent<TileVacancy>().occupant.CompareTag("Player"))
        {
            levelCompleteScreen.GetComponent<MenuManager>().setMenuActive();
            gameObject.GetComponent<TileVacancy>().occupant.GetComponent<PlayerController>().inMenu = true;
        }
    }
}
