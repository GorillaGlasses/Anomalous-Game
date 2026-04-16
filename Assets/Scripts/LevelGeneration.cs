using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class levelGeneration : MonoBehaviour
{
    public GameObject tile;
    public GameObject wall;
    public GameObject door;
    public GameObject player;
    public int turnChance = 0;
    public int doorChance = 50;
    public float gridInterval = 1.5f;
    public int roomCount = 5;
    public int placedRooms = 0;
    public int playerCount = 1;
    public int placedPlayers = 0;
    public bool exitRoom = false; // When true, the next roomGen will generate an exit to the floor.



    public class DoorInstance // Class that tracks individual doors that are created to keep track of location and direction
    {
        public UnityEngine.Vector2 doorLocation;
        public string direction;
        bool isFromPath;
        public DoorInstance(UnityEngine.Vector2 doorLoc, string dir, bool fromPath)
        {
            doorLocation = doorLoc;
            direction = dir;
            isFromPath = fromPath;
        }

        public UnityEngine.Vector2 getLocation()
        {
            return doorLocation;
        }

        public string getDirection()
        {
            return direction;
        }

        public bool fromPath()
        {
            return isFromPath;
        }
    }

    public List<DoorInstance> DoorInstances = new List<DoorInstance>(); // List of door instances that is updated as the level generates.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        int roomLength = UnityEngine.Random.Range(5,15);
        int roomWidth = UnityEngine.Random.Range(5,15);
        roomGen(roomLength, roomWidth, randomCoordGeneration(), randomCoordGeneration(), "Right");

        while(placedRooms < roomCount){
            if(!DoorInstances.Last().fromPath()){
                pathGen(UnityEngine.Random.Range(4,11), DoorInstances.Last().getLocation(), DoorInstances.Last().getDirection());
            }
            
            if(DoorInstances.Last().fromPath()){
                if(DoorInstances.Last().getDirection() == "Right")
                {
                    roomLength = UnityEngine.Random.Range(5,15);
                    roomWidth = UnityEngine.Random.Range(5,15);
                    roomGen(roomWidth, roomLength, DoorInstances.Last().getLocation().x+gridInterval, DoorInstances.Last().getLocation().y+((roomLength)/2)*gridInterval, DoorInstances.Last().getDirection());
                }else if (DoorInstances.Last().getDirection() == "Left")
                {
                    roomLength = UnityEngine.Random.Range(5,15);
                    roomWidth = UnityEngine.Random.Range(5,15);
                    roomGen(roomWidth, roomLength, DoorInstances.Last().getLocation().x-(roomWidth)*gridInterval, DoorInstances.Last().getLocation().y+((roomLength)/2)*gridInterval, DoorInstances.Last().getDirection());
                }else if (DoorInstances.Last().getDirection() == "Up")
                {
                    roomLength = UnityEngine.Random.Range(5,15);
                    roomWidth = UnityEngine.Random.Range(5,15);
                    roomGen(roomWidth, roomLength, DoorInstances.Last().getLocation().x-((roomWidth)/2)*gridInterval, DoorInstances.Last().getLocation().y+(roomLength)*gridInterval, DoorInstances.Last().getDirection());
                }else if(DoorInstances.Last().getDirection() == "Down")
                {
                    roomLength = UnityEngine.Random.Range(5,15);
                    roomWidth = UnityEngine.Random.Range(5,15);
                    roomGen(roomWidth, roomLength, DoorInstances.Last().getLocation().x-((roomWidth)/2)*gridInterval, DoorInstances.Last().getLocation().y-gridInterval, DoorInstances.Last().getDirection());
                }
            }
        }

        doorChance = 0;

        while (DoorInstances.Count() != 0)
        {
            if(!DoorInstances.Last().fromPath()){
                pathGen(UnityEngine.Random.Range(4,11), DoorInstances.Last().getLocation(), DoorInstances.Last().getDirection());
            }
            
            if(DoorInstances.Last().fromPath()){
                if(DoorInstances.Last().getDirection() == "Right")
                {
                    roomLength = UnityEngine.Random.Range(5,15);
                    roomWidth = UnityEngine.Random.Range(5,15);
                    roomGen(roomWidth, roomLength, DoorInstances.Last().getLocation().x+gridInterval, DoorInstances.Last().getLocation().y+((roomLength)/2)*gridInterval, DoorInstances.Last().getDirection());
                }else if (DoorInstances.Last().getDirection() == "Left")
                {
                    roomLength = UnityEngine.Random.Range(5,15);
                    roomWidth = UnityEngine.Random.Range(5,15);
                    roomGen(roomWidth, roomLength, DoorInstances.Last().getLocation().x-(roomWidth)*gridInterval, DoorInstances.Last().getLocation().y+((roomLength)/2)*gridInterval, DoorInstances.Last().getDirection());
                }else if (DoorInstances.Last().getDirection() == "Up")
                {
                    roomLength = UnityEngine.Random.Range(5,15);
                    roomWidth = UnityEngine.Random.Range(5,15);
                    roomGen(roomWidth, roomLength, DoorInstances.Last().getLocation().x-((roomWidth)/2)*gridInterval, DoorInstances.Last().getLocation().y+(roomLength)*gridInterval, DoorInstances.Last().getDirection());
                }else if(DoorInstances.Last().getDirection() == "Down")
                {
                    roomLength = UnityEngine.Random.Range(5,15);
                    roomWidth = UnityEngine.Random.Range(5,15);
                    roomGen(roomWidth, roomLength, DoorInstances.Last().getLocation().x-((roomWidth)/2)*gridInterval, DoorInstances.Last().getLocation().y-gridInterval, DoorInstances.Last().getDirection());
                }
            }
        }
    }

    //Generates a random path 
    void pathGen(float tileNum, UnityEngine.Vector2 startingPos, string startDir)
    {
        if (DoorInstances.Count() != 0)
        {
            DoorInstances.RemoveAt(DoorInstances.Count-1);
        }
        int directionChance;
        float xAdjust = 0;
        float yAdjust = 0;

        if (startDir == "Right")
        {
            xAdjust = 1.5f;
            yAdjust = 0;
        }else if (startDir == "Left")
        {
            xAdjust = -1.5f;
            yAdjust = 0;
        }else if (startDir == "Up")
        {
            xAdjust = 0;
            yAdjust = 1.5f;
        }else if (startDir == "Down")
        {
            xAdjust = 0;
            yAdjust = -1.5f;
        }

        UnityEngine.Debug.Log(tileNum + "\nXAdj: " + xAdjust + "\nYAdj: " + yAdjust + "\nXPos: " + startingPos.x + "\nYPos: " + startingPos.y);
        UnityEngine.Vector2 workingPos = new UnityEngine.Vector2(startingPos.x + xAdjust, startingPos.y + yAdjust);
        Collider2D spotCheck = Physics2D.OverlapPoint(workingPos);
        for (int i = 0; i < tileNum; i++)
        {
            if (spotCheck == null) // So long as the next tile is vacant we can proceed
            {
                if (i == tileNum-1)
                {
                    Instantiate(door, workingPos, transform.rotation);
                    if(xAdjust > 0){
                        DoorInstances.Add(new DoorInstance(workingPos, "Right", true));
                    } else if (xAdjust < 0)
                    {
                        DoorInstances.Add(new DoorInstance(workingPos, "Left", true));
                    }else if (yAdjust > 0)
                    {
                        DoorInstances.Add(new DoorInstance(workingPos, "Up", true));
                    }else if (yAdjust < 0)
                    {
                        DoorInstances.Add(new DoorInstance(workingPos, "Down", true));
                    }
                } else {
                    Instantiate(tile, workingPos, transform.rotation);
                }
                placeAdjacentWalls(workingPos, xAdjust, yAdjust);
                directionChance = UnityEngine.Random.Range(1,101);
                if (directionChance > turnChance) // If chance check is greater than the chance to turn, we continue in the same direction, not turning.
                {
                    UnityEngine.Debug.Log("Didn't turn: " + directionChance+ "/" + turnChance);
                    workingPos = new UnityEngine.Vector2(workingPos.x + xAdjust, workingPos.y + yAdjust);
                } else // Otherwise, turn left or right in relation to the current direction.
                {
                    if(xAdjust > 0) // Handles if the previous direction was going RIGHT
                    {
                        directionChance = UnityEngine.Random.Range(1,3);
                        switch (directionChance)
                        {
                            case 1: // Going up
                                xAdjust = 0;
                                yAdjust = 1.5f;
                                break;
                            case 2: // Going down
                                xAdjust = 0;
                                yAdjust = -1.5f;
                                break;
                        }
                        placeAdjacentWalls(workingPos, xAdjust, yAdjust);
                    }else if (xAdjust < 0) // Handles if the previous direction was going LEFT
                    {
                        directionChance = UnityEngine.Random.Range(1,3);
                        switch (directionChance)
                        {
                            case 1: // Going down
                                xAdjust = 0;
                                yAdjust = -1.5f;
                                break;
                            case 2: // Going up
                                xAdjust = 0;
                                yAdjust = 1.5f;
                                break;
                        }
                        placeAdjacentWalls(workingPos, xAdjust, yAdjust);
                    } else if (yAdjust > 0) // Handles if the previous direction was going UP
                    {
                        directionChance = UnityEngine.Random.Range(1,3);
                        switch (directionChance)
                        {
                            case 1: // Going left
                                xAdjust = -1.5f;
                                yAdjust = 0;
                                break;
                            case 2: // Going right
                                xAdjust = 1.5f;
                                yAdjust = 0;
                                break;
                        }
                        placeAdjacentWalls(workingPos, xAdjust, yAdjust);
                    } else if(yAdjust < 0) // Handles if the previous direction was going DOWN
                    {
                        directionChance = UnityEngine.Random.Range(1,3);
                        switch (directionChance)
                        {
                            case 1: // Going right
                                xAdjust = 1.5f;
                                yAdjust = 0;
                                break;
                            case 2: // Going left
                                xAdjust = -1.5f;
                                yAdjust = 0;
                                break;
                        }
                        placeAdjacentWalls(workingPos, xAdjust, yAdjust);
                    }

                    workingPos = new UnityEngine.Vector2(workingPos.x + xAdjust, workingPos.y + yAdjust);
                    UnityEngine.Debug.Log("DID turn: " + directionChance+ "/" + turnChance + "\nXAdj: " + xAdjust + "\nYAdj: " + yAdjust);
                }
                
            } else // If it is not vacant, we must change positions until we are moving freely.
            {
                i += -1;
                workingPos = new UnityEngine.Vector2(workingPos.x - xAdjust, workingPos.y - yAdjust);
                if(xAdjust > 0) // Handles if the previous direction was going RIGHT
                {
                    directionChance = UnityEngine.Random.Range(1,3);
                    switch (directionChance)
                    {
                        case 1: // Going up
                            xAdjust = 0;
                            yAdjust = 1.5f;
                            break;
                        case 2: // Going down
                            xAdjust = 0;
                            yAdjust = -1.5f;
                            break;
                    }
                }else if (xAdjust < 0) // Handles if the previous direction was going LEFT
                {
                    directionChance = UnityEngine.Random.Range(1,3);
                    switch (directionChance)
                    {
                        case 1: // Going down
                            xAdjust = 0;
                            yAdjust = -1.5f;
                            break;
                        case 2: // Going up
                            xAdjust = 0;
                            yAdjust = 1.5f;
                            break;
                    }
                } else if (yAdjust > 0) // Handles if the previous direction was going UP
                {
                    directionChance = UnityEngine.Random.Range(1,3);
                    switch (directionChance)
                    {
                        case 1: // Going left
                            xAdjust = -1.5f;
                            yAdjust = 0;
                            break;
                        case 2: // Going right
                            xAdjust = 1.5f;
                            yAdjust = 0;
                            break;
                    }
                } else if(yAdjust < 0) // Handles if the previous direction was going DOWN
                {
                    directionChance = UnityEngine.Random.Range(1,3);
                    switch (directionChance)
                    {
                        case 1: // Going right
                            xAdjust = 1.5f;
                            yAdjust = 0;
                            break;
                        case 2: // Going left
                            xAdjust = -1.5f;
                            yAdjust = 0;
                            break;
                    }
                }

                workingPos = new UnityEngine.Vector2(workingPos.x + xAdjust, workingPos.y + yAdjust);
            }
            spotCheck = Physics2D.OverlapPoint(workingPos);
        }
    }

    // Generates tiles in a rectangular formation based on the assigned parameters. The outermost tiles are all brick walls instead. 
    // Also randomly spawns the player on one of these tiles.
    void roomGen(float roomWidth, float roomLength, float xPos, float yPos, string startDir) // ! Consider reworking this to make it so the doors are randomly predetermined instead of the fixed order they are currently checked in. !
    {
        if (DoorInstances.Count() != 0)
        {
            DoorInstances.RemoveAt(DoorInstances.Count-1);
        }
        float generationX = xPos;
        float generationY = yPos;
        int playerPlace = (int)UnityEngine.Random.Range(1, roomWidth*roomLength);
        int tileCount = 0;
        int doorCheck = 0; // Used to randomly generate a number 
        Collider2D spotCheck;
        for (int i = 0; i < roomLength; i++)
        {
            for (int j = 0; j < roomWidth; j++)
            {
                spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX, generationY));
                if(spotCheck == null){
                    Instantiate(tile, new UnityEngine.Vector3(generationX,generationY,0), transform.rotation);
                }

                if(j == 0 && i == (int)((roomLength-1)/2) && startDir != "Right" && placedRooms < roomCount){// Left door generation
                    doorCheck = UnityEngine.Random.Range(1,101);
                    UnityEngine.Debug.Log("Door Chance is: " + doorCheck + "/" + doorChance);
                    if(doorCheck <= doorChance){
                        spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX-1.5f,generationY));
                        if(spotCheck == null){
                            Instantiate(door, new UnityEngine.Vector3(generationX-1.5f,generationY,0), transform.rotation);
                            DoorInstances.Add(new DoorInstance(new UnityEngine.Vector2(generationX-1.5f,generationY), "Left", false));
                        }
                    }
                } else if(j == roomWidth-1 && i == (int)((roomLength-1)/2) && startDir != "Left" && placedRooms < roomCount){// Right door generation
                    doorCheck = UnityEngine.Random.Range(1,101);
                    UnityEngine.Debug.Log("Door Chance is: " + doorCheck + "/" + doorChance);
                    if(doorCheck <= doorChance){
                        spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX+1.5f,generationY));
                        if(spotCheck == null){
                            Instantiate(door, new UnityEngine.Vector3(generationX+1.5f,generationY,0), transform.rotation);
                            DoorInstances.Add(new DoorInstance(new UnityEngine.Vector2(generationX+1.5f,generationY), "Right", false));
                        }
                    }
                } 
                
                if(i == 0 && j == (int)((roomWidth-1)/2) && startDir != "Down" && placedRooms < roomCount){// Top door generation
                    doorCheck = UnityEngine.Random.Range(1,101);
                    UnityEngine.Debug.Log("Door Chance is: " + doorCheck + "/" + doorChance);
                    if(doorCheck <= doorChance){
                        spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX,generationY+1.5f));
                        if(spotCheck == null){
                            Instantiate(door, new UnityEngine.Vector3(generationX,generationY+1.5f,0), transform.rotation);
                            DoorInstances.Add(new DoorInstance(new UnityEngine.Vector2(generationX,generationY+1.5f), "Up", false));
                        }
                    }
                } else if(i == roomLength-1 && j == (int)((roomWidth-1)/2) && startDir != "Up" && placedRooms < roomCount){// Bottom door generation
                    doorCheck = UnityEngine.Random.Range(1,101);
                    UnityEngine.Debug.Log("Door Chance is: " + doorCheck + "/" + doorChance);
                    if(doorCheck <= doorChance){
                        spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX,generationY-1.5f));
                        if(spotCheck == null){
                            Instantiate(door, new UnityEngine.Vector3(generationX,generationY-1.5f,0), transform.rotation);
                            DoorInstances.Add(new DoorInstance(new UnityEngine.Vector2(generationX,generationY-1.5f), "Down", false));
                        }
                    }
                }
                
                if(j == 0){// Wall generation
                    spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX-1.5f,generationY));
                    // Generates wall to the left
                    if(spotCheck == null){
                        Instantiate(wall, new UnityEngine.Vector3(generationX-1.5f,generationY,0), transform.rotation);
                    }
                    //Places walls in the left corners
                    if (i == 0)
                    {
                        spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX-1.5f,generationY+1.5f));
                        if(spotCheck == null){
                            Instantiate(wall, new UnityEngine.Vector3(generationX-1.5f,generationY+1.5f,0), transform.rotation);
                        }
                    }
                    else if (i == roomLength-1)
                    {
                        spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX-1.5f,generationY-1.5f));
                        if(spotCheck == null){
                            Instantiate(wall, new UnityEngine.Vector3(generationX-1.5f,generationY-1.5f,0), transform.rotation);
                        }
                    }
                } 

                if (j == roomWidth-1)
                {
                    spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX+1.5f,generationY));
                    // Generates wall to the right
                    if(spotCheck == null){
                        Instantiate(wall, new UnityEngine.Vector3(generationX+1.5f,generationY,0), transform.rotation);
                    }
                    //Places walls in the right corners
                    if (i == 0)
                    {
                        spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX+1.5f,generationY+1.5f));
                        if(spotCheck == null){
                            Instantiate(wall, new UnityEngine.Vector3(generationX+1.5f,generationY+1.5f,0), transform.rotation);
                        }
                    }
                    else if (i == roomLength-1)
                    {
                        spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX+1.5f,generationY-1.5f));
                        if(spotCheck == null){
                            Instantiate(wall, new UnityEngine.Vector3(generationX+1.5f,generationY-1.5f,0), transform.rotation);
                        }
                    }
                }

                if (i == 0)
                {
                    spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX,generationY+1.5f));
                    if(spotCheck == null){
                        Instantiate(wall, new UnityEngine.Vector3(generationX,generationY+1.5f,0), transform.rotation);
                    }
                } else if (i == roomLength-1)
                {
                    spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX,generationY-1.5f));
                    if(spotCheck == null){
                        Instantiate(wall, new UnityEngine.Vector3(generationX,generationY-1.5f,0), transform.rotation);
                    }
                }
        
                tileCount++;
                if (tileCount == playerPlace && placedPlayers < playerCount)
                {
                    Instantiate(player, new UnityEngine.Vector3(generationX,generationY,0), transform.rotation);
                    placedPlayers += 1;
                }
                generationX += 1.5f;
            }
            // Moves down one row and back to the first column.
            generationX = xPos;
            generationY += -1.5f;
        }
        placedRooms += 1;
    }
    
    // Generates a coordinate randomly within the range of -60 to 60.
    float randomCoordGeneration()
    {   
        float intervalPosition = UnityEngine.Random.Range(-60,60);
        float positionFloor = Mathf.Floor(intervalPosition / gridInterval);
        intervalPosition = positionFloor * gridInterval;
        return intervalPosition;
    }

    // This function places a wall tile in each unnocupied space to the sides of the
    // given position, based on the given direction.
    void placeAdjacentWalls(UnityEngine.Vector2 position, float baseX, float baseY)
    {
        UnityEngine.Vector2 workingPos;
        Collider2D spotCheck;
        if(baseX > 0) // Handles if the previous direction was going RIGHT
        {
            // Back left of position
            workingPos = new UnityEngine.Vector2(position.x-1.5f, position.y+1.5f);
            spotCheck = Physics2D.OverlapPoint(workingPos);
            if(spotCheck == null){
                Instantiate(wall, workingPos, transform.rotation);
            }

            // Back right of position
            workingPos = new UnityEngine.Vector2(position.x-1.5f, position.y-1.5f);
            spotCheck = Physics2D.OverlapPoint(workingPos);
            if(spotCheck == null){
                Instantiate(wall, workingPos, transform.rotation);
            }

            // Back of position
            workingPos = new UnityEngine.Vector2(position.x-1.5f, position.y);
            spotCheck = Physics2D.OverlapPoint(workingPos);
            if(spotCheck == null){
                Instantiate(wall, workingPos, transform.rotation);
            }
        }else if (baseX < 0) // Handles if the previous direction was going LEFT
        {
            // Back left of position
            workingPos = new UnityEngine.Vector2(position.x+1.5f, position.y-1.5f);
            spotCheck = Physics2D.OverlapPoint(workingPos);
            if(spotCheck == null){
                Instantiate(wall, workingPos, transform.rotation);
            }

            // Back right of position
            workingPos = new UnityEngine.Vector2(position.x+1.5f, position.y+1.5f);
            spotCheck = Physics2D.OverlapPoint(workingPos);
            if(spotCheck == null){
                Instantiate(wall, workingPos, transform.rotation);
            }

            // Back of position
            workingPos = new UnityEngine.Vector2(position.x+1.5f, position.y);
            spotCheck = Physics2D.OverlapPoint(workingPos);
            if(spotCheck == null){
                Instantiate(wall, workingPos, transform.rotation);
            }
        } else if (baseY > 0) // Handles if the previous direction was going UP
        {
            // Back left of position
            workingPos = new UnityEngine.Vector2(position.x-1.5f, position.y-1.5f);
            spotCheck = Physics2D.OverlapPoint(workingPos);
            if(spotCheck == null){
                Instantiate(wall, workingPos, transform.rotation);
            }

            // Back right of position
            workingPos = new UnityEngine.Vector2(position.x+1.5f, position.y-1.5f);
            spotCheck = Physics2D.OverlapPoint(workingPos);
            if(spotCheck == null){
                Instantiate(wall, workingPos, transform.rotation);
            }

            // Back of position
            workingPos = new UnityEngine.Vector2(position.x, position.y-1.5f);
            spotCheck = Physics2D.OverlapPoint(workingPos);
            if(spotCheck == null){
                Instantiate(wall, workingPos, transform.rotation);
            }
        } else if(baseY < 0) // Handles if the previous direction was going DOWN
        {
            // Back left of position
            workingPos = new UnityEngine.Vector2(position.x+1.5f, position.y+1.5f);
            spotCheck = Physics2D.OverlapPoint(workingPos);
            if(spotCheck == null){
                Instantiate(wall, workingPos, transform.rotation);
            }

            // Back right of position
            workingPos = new UnityEngine.Vector2(position.x-1.5f, position.y+1.5f);
            spotCheck = Physics2D.OverlapPoint(workingPos);
            if(spotCheck == null){
                Instantiate(wall, workingPos, transform.rotation);
            }

            // Back of position
            workingPos = new UnityEngine.Vector2(position.x, position.y+1.5f);
            spotCheck = Physics2D.OverlapPoint(workingPos);
            if(spotCheck == null){
                Instantiate(wall, workingPos, transform.rotation);
            }
        }
    }

}
