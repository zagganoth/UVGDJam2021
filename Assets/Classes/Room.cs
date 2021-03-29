using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room
{
    public Vector3Int bottomLeftCorner;
    public Vector3Int topRightCorner;
    public Dictionary<Vector3Int,System.Tuple<Vector3Int, Room>> exitLocations;
    public RoomLayout layout;
    public Room(Vector3Int bottomLeft, Vector3Int topRight, RoomLayout lay)
    {
        bottomLeftCorner = bottomLeft;
        topRightCorner = topRight;
        layout = lay;
        exitLocations = new Dictionary<Vector3Int, System.Tuple<Vector3Int, Room>>();
    }
    public void AddExit(Vector3Int exitLocation, Vector3Int exitToLocation, Room exitToRoom)
    {
        exitLocations.Add(exitLocation, new System.Tuple<Vector3Int, Room>(exitToLocation, exitToRoom));
    }

}
