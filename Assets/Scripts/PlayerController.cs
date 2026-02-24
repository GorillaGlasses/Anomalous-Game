using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Basic Movement Code
        if(Input.GetKeyDown(KeyCode.W)){
            transform.position = new Vector3(transform.position.x, transform.position.y + moveSpeed, transform.position.z);
        }

        if(Input.GetKeyDown(KeyCode.A)){
            transform.position = new Vector3(transform.position.x - moveSpeed, transform.position.y, transform.position.z);
        }

        if(Input.GetKeyDown(KeyCode.S)){
            transform.position = new Vector3(transform.position.x, transform.position.y - moveSpeed, transform.position.z);
        }

        if(Input.GetKeyDown(KeyCode.D)){
            transform.position = new Vector3(transform.position.x + moveSpeed, transform.position.y, transform.position.z);
        }
    }
}
