using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public int actionCount = 0;
    public Rigidbody2D playerBody;
    private Transform camPos;
    private bool camLocked = true;
    public bool inMenu = false;
    public TurnManagement turnManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // In start, find the camera object and its transform, then put that transform into the camPos variable and change the position to hover over the player
    void Start()
    {
        GameObject.FindGameObjectWithTag("MainCamera").TryGetComponent<Transform>(out Transform camForm);        
        camPos = camForm;
        camPos.position = new UnityEngine.Vector3(playerBody.position.x, playerBody.position.y, -10);
        turnManager = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        // Basic Movement Code, calls a function to make sure the next tile can be moved to.
        if(Input.GetKeyDown(KeyCode.W) && !inMenu && turnManager.playerTurn){
            if(camLocked){
                checkDestinationEmpty(new UnityEngine.Vector2(playerBody.position.x, playerBody.position.y + moveSpeed));
            }
            else
            {
                camPos.position = new UnityEngine.Vector3(camPos.position.x, camPos.position.y + moveSpeed, -10);
            }
        }

        if(Input.GetKeyDown(KeyCode.A) && !inMenu && turnManager.playerTurn){
            if(camLocked){
                checkDestinationEmpty(new UnityEngine.Vector2(playerBody.position.x - moveSpeed, playerBody.position.y));
            }
            else
            {
                camPos.position = new UnityEngine.Vector3(camPos.position.x - moveSpeed, camPos.position.y, -10);
            }
        }

        if(Input.GetKeyDown(KeyCode.S) && !inMenu && turnManager.playerTurn){
            if(camLocked){
                checkDestinationEmpty(new UnityEngine.Vector2(playerBody.position.x, playerBody.position.y - moveSpeed));
            }
            else
            {
                camPos.position = new UnityEngine.Vector3(camPos.position.x, camPos.position.y - moveSpeed, -10);
            }
        }

        if(Input.GetKeyDown(KeyCode.D) && !inMenu && turnManager.playerTurn){
            if(camLocked){
                checkDestinationEmpty(new UnityEngine.Vector2(playerBody.position.x + moveSpeed, playerBody.position.y));
            }
            else
            {
                camPos.position = new UnityEngine.Vector3(camPos.position.x + moveSpeed, camPos.position.y, -10);
            }
        }
        
        // Enables camera movement by unlocking the camera to the player. When locked off, instead of moving the player, the camera instead moves. When locked on, 
        // the camera is set to the player's position and follows the player.
        if (Input.GetKeyDown(KeyCode.L) && !inMenu && turnManager.playerTurn)
        {
            if (camLocked)
            {
                camLocked = false;
            } 
            else
            {
                camLocked = true;
                camPos.position = new UnityEngine.Vector3(playerBody.position.x, playerBody.position.y, -10);
            }
        }
    }

    //Checks if the spot the player is about to move to is an unoccupied tile. Moves player and camera to tile if it is open, and does nothing if not.
    void checkDestinationEmpty(UnityEngine.Vector2 direction)
    {
        UnityEngine.Vector2 targetSpot = direction; 
        Collider2D spotCheck = Physics2D.OverlapPoint(targetSpot);

        if (spotCheck.gameObject.TryGetComponent<TileVacancy>(out TileVacancy tVac) && tVac.occupant == null)
        {
            playerBody.position = targetSpot;
            camPos.position = new UnityEngine.Vector3(targetSpot.x, targetSpot.y, -10);
            actionCount += 100;
        } else if (spotCheck.gameObject.TryGetComponent<DoorOpening>(out DoorOpening dOpen))
        {
            dOpen.useDoor();
            actionCount += 100;
        }
        return;
    }
}
