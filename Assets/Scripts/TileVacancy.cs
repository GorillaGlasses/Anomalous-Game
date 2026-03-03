using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileVacancy : MonoBehaviour
{
    public GameObject occupant;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Occupied by " + collision.gameObject.tag);
        occupant = collision.gameObject;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name + " left");
        occupant = null;
    }
}
