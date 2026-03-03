using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public Rigidbody2D playerBody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Basic Movement Code
        if(Input.GetKeyDown(KeyCode.W)){
            checkDestinationEmpty(new UnityEngine.Vector2(playerBody.position.x, playerBody.position.y + moveSpeed));
        }

        if(Input.GetKeyDown(KeyCode.A)){
            checkDestinationEmpty(new UnityEngine.Vector2(playerBody.position.x - moveSpeed, playerBody.position.y));
        }

        if(Input.GetKeyDown(KeyCode.S)){
            checkDestinationEmpty(new UnityEngine.Vector2(playerBody.position.x, playerBody.position.y - moveSpeed));
        }

        if(Input.GetKeyDown(KeyCode.D)){
            checkDestinationEmpty(new UnityEngine.Vector2(playerBody.position.x + moveSpeed, playerBody.position.y));
        }
    }

    //Checks if the spot the player is about to move to is an unoccupied tile.
    void checkDestinationEmpty(UnityEngine.Vector2 direction)
    {
        UnityEngine.Vector2 targetSpot = direction; 
        Collider2D spotCheck = Physics2D.OverlapPoint(targetSpot);

        if (spotCheck.gameObject.TryGetComponent<TileVacancy>(out TileVacancy tVac) && tVac.occupant == null)
        {
            playerBody.position = targetSpot;
        }
        return;
    }
}
