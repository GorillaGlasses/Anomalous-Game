using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int sightRadius = 16;
    public TurnManagement turnManager;
    public Rigidbody2D enemyBody;
    public UnityEngine.Vector2 targetLocation;
    public List<UnityEngine.Vector2> adjacentTiles = new List<UnityEngine.Vector2>();
    public bool takenTurn = false;
    public StatBlock enemyStats;
    void Start()
    {
        turnManager = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManagement>();
        targetLocation = enemyBody.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!turnManager.playerTurn && !takenTurn)
        {
            targetLocation = checkRadius();
            if (targetLocation != enemyBody.position)
            {
                checkDestinationEmpty(pathToTarget());
            }
            else
            {
                turnManager.finishedEnemies++;
                takenTurn = true;
            }
        }

        if (turnManager.playerTurn)
        {
            takenTurn = false;
        }
    }

    //Finds the next best tile to move to using the A* pathfinding algorithm, and returns the position of that tile. If no adjacent tiles are open, returns the current position of the enemy.
    UnityEngine.Vector2 pathToTarget()
    {
        float gCost = 0;
        float hCost = math.distance(enemyBody.position, targetLocation);
        float fCost = gCost + hCost;
        bool firstTile = true;
        int debugCounter = 0;
        checkAdjacentTiles(enemyBody.position);
        if (adjacentTiles.Count > 0)
        {
            UnityEngine.Vector2 bestTile = enemyBody.position;
            foreach (UnityEngine.Vector2 tile in adjacentTiles)
            {  
                debugCounter++;
                gCost = math.distance(enemyBody.position, tile);
                hCost = math.distance(tile, targetLocation);
                float tileFCost = gCost + hCost;
                if (tileFCost <= fCost || firstTile)
                {
                    bestTile = tile;
                    fCost = tileFCost;
                    firstTile = false;
                }
            }
            adjacentTiles.Clear();
            return bestTile;
        }

        return enemyBody.position;
    }

    // Checks all tiles adjacent to the enemy for vacancy, adding their position to the list of adjacent tiles if they are open. This is used in the pathfinding algorithm to determine which tiles the enemy can move to.
    void checkAdjacentTiles(UnityEngine.Vector2 startingPoint)
    {
        Collider2D spotCheck = Physics2D.OverlapPoint(new Vector2(startingPoint.x, startingPoint.y+1.5f));
        if ((spotCheck.gameObject.TryGetComponent<TileVacancy>(out TileVacancy tVac) && tVac.occupant == null) || spotCheck.gameObject.TryGetComponent<StatBlock>(out StatBlock player)) // Checks the tile directly above the enemy, and if it is unoccupied, adds it to the list of adjacent tiles.
        {
            adjacentTiles.Add(new UnityEngine.Vector2(startingPoint.x, startingPoint.y+1.5f));
        }
        spotCheck = Physics2D.OverlapPoint(new Vector2(startingPoint.x-1.5f, startingPoint.y));
        if ((spotCheck.gameObject.TryGetComponent<TileVacancy>(out tVac) && tVac.occupant == null) || spotCheck.gameObject.TryGetComponent<StatBlock>(out player)) // Checks the tile directly to the left of the enemy, and if it is unoccupied, adds it to the list of adjacent tiles.
        {
            adjacentTiles.Add(new UnityEngine.Vector2(startingPoint.x-1.5f, startingPoint.y));
        }
        spotCheck = Physics2D.OverlapPoint(new Vector2(startingPoint.x+1.5f, startingPoint.y));
        if ((spotCheck.gameObject.TryGetComponent<TileVacancy>(out tVac) && tVac.occupant == null) || spotCheck.gameObject.TryGetComponent<StatBlock>(out player)) // Checks the tile directly to the right of the enemy, and if it is unoccupied, adds it to the list of adjacent tiles.
        {
            adjacentTiles.Add(new UnityEngine.Vector2(startingPoint.x+1.5f, startingPoint.y));
        }
        spotCheck = Physics2D.OverlapPoint(new Vector2(startingPoint.x, startingPoint.y-1.5f));
        if ((spotCheck.gameObject.TryGetComponent<TileVacancy>(out tVac) && tVac.occupant == null) || spotCheck.gameObject.TryGetComponent<StatBlock>(out player)) // Checks the tile directly below the enemy, and if it is unoccupied, adds it to the list of adjacent tiles.
        {
            adjacentTiles.Add(new UnityEngine.Vector2(startingPoint.x, startingPoint.y-1.5f));
        }
    }

    // Checks fora player within the sight radius of the enemy. Returns the position of the closest player if found, and the previous target location if not.
    UnityEngine.Vector2 checkRadius()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(enemyBody.position, sightRadius);
        UnityEngine.Vector2 closestPlayer = new UnityEngine.Vector2(math.INFINITY, math.INFINITY);
        foreach (Collider2D collider in hitColliders) {
            if (collider.gameObject.CompareTag("Player"))
            {
                if(closestPlayer.x >= gameObject.GetComponent<Rigidbody2D>().position.x && closestPlayer.y >= gameObject.GetComponent<Rigidbody2D>().position.y){
                    closestPlayer = collider.gameObject.GetComponent<Rigidbody2D>().position;
                }
            }
        }
        if (closestPlayer.x < math.INFINITY && closestPlayer.y < math.INFINITY)
        {
            return closestPlayer;
        }
        return targetLocation;
    }

    //Checks if the spot the enemy is about to move to is an unoccupied tile. Moves enemy to the tile if it is open, and does nothing if not.
    void checkDestinationEmpty(UnityEngine.Vector2 direction)
    {
        UnityEngine.Vector2 targetSpot = direction; 
        Collider2D spotCheck = Physics2D.OverlapPoint(targetSpot);

        if (spotCheck.gameObject.TryGetComponent<TileVacancy>(out TileVacancy tVac) && tVac.occupant == null)
        {
            enemyBody.position = targetSpot;
            turnManager.finishedEnemies++;
            takenTurn = true;
        }
        else if(spotCheck.gameObject.TryGetComponent<StatBlock>(out StatBlock player) && player.isPlayer){
            if (UnityEngine.Random.Range(1, 21) + enemyStats.strength >= player.defense)
            {
                int damage = UnityEngine.Random.Range(1, 5) + enemyStats.strength;
                player.health -= damage;
                enemyStats.combatLog.text += enemyStats.charName + " hits " + player.charName + " for " + damage + " damage!\n";
                turnManager.finishedEnemies++;
                takenTurn = true;
            } else {
                enemyStats.combatLog.text += enemyStats.charName + " misses " + player.charName + "!\n";
                turnManager.finishedEnemies++;
                takenTurn = true;
            }
        } else {
            turnManager.finishedEnemies++;
            takenTurn = true;
        }
        return;
    }
}
