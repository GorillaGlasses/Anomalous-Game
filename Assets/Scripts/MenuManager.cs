using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public List<GameObject> menuObjects;

    public void startLevel()
    {
        SceneManager.LoadScene(SceneManager.GetSceneByName("Dungeon").buildIndex);
    }

    public void returnToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetSceneByName("MainMenu").buildIndex);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void setMenuActive(){
        for (int i = 0; i < menuObjects.Count; i++)
        {
            menuObjects[i].SetActive(true);
        }
    }

    public void setMenuInactive(){
        for (int i = 0; i < menuObjects.Count; i++)
        {
            menuObjects[i].SetActive(false);
        }
    }

    public void changeText(TextMeshProUGUI textBox, string newText)
    {
        textBox.text = newText;
    }
}
