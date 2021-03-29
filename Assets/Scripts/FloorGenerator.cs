using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorGenerator : MonoBehaviour
{
    [SerializeField]
    FloorLayout curLayout;
    [SerializeField]
    Tilemap tilemap;
    List<Room> rooms;

    RoomGenerator rGenerator;
    // Start is called before the first frame update
    void Start()
    {
        rGenerator = GetComponent<RoomGenerator>();
        //curLayout.init();
        GenerateFloor();
    }
    public void GenerateFloor()
    {
        if(rooms != null && rooms.Count > 0)
        {
            foreach(var room in rooms)
            {
                rGenerator.ClearRoom(room);
                room.exitLocations = new Dictionary<Vector3Int, Tuple<Vector3Int, Room>>();
            }
        }
        rooms = new List<Room>();
        GenerateRooms();
        bool startSet = false;
        //printRooms();
        int bossRoomIndex = UnityEngine.Random.Range(0, rooms.Count - 1);
        foreach (var room in rooms)
        {
            if (!startSet)
            {
                startSet = true;
                GameController.instance.setRoom(room, true);
            }
            rGenerator.GenerateRoom(room, curLayout.floorTile, curLayout.wallTile);
        }
    }
    void printRooms()
    {
        
        foreach(var room in rooms)
        {
            print(room.bottomLeftCorner);
            print(room.topRightCorner);
        }
    }
    enum direction
    {
        UP,
        LEFT,
        DOWN,
        RIGHT,
        INVALID
    }
    struct RoomDisc
    {
        public Vector2Int pos;
        public Room prevRoom;
    }
    void GenerateRooms()
    {
        Vector3Int bottomLeft = new Vector3Int(-Mathf.FloorToInt(curLayout.width / 2), -Mathf.FloorToInt(curLayout.height / 2), 0);
        Vector3Int topRight = new Vector3Int(Mathf.FloorToInt(curLayout.width / 2), Mathf.FloorToInt(curLayout.height / 2), 0);

        //Create the entrance on the top left of the map
        Room entrance = new Room(new Vector3Int(bottomLeft.x,topRight.y-(curLayout.entranceRoom.height*curLayout.baseRoomY),0), new Vector3Int(bottomLeft.x + (curLayout.entranceRoom.width*curLayout.baseRoomX), topRight.y, 0), curLayout.entranceRoom);
        rooms.Add(entrance);

        Room[,] roomGrid = new Room[Mathf.FloorToInt(curLayout.width / curLayout.baseRoomX), Mathf.FloorToInt(curLayout.height / curLayout.baseRoomY)];
        roomGrid[0, 0] = entrance;
        int curRoomX = 0;
        int curRoomY = 0;

        int numRooms = 1;

        Room prevRoom = entrance;
        direction[] directions = { direction.UP, direction.DOWN, direction.LEFT, direction.RIGHT };
        Stack<RoomDisc> roomsVisited = new Stack<RoomDisc>();
        roomsVisited.Push(new RoomDisc { pos = new Vector2Int(curRoomX, curRoomY), prevRoom = prevRoom });
        /*
        createRoomAtLocation(1,0,curLayout.validRooms[0],direction.RIGHT,bottomLeft,topRight);
        createRoomAtLocation(2, 0, curLayout.validRooms[0], direction.RIGHT, bottomLeft, topRight);
        createRoomAtLocation(3, 0, curLayout.validRooms[0], direction.RIGHT, bottomLeft, topRight);
        createRoomAtLocation(3, 1, curLayout.validRooms[1], direction.RIGHT, bottomLeft, topRight);
        */
        //Simulate a random walk starting from the entrance until we have reached curLayout.maxNumRooms rooms
        while(numRooms < curLayout.maxNumRooms + GameController.instance.level)
        {
            direction curDirection = chooseDirection(curRoomX, curRoomY, directions, roomGrid);

            if (curDirection == direction.INVALID)
            {
                Debug.Log("Stopping because no direction found from " + curRoomX + ", " + curRoomY);
                //Later pop up the creation stack until a valid direction is found
                if (roomsVisited.Count == 0)
                {
                    break;
                }else
                {
                    RoomDisc cur = roomsVisited.Pop();
                    curRoomX = cur.pos.x;
                    curRoomY = cur.pos.y;
                    prevRoom = cur.prevRoom;
                    continue;
                }
            }
            roomsVisited.Push(new RoomDisc { pos = new Vector2Int(curRoomX, curRoomY), prevRoom = prevRoom });
            List<RoomLayout> fitRooms = new List<RoomLayout>();
            foreach(var layout in curLayout.validRooms)
            {
                if (roomFitsInDirection(curRoomX, curRoomY, layout, curDirection, roomGrid))
                {
                    fitRooms.Add(layout);
                }
            }
            if(fitRooms.Count == 0)
            {
                Debug.Log("No rooms fit in the chosen direction... weird: " + curDirection);
                //This should never happen
                break;
            }
            RoomLayout curRoomLayout = fitRooms[UnityEngine.Random.Range(0, fitRooms.Count)];
            if (curDirection == direction.LEFT)
            {
                curRoomX -= curRoomLayout.width;
            }
            else if (curDirection == direction.RIGHT)
            {
                curRoomX += curRoomLayout.width;
            }
            else if (curDirection == direction.DOWN)
            {
                curRoomY += curRoomLayout.height;
            }
            else if (curDirection == direction.UP)
            {
                curRoomY -= curRoomLayout.height;
            }

            Room next = createRoomAtLocation(curRoomX, curRoomY, curRoomLayout, curDirection, bottomLeft, topRight, roomGrid);
            createCorridor(prevRoom, next, curDirection);
            prevRoom = next;

            numRooms += 1;
        }


        direction bossDirection;
        while((bossDirection = chooseDirection(curRoomX,curRoomY, directions, roomGrid)) == direction.INVALID && roomsVisited.Count > 0)
        {
            RoomDisc cur = roomsVisited.Pop();
            curRoomX = cur.pos.x;
            curRoomY = cur.pos.y;
            prevRoom = cur.prevRoom;
        }
        if(bossDirection == direction.INVALID)
        {
            Debug.Log("No direction for bossRoom");
            return;
        }
        if (bossDirection == direction.LEFT)
        {
            curRoomX -= curLayout.bossRoom.width;
        }
        else if (bossDirection == direction.RIGHT)
        {
            curRoomX += curLayout.bossRoom.width;
        }
        else if (bossDirection == direction.DOWN)
        {
            curRoomY += curLayout.bossRoom.height;
        }
        else if (bossDirection == direction.UP)
        {
            curRoomY -= curLayout.bossRoom.height;
        }
        Room boss = createRoomAtLocation(curRoomX, curRoomY, curLayout.bossRoom, bossDirection, bottomLeft, topRight, roomGrid);
        createCorridor(prevRoom, boss, bossDirection);
        //printRoomGrid(roomGrid);
        /*
        Room bossRoom = new Room(
            new Vector3Int(prevRoom.topRightCorner.x + cur.x, prevRoom.bottomLeftCorner.y + cur.y, 0),
            new Vector3Int(prevRoom.topRightCorner.x + cur.x + curLayout.bossRoom.width, prevRoom.bottomLeftCorner.y + curLayout.bossRoom.height, 0),
            curLayout.bossRoom
            );
        rooms.Add(bossRoom);*/
    }
    direction chooseDirection(int curRoomX, int curRoomY, direction[] directions, Room[,] roomGrid)
    {
        Stack<direction> remainingDirections = new Stack<direction>(directions.OrderBy(a => Guid.NewGuid()).ToList());
        bool validDirectionFound = false;
        direction curDirection = direction.RIGHT;
        while (remainingDirections.Count > 0 && !validDirectionFound)
        {
            curDirection = remainingDirections.Pop();
            //We've picked a random direction to create a room towards, now to actually see if a new room can fit in it
            if(validateDirection(curRoomX, curRoomY, curDirection, roomGrid))
            {
                return curDirection;
            }
        }
        return direction.INVALID;
    }
    void createCorridor(Room room1, Room room2, direction curDirection)
    {
        if (room1 == room2) return;
        //Debug.Log("Creating a corridor between room: " + room1.bottomLeftCorner + ", " + room1.topRightCorner + " and " + room2.bottomLeftCorner + ", " + room2.topRightCorner);
        if(curDirection == direction.LEFT)
        {
            Vector3Int room1Mid = new Vector3Int(room1.bottomLeftCorner.x, room1.bottomLeftCorner.y + Mathf.FloorToInt((room1.topRightCorner.y - room1.bottomLeftCorner.y) / 2), 0);
            Vector3Int room2Mid = new Vector3Int(room2.topRightCorner.x-1, room2.bottomLeftCorner.y + Mathf.FloorToInt((room2.topRightCorner.y - room2.bottomLeftCorner.y) / 2), 0);
            room1.AddExit(room1Mid, new Vector3Int(room2Mid.x - 2, room2Mid.y, room2Mid.z), room2);
            room2.AddExit(room2Mid, new Vector3Int(room1Mid.x + 2, room1Mid.y, room1Mid.z), room1);
        }
        else if (curDirection == direction.RIGHT)
        {
            Vector3Int room1Mid = new Vector3Int(room1.topRightCorner.x - 1, room1.bottomLeftCorner.y + Mathf.FloorToInt((room1.topRightCorner.y - room1.bottomLeftCorner.y) / 2), 0);
            Vector3Int room2Mid = new Vector3Int(room2.bottomLeftCorner.x, room2.bottomLeftCorner.y + Mathf.FloorToInt((room2.topRightCorner.y - room2.bottomLeftCorner.y) / 2), 0);
            room1.AddExit(room1Mid, new Vector3Int(room2Mid.x + 2, room2Mid.y, room2Mid.z), room2);
            room2.AddExit(room2Mid, new Vector3Int(room1Mid.x - 2, room1Mid.y, room1Mid.z), room1);
        }
        else if(curDirection == direction.DOWN)
        {
            Vector3Int room1Mid = new Vector3Int(room1.bottomLeftCorner.x + Mathf.FloorToInt((room1.topRightCorner.x - room1.bottomLeftCorner.x) / 2), room1.bottomLeftCorner.y, 0);
            Vector3Int room2Mid = new Vector3Int(room2.bottomLeftCorner.x + Mathf.FloorToInt((room2.topRightCorner.x - room2.bottomLeftCorner.x) / 2), room2.topRightCorner.y - 1, 0);
            room1.AddExit(room1Mid, new Vector3Int(room2Mid.x, room2Mid.y - 2, room2Mid.z), room2);
            room2.AddExit(room2Mid, new Vector3Int(room1Mid.x, room1Mid.y + 2, room1Mid.z), room1);
        }
        else if (curDirection == direction.UP)
        {
            Vector3Int room1Mid = new Vector3Int(room1.bottomLeftCorner.x + Mathf.FloorToInt((room1.topRightCorner.x - room1.bottomLeftCorner.x) / 2), room1.topRightCorner.y - 1, 0);
            Vector3Int room2Mid = new Vector3Int(room2.bottomLeftCorner.x + Mathf.FloorToInt((room2.topRightCorner.x - room2.bottomLeftCorner.x) / 2), room2.bottomLeftCorner.y, 0);
            room1.AddExit(room1Mid, new Vector3Int(room2Mid.x, room2Mid.y + 2, room2Mid.z), room2);
            room2.AddExit(room2Mid, new Vector3Int(room1Mid.x, room1Mid.y - 2, room1Mid.z), room1);
        }
    }
    void printRoomGrid(Room[,] roomGrid)
    {
        for(int y = 0; y < roomGrid.GetLength(0);y++)
        {
            for(int x = 0; x < roomGrid.GetLength(1); x++)
            {
                Debug.Log("x: " + x + ", y: " + y + ", room: " + (roomGrid[y, x] != null ? roomGrid[y,x].layout.name : "None"));
            }
        }
    }
    //validate that a direction fits at least one room
    bool validateDirection(int prevRoomX, int prevRoomY, direction direction, Room[,] roomGrid)
    {
        if(direction == direction.LEFT)
        {
            return prevRoomX - 1 > 0 && roomGrid[prevRoomY,prevRoomX-1] == null;
        }//right
        else if(direction == direction.RIGHT)
        {
            return prevRoomX + 1 < roomGrid.GetLength(1) && roomGrid[prevRoomY,prevRoomX+1] == null;
        }//down
        else if(direction == direction.DOWN)
        {
            return prevRoomY + 1 < roomGrid.GetLength(0) && roomGrid[prevRoomY+1,prevRoomX] == null;
        }//up
        else
        {
            return prevRoomY - 1 >= 0 && roomGrid[prevRoomY-1,prevRoomX] == null;
        }
    }

    bool roomFitsInDirection(int curRoomX, int curRoomY, RoomLayout layout, direction direction, Room[,] roomGrid)
    {
        if (direction == direction.LEFT)
        {
            if (curRoomX - layout.width < 0) {
                return false;
            }
            for (int i = 1; i < layout.width; i++)
            {
                if(roomGrid[curRoomX - i, curRoomY] != null)
                {
                    return false;
                }
            }
            return true;
        }//right
        else if (direction == direction.RIGHT)
        {
            if(curRoomX + layout.width >= roomGrid.GetLength(1)) {
                return false;
            }
            for(int i = 1; i < layout.width;i++)
            {
                if (roomGrid[curRoomX + i, curRoomY] != null)
                {
                    return false;
                }
            }
            return true;
        }//down
        else if (direction == direction.DOWN)
        { 
            if(curRoomY + layout.height >= roomGrid.GetLength(0))
            {
                Debug.Log("CurRoomY: " + curRoomY + ", layout.height: " + layout.height);
                return false;
            }
            for(int j = 1; j < layout.height; j++)
            {
                if (layout.width > 1)
                {
                    for (int i = 1; i < layout.width; i++)
                    {
                        if (roomGrid[curRoomX - i, curRoomY - j] != null)
                        {
                            return false;
                        }
                    }
                }
                if (roomGrid[curRoomX,curRoomY+j] != null)
                {
                    return false;
                }
            }

            return true;
        }//up
        else
        {
            if(curRoomY - layout.height < 0)
            {
                return false;
            }
            for (int j = 1; j < layout.height; j++)
            {
                if (layout.width > 1)
                {
                    for (int i = 1; i < layout.width; i++)
                    {
                        if (roomGrid[curRoomX - i, curRoomY - j] != null)
                        {
                            return false;
                        }
                    }
                }
                if (roomGrid[curRoomX, curRoomY - j] != null)
                {
                    return false;
                }
            }
            return true;

        }
    }

    Room createRoomAtLocation(int curRoomX, int curRoomY, RoomLayout layout, direction direction, Vector3Int bottomLeft, Vector3Int topRight, Room[,] roomGrid)
    {
        //Debug.Log("Creating room in direction: " + direction + ", " + layout.name);
        Room newRoom = new Room(
            //bottom Left
            new Vector3Int(
                    bottomLeft.x + ((curRoomX-layout.width+1) * curLayout.baseRoomX + curRoomX) - (layout.width-1),
                    topRight.y - (curRoomY * curLayout.baseRoomY + curRoomY)   - (curLayout.baseRoomY * layout.height),
                    0
                ),
            new Vector3Int(
                    bottomLeft.x + ((curRoomX - layout.width + 1) * curLayout.baseRoomX + curRoomX) + (curLayout.baseRoomX * layout.width),
                    topRight.y - (curRoomY * curLayout.baseRoomY + curRoomY),
                    0
                ),
            layout
            );
        rooms.Add(newRoom);
        if(direction == direction.UP)
        {
            for (int i = curRoomY; i < curRoomY + layout.height; i++)
            {
                roomGrid[i, curRoomX] = newRoom;
            }
        }
        else if(direction == direction.DOWN)
        {
            for (int i = curRoomY; i > curRoomY - layout.height; i--)
            {
                roomGrid[i, curRoomX] = newRoom;
            }
        }
        else if (direction == direction.LEFT)
        {
            for (int i = curRoomX; i < curRoomX + layout.width; i++)
            {
                roomGrid[curRoomY, i] = newRoom;
            }
        }
        else if (direction == direction.RIGHT)
        {
            for (int i = curRoomX; i > curRoomX - layout.width; i--)
            {
                roomGrid[curRoomY, i] = newRoom;
            }
        }
        return newRoom;
    }
    Room GetRoomFromDirection(Room currentRoom, RoomLayout newRoomLayout, direction direction)
    {
        return null;
    }
    /*
    void GenerateRoomsOld()
    {
        List<BoundingRectangle> roomRectangles = new List<BoundingRectangle>();
        Queue<BoundingRectangle> nodesToVisit = new Queue<BoundingRectangle>();
        Vector3Int bottomLeft = new Vector3Int(-Mathf.FloorToInt(curLayout.width / 2), -Mathf.FloorToInt(curLayout.height / 2), 0);
        Vector3Int topRight = new Vector3Int(Mathf.FloorToInt(curLayout.width / 2), Mathf.FloorToInt(curLayout.height / 2), 0);
        nodesToVisit.Enqueue(new BoundingRectangle(bottomLeft, topRight));
        while(nodesToVisit.Count > 0)
        {
            BoundingRectangle cur = nodesToVisit.Dequeue();
            if(cur.split(curLayout.validRooms,curLayout.minRoomX, curLayout.minRoomY))
            {
                nodesToVisit.Enqueue(cur.child1);
                nodesToVisit.Enqueue(cur.child2);
            }
            else if(cur.containedRoom != null)
            {
                roomRectangles.Add(cur);
            }
        }
        GenerateCorridors(roomRectangles);
    }*/
    void GenerateCorridors(List<BoundingRectangle> roomRectangles)
    {
        //roomRectangles is a list of all the boundingRectangles which contain rooms
        //So if I just 
        foreach (var roomRect in roomRectangles)
        {
            Dictionary<Vector3Int, System.Tuple<Vector3Int, Room>> exitDict = new Dictionary<Vector3Int, System.Tuple<Vector3Int, Room>>();
            foreach (var exit in roomRect.exits)
            {

                Room roomTo = null;
                if (exit.Value.Item2.containedRoom != null)
                {
                    roomTo = exit.Value.Item2.containedRoom;
                }
                else
                {
                    Queue<BoundingRectangle> rectsToExplore = new Queue<BoundingRectangle>();
                    rectsToExplore.Enqueue(exit.Value.Item2);
                    while (roomTo == null && rectsToExplore.Count > 0)
                    {
                        BoundingRectangle curRect = rectsToExplore.Dequeue();
                        if (curRect.child1 != null)
                        {
                            if (curRect.child1.containsLoc(exit.Value.Item1) && curRect.child1.containedRoom != null)
                            {
                                roomTo = curRect.child1.containedRoom;
                                break;
                            }
                            else
                            {
                                rectsToExplore.Enqueue(curRect.child1);
                            }
                        }
                        if (curRect.child2 != null)
                        {
                            if (curRect.child2.containsLoc(exit.Value.Item1) && curRect.child2.containedRoom != null)
                            {
                                roomTo = curRect.child2.containedRoom;
                                break;
                            }
                            else
                            {
                                rectsToExplore.Enqueue(curRect.child2);
                            }
                        }

                    }
                }

                if (roomTo != null)
                {
                    exitDict.Add(exit.Key, new System.Tuple<Vector3Int, Room>(exit.Value.Item1, roomTo));
                }
            }
            //roomRect.containedRoom.SetExits(exitDict);
            rooms.Add(roomRect.containedRoom);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
