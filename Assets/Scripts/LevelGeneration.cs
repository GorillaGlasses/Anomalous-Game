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



    public class DoorInstance
    {
        public UnityEngine.Vector2 doorLocation;
        public string direction;
        public DoorInstance(UnityEngine.Vector2 doorLoc, string dir)
        {
            doorLocation = doorLoc;
            direction = dir;
        }

        public UnityEngine.Vector2 getLocation()
        {
            return doorLocation;
        }

        public string getDirection()
        {
            return direction;
        }
    }

    public List<DoorInstance> DoorInstances = new List<DoorInstance>(); // List of door instances that is updated as the level generates.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomGen(UnityEngine.Random.Range(5,15), UnityEngine.Random.Range(5,15), randomCoordGeneration(), randomCoordGeneration(), "Right");

        pathGen(8f, DoorInstances.Last().getLocation(), DoorInstances.Last().getDirection());

        int roomLength = UnityEngine.Random.Range(5,15);
        int roomWidth = UnityEngine.Random.Range(5,15);
        if(DoorInstances.Last().getDirection() == "Right")
        {
            roomGen(roomWidth, roomLength, DoorInstances.Last().getLocation().x+1.5f, DoorInstances.Last().getLocation().y+((roomLength)/2)*1.5f, DoorInstances.Last().getDirection());
        }else if (DoorInstances.Last().getDirection() == "Left")
        {
            roomGen(roomWidth, roomLength, DoorInstances.Last().getLocation().x-(roomWidth)*1.5f, DoorInstances.Last().getLocation().y+((roomLength)/2)*1.5f, DoorInstances.Last().getDirection());
        }else if (DoorInstances.Last().getDirection() == "Up")
        {
            roomGen(roomWidth, roomLength, DoorInstances.Last().getLocation().x-((roomWidth)/2)*1.5f, DoorInstances.Last().getLocation().y+(roomLength)*1.5f, DoorInstances.Last().getDirection());
        }else if(DoorInstances.Last().getDirection() == "Down")
        {
            roomGen(roomWidth, roomLength, DoorInstances.Last().getLocation().x-((roomWidth)/2)*1.5f, DoorInstances.Last().getLocation().y-1.5f, DoorInstances.Last().getDirection());
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
                        DoorInstances.Add(new DoorInstance(workingPos, "Right"));
                    } else if (xAdjust < 0)
                    {
                        DoorInstances.Add(new DoorInstance(workingPos, "Left"));
                    }else if (yAdjust > 0)
                    {
                        DoorInstances.Add(new DoorInstance(workingPos, "Up"));
                    }else if (yAdjust < 0)
                    {
                        DoorInstances.Add(new DoorInstance(workingPos, "Down"));
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
    void roomGen(float roomWidth, float roomLength, float xPos, float yPos, string startDir)
    {
        if (DoorInstances.Count() != 0)
        {
            DoorInstances.RemoveAt(DoorInstances.Count-1);
        }
        float generationX = xPos; // !!! The way the starting position is picked is going to need to be adjusted to align with the path gen and overall level gen !!! 
        float generationY = yPos;
        int playerPlace = (int)UnityEngine.Random.Range(1, roomWidth*roomLength);
        int tileCount = 0;
        int doorCheck; // Used to randomly generate a number 
        bool doorPlaced = false;
        Collider2D spotCheck;
        for (int i = 0; i < roomLength; i++)
        {
            for (int j = 0; j < roomWidth; j++)
            {
                Instantiate(tile, new UnityEngine.Vector3(generationX,generationY,0), transform.rotation);
                doorCheck = UnityEngine.Random.Range(1,101);
                if(j == 0 && i == (roomLength-1)/2 && startDir != "Right" && placedRooms < roomCount && (doorCheck <= doorChance || doorPlaced == false)){// Left door generation
                    spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX-1.5f,generationY));
                    if(spotCheck == null){
                        Instantiate(door, new UnityEngine.Vector3(generationX-1.5f,generationY,0), transform.rotation);
                        DoorInstances.Add(new DoorInstance(new UnityEngine.Vector2(generationX-1.5f,generationY), "Left"));
                        doorPlaced = true;
                    }
                } else if(j == 0){// Wall generation
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
                } else if(j == roomWidth-1 && i == (roomLength-1)/2 && startDir != "Left" && placedRooms < roomCount && (doorCheck <= doorChance || doorPlaced == false)){// Right door generation
                    spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX+1.5f,generationY));
                    if(spotCheck == null){
                        Instantiate(door, new UnityEngine.Vector3(generationX+1.5f,generationY,0), transform.rotation);
                        DoorInstances.Add(new DoorInstance(new UnityEngine.Vector2(generationX+1.5f,generationY), "Right"));
                        doorPlaced = true;
                    }
                } else if (j == roomWidth-1)
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

                if(i == 0 && j == (roomWidth-1)/2 && startDir != "Down" && placedRooms < roomCount && (doorCheck <= doorChance || doorPlaced == false)){// Top door generation
                    spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX,generationY+1.5f));
                    if(spotCheck == null){
                        Instantiate(door, new UnityEngine.Vector3(generationX,generationY+1.5f,0), transform.rotation);
                        DoorInstances.Add(new DoorInstance(new UnityEngine.Vector2(generationX,generationY+1.5f), "Up"));
                        doorPlaced = true;
                    }
                } else if (i == 0)
                {
                    spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX,generationY+1.5f));
                    if(spotCheck == null){
                        Instantiate(wall, new UnityEngine.Vector3(generationX,generationY+1.5f,0), transform.rotation);
                    }
                } else if(i == 0 && j == (roomWidth-1)/2 && startDir != "Up" && placedRooms < roomCount && (doorCheck <= doorChance || doorPlaced == false)){// Bottom door generation
                    spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX,generationY-1.5f));
                    if(spotCheck == null){
                        Instantiate(door, new UnityEngine.Vector3(generationX,generationY-1.5f,0), transform.rotation);
                        DoorInstances.Add(new DoorInstance(new UnityEngine.Vector2(generationX,generationY-1.5f), "Down"));
                        doorPlaced = true;
                    }
                }
                else if (i == roomLength-1)
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
