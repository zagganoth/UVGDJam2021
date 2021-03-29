using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour
{
    public Tilemap collidableTilemap;
    public Tilemap walkableTilemap;
    [SerializeField]
    TileBase lockedCorridorTile;
    [SerializeField]
    TileBase unlockedCorridorTile;
    [SerializeField]
    GameObject colliderPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void GenerateRoom(Room room, TileBase floorTile, TileBase wallTile)
    {
        GenerateRoomBasics(room, floorTile, wallTile);
        GenerateLockedExits(room, floorTile, wallTile);
        GenerateSpawnables(room);
    }
    void GenerateRoomBasics(Room room, TileBase floorTile, TileBase wallTile)
    {
        int bottomY = room.bottomLeftCorner.y;
        int bottomX = room.bottomLeftCorner.x;
        int topY = room.topRightCorner.y;
        int topX = room.topRightCorner.x;

        for(int i = bottomX; i < topX; i++)
        {
            for (int j = bottomY; j < topY; j++)
            {
                if(i == bottomX || j == bottomY || i == topX-1 || j == topY-1)
                {
                    collidableTilemap.SetTile(new Vector3Int(i, j, 0), wallTile);
                }
                else
                {
                    walkableTilemap.SetTile(new Vector3Int(i, j, 0), floorTile);
                }
            }
        }

    }
    public void ClearRoom(Room room)
    {
        int bottomY = room.bottomLeftCorner.y;
        int bottomX = room.bottomLeftCorner.x;
        int topY = room.topRightCorner.y;
        int topX = room.topRightCorner.x;

        for (int i = bottomX; i < topX; i++)
        {
            for (int j = bottomY; j < topY; j++)
            {
                if (i == bottomX || j == bottomY || i == topX - 1 || j == topY - 1)
                {
                    collidableTilemap.SetTile(new Vector3Int(i, j, 0), null);
                }
                else
                {
                    walkableTilemap.SetTile(new Vector3Int(i, j, 0), null);
                }
            }
        }
        if (room.exitLocations != null)
        {
            foreach (var location in room.exitLocations)
            {
                collidableTilemap.SetTile(location.Key, null);
                walkableTilemap.SetTile(location.Key, null);

            }
        }
    }
    void GenerateLockedExits(Room room, TileBase floorTile, TileBase wallTile)
    {
        if (room.exitLocations != null)
        {
            foreach (var location in room.exitLocations)
            {
                collidableTilemap.SetTile(location.Key, lockedCorridorTile);
                //walkableTilemap.SetTile(location.Key, corridorTile);

            }
        }
    }
    public void unlockExits(Room room)
    {
        if(room.exitLocations != null)
        {
            foreach(var location in room.exitLocations)
            {
                collidableTilemap.SetTile(location.Key, null);
                walkableTilemap.SetTile(location.Key, unlockedCorridorTile);
                GameObject transition = Instantiate(colliderPrefab, new Vector3(location.Key.x + 0.5f, location.Key.y + 0.5f, location.Key.z), Quaternion.identity);
                transition.transform.SetParent(GameController.instance.colliderParent.transform);
                RoomTransition rt = transition.GetComponent<RoomTransition>();
                rt.roomTo = location.Value.Item2;
                rt.pos = location.Value.Item1;
            }
        }
    }
    void GenerateSpawnables(Room room)
    {
        if (room.layout.specialLocations != null && room.layout.specialLocations.Length > 0)
        {
            foreach (var locSpawn in room.layout.specialLocations)
            {
                Spawnable spawnable = Instantiate(locSpawn.spawnable, room.bottomLeftCorner + locSpawn.location + new Vector3(0.5f, 0.5f, -1), Quaternion.identity);
                spawnable.transform.SetParent(GameController.instance.spawnableParent.transform);
                spawnable.setRoom(room);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
