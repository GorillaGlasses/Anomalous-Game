using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorOpening : MonoBehaviour
{
    TileVacancy tileVac;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       tileVac = gameObject.GetComponent<TileVacancy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
