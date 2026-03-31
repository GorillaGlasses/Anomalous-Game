using System;
using System.Collections;
using System.Diagnostics;
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
    public int turnChance = 15;
    public int branchChance = 60; // Percent chance for the path to branch 
    public float gridInterval = 1.5f;
    public int roomCount = 5;
    public int playerCount = 1;
    public bool exitRoom = false; // When true, the next roomGen will generate an exit to the floor.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pathGen(25f, new UnityEngine.Vector2(0/*randomCoordGeneration()*/, 1.5f/*randomCoordGeneration()*/), 0, 1.5f);
        roomGen(UnityEngine.Random.Range(5,15), UnityEngine.Random.Range(5,15), 0/*randomCoordGeneration()*/, 0/*randomCoordGeneration()*/);
    }

    //Generates a random path 
    void pathGen(float tileNum, UnityEngine.Vector2 startingPos, float baseX, float baseY)
    {
        int directionChance;
        float xAdjust = baseX;
        float yAdjust = baseY;
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
    void roomGen(float roomWidth, float roomLength, float xPos, float yPos)
    {
        float generationX = xPos; // !!! The way the starting position is picked is going to need to be adjusted to align with the path gen and overall level gen !!! 
        float generationY = yPos;
        int playerPlace = (int)UnityEngine.Random.Range(1, roomWidth*roomLength);
        int tileCount = 0;
        Collider2D spotCheck;
        for (int i = 0; i < roomLength; i++)
        {
            for (int j = 0; j < roomWidth; j++)
            {
                Instantiate(tile, new UnityEngine.Vector3(generationX,generationY,0), transform.rotation);

                // Wall generation
                if(j == 0){
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
                else if (j == roomWidth-1)
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
                }
                else if (i == roomLength-1)
                {
                    spotCheck = Physics2D.OverlapPoint(new UnityEngine.Vector2(generationX,generationY-1.5f));
                    if(spotCheck == null){
                        Instantiate(wall, new UnityEngine.Vector3(generationX,generationY-1.5f,0), transform.rotation);
                    }
                }
        
                tileCount++;
                if (tileCount == playerPlace)
                {
                    Instantiate(player, new UnityEngine.Vector3(generationX,generationY,0), transform.rotation);
                }
                generationX += 1.5f;
            }
            // Moves down one row and back to the first column.
            generationX = xPos;
            generationY += -1.5f;
        }
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
