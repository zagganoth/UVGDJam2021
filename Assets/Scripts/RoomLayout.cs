using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New RoomLayout", menuName = "ScriptableObjects/RoomLayout")]
public class RoomLayout : ScriptableObject
{
    [Serializable]
    public struct SpawnLocation
    {
        public Vector3Int location;
        public Spawnable spawnable;
    }
    public SpawnLocation[] specialLocations;
    public int width;
    public int height;
    [SerializeField]
    public int enemyCount;
    public RoomLayout(int w, int h)
    {
        width = w;
        height = h;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
