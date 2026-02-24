using System;
using UnityEngine;

public class levelGeneration : MonoBehaviour
{
    public GameObject tile;
    public GameObject player;
    public float tileNum = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Generates tiles in a perfect square based on the square root assigned with tileNum. Also randomly spawns the player on one of these tiles.
        float generationX = -tileNum * 0.5f;
        float generationY = tileNum * 0.5f;
        int playerPlace = (int)UnityEngine.Random.Range(1, tileNum*tileNum);
        int tileCount = 0;
        for (int i = 0; i < tileNum; i++)
        {
            for (int j = 0; j < tileNum; j++)
            {
                Instantiate(tile, new Vector3(generationX,generationY,0), transform.rotation);
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
