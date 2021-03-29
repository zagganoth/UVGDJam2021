using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName ="New FloorLayout",menuName ="ScriptableObjects/FloorLayout")]
public class FloorLayout : ScriptableObject
{
    public int width;
    public int height;
    public RoomLayout[] validRooms;
    public RoomLayout entranceRoom;
    public RoomLayout bossRoom;
    public int maxNumRooms;
    public int baseRoomX;
    public int baseRoomY;
    public TileBase wallTile;
    public TileBase floorTile;
    /*
    public void init()
    {
        int minX = int.MaxValue;
        int minY = int.MaxValue;
        foreach(RoomLayout layout in validRooms)
        {
            if(layout.width < minX)
            {
                minX = layout.width;
            }
            if(layout.height < minY)
            {
                minY = layout.height;
            }
        }
        minRoomX = minX;
        minRoomY = minY;
    } */
    public RoomLayout getRandomRoomLayout()
    {
        return validRooms[Random.Range(0, validRooms.Length)];
    }
}
