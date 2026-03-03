using System;
using UnityEngine;

public class levelGeneration : MonoBehaviour
{
    public GameObject tile;
    public GameObject wall;
    public GameObject player;
    public float tileNum = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Generates tiles in a perfect square based on the square root assigned with tileNum. The outermost tiles are all brick walls instead. 
        // Also randomly spawns the player on one of these tiles.
        float generationX = -tileNum * 0.5f;
        float generationY = tileNum * 0.5f;
        int playerPlace = (int)UnityEngine.Random.Range(tileNum+1, (tileNum*tileNum)-tileNum);
        int tileCount = 0;
        for (int i = 0; i < tileNum; i++)
        {
            for (int j = 0; j < tileNum; j++)
            {
                if(i < tileNum-1 && i > 0 && j < tileNum-1 && j > 0){
                    // Generates empty tile when near the center of the room
                    Instantiate(tile, new Vector3(generationX,generationY,0), transform.rotation);
                } else {
                    // Generates wall near the outskirts of the room
                    Instantiate(wall, new Vector3(generationX,generationY,0), transform.rotation);
                }
                tileCount++;
                if (tileCount == playerPlace)
                {
                    Instantiate(player, new Vector3(generationX,generationY,0), transform.rotation);
                }
                generationX += 1.5f;
            }
            generationY += -1.5f;
            generationX = -tileNum * 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
