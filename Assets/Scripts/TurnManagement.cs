using UnityEngine;

public class TurnManagement : MonoBehaviour
{
    public int turnCount = 0;
    public int actionCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnityEngine.Debug.Log("Turn " + turnCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
