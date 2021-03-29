using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    [SerializeField] 
    Room room;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void setRoom(Room r)
    {
        room = r;
    }
    public Room getRoom()
    {
        return room;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
