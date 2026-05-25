using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorOpening : MonoBehaviour
{
    public TileVacancy tileVac;
    public SpriteRenderer openDoor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Switches door textures from open to closed and vice versa while changing its occupant accordingly.
    public void useDoor()
    {
        // Closes door if open and unnoccupied.
        if (tileVac.occupant == null)
        {
            tileVac.occupant = this.gameObject;
            this.gameObject.tag = "Wall";
            openDoor.enabled = true;

        } else if(tileVac.occupant == this.gameObject) // Opens door if closed
        {
            tileVac.occupant = null;
            this.gameObject.tag = "Untagged";
            openDoor.enabled = false;
        }
    }
}
