using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingRectangle
{
    int width;
    int height;
    public Vector3Int bottomLeftPos;
    public Vector3Int topRightPos;
    public BoundingRectangle child1;
    public BoundingRectangle child2;
    public Room containedRoom;
    //public RoomLayout possibleLayout;
    //For each exit, keep track of 1) the location it exits to, 2) the room it exits to
    public Dictionary<Vector3Int,Tuple<Vector3Int,BoundingRectangle>> exits;

    public BoundingRectangle(Vector3Int bottomLeft, Vector3Int topRight)
    {
        bottomLeftPos = bottomLeft;
        topRightPos = topRight;
        width = topRightPos.x - bottomLeftPos.x;
        height = topRightPos.y - bottomLeftPos.y;
        child1 = null;
        child2 = null;
        containedRoom = null;
        exits = new Dictionary<Vector3Int, Tuple<Vector3Int,BoundingRectangle>>();
    }
    public void SetExits(Dictionary<Vector3Int,Tuple<Vector3Int,BoundingRectangle>> exitLocations)
    {
        exits = exitLocations;
    }
    //Whether the rectangle can be split further
    public bool split(RoomLayout[] validRooms, int minRoomX, int minRoomY)
    {
        //Partial credit to 
        //https://gamedevelopment.tutsplus.com/tutorials/how-to-use-bsp-trees-to-generate-game-maps--gamedev-12268
        bool splitVertically;
        splitVertically = UnityEngine.Random.Range(0, 1) > 0.5 ? true : false;
        if (width > height && width / height >= 1.25)
        {
            splitVertically = false;
        }
        else if(height > width && height / width >= 1.25)
        {
            splitVertically = true;
        }

        //maximum vertical or horizontal position to split at
        int max = splitVertically ? height - minRoomY : width - minRoomX;
        List<RoomLayout> fitRooms = new List<RoomLayout>();
        foreach (var layout in validRooms)
        {
            //Debug.Log("SplitVertically?: " + splitVertically + ", layout.height: " + layout.height + ", layout.width" + layout.width + ", max: " + max);
            if ((splitVertically && layout.height <= max) || (!splitVertically && layout.width <= max))
            {
                fitRooms.Add(layout);
            }
        }
        int roomIndex = UnityEngine.Random.Range(0, fitRooms.Count);
        if ((!splitVertically && max < minRoomX) || (splitVertically && max < minRoomY) || UnityEngine.Random.Range(1,5) == 4)
        {

            Debug.Log(fitRooms.Count);
            //containedRoom = fitRooms.Count > 0 ? new Room(bottomLeftPos, bottomLeftPos + new Vector3Int(fitRooms[roomIndex].width, fitRooms[roomIndex].height, 0)) : new Room(bottomLeftPos,topRightPos);//topRightPos);
            //containedRoom.layout = fitRooms.Count == 0 ? GameController.instance.defaultLayout : fitRooms[roomIndex];
            return false;
        }


        int split = UnityEngine.Random.Range(splitVertically ? minRoomY : minRoomX, max);//fitRooms[splitIndex].height : fitRooms[splitIndex].width;
        if (splitVertically)
        {
            int corridorX = UnityEngine.Random.Range(bottomLeftPos.x + 1, topRightPos.x - 1);

            //bottom half
            child1 = new BoundingRectangle(bottomLeftPos, new Vector3Int(topRightPos.x, bottomLeftPos.y + split, 0));
            //child1.possibleLayout = fitRooms[splitIndex];
            //top half
            child2 = new BoundingRectangle(new Vector3Int(bottomLeftPos.x, bottomLeftPos.y + split + 1, 0), topRightPos);
            Dictionary<Vector3Int,Tuple<Vector3Int, BoundingRectangle>> bottomHalfExits = new Dictionary<Vector3Int, Tuple<Vector3Int,BoundingRectangle>>();
            addValidExits(bottomHalfExits, child1);
            bottomHalfExits.Add(new Vector3Int(corridorX, bottomLeftPos.y + split - 1, 0), new Tuple<Vector3Int,BoundingRectangle>(new Vector3Int(corridorX, bottomLeftPos.y + split + 3, 0),child2));
            bottomHalfExits.Add(new Vector3Int(corridorX, bottomLeftPos.y + split, 0), new Tuple<Vector3Int, BoundingRectangle>(new Vector3Int(corridorX, bottomLeftPos.y + split + 3, 0),child2));
            child1.SetExits(bottomHalfExits);
            Dictionary<Vector3Int, Tuple<Vector3Int, BoundingRectangle>> topHalfExits = new Dictionary<Vector3Int, Tuple<Vector3Int, BoundingRectangle>>();
            addValidExits(topHalfExits, child2);
            topHalfExits.Add(new Vector3Int(corridorX, bottomLeftPos.y + split + 1, 0), new Tuple<Vector3Int, BoundingRectangle>(new Vector3Int(corridorX, bottomLeftPos.y + split - 3, 0),child1));
            child2.SetExits(topHalfExits);
        }
        else
        {
            int corridorY = UnityEngine.Random.Range(bottomLeftPos.y + 1, topRightPos.y - 1);
            //left half
            child1 = new BoundingRectangle(bottomLeftPos, new Vector3Int(bottomLeftPos.x + split, topRightPos.y, 0));
            //child1.possibleLayout = fitRooms[splitIndex];
            //right half
            child2 = new BoundingRectangle(new Vector3Int(bottomLeftPos.x + split + 1, bottomLeftPos.y, 0), topRightPos);
            Dictionary<Vector3Int,Tuple<Vector3Int, BoundingRectangle>> leftHalfExits = new Dictionary<Vector3Int, Tuple<Vector3Int, BoundingRectangle>>();
            addValidExits(leftHalfExits, child1);
            leftHalfExits.Add(new Vector3Int(bottomLeftPos.x + split-1, corridorY, 0), new Tuple<Vector3Int, BoundingRectangle>(new Vector3Int(bottomLeftPos.x + split + 3, corridorY, 0), child2));
            leftHalfExits.Add(new Vector3Int(bottomLeftPos.x + split, corridorY, 0), new Tuple<Vector3Int, BoundingRectangle>(new Vector3Int(bottomLeftPos.x + split + 3, corridorY, 0),child2));
            child1.SetExits(leftHalfExits);


            Dictionary<Vector3Int, Tuple<Vector3Int, BoundingRectangle>> rightHalfExits = new Dictionary<Vector3Int, Tuple<Vector3Int, BoundingRectangle>>();
            addValidExits(rightHalfExits, child2);
            rightHalfExits.Add(new Vector3Int(bottomLeftPos.x + split+1, corridorY, 0), new Tuple<Vector3Int, BoundingRectangle>(new Vector3Int(bottomLeftPos.x + split - 3, corridorY, 0), child1));
            child2.SetExits(rightHalfExits);
        }
        return true;
    }
    void addValidExits(Dictionary<Vector3Int, Tuple<Vector3Int, BoundingRectangle>> destDict, BoundingRectangle child)
    {
        foreach (var exit in exits)
        {
            if (child.containsLoc(exit.Key))
            {
                destDict.Add(exit.Key, exit.Value);
            }
        }
    }
    public bool containsLoc(Vector3Int loc)
    {
        //Should not be in the corner of or outside of the square
        return (loc.x >= bottomLeftPos.x && loc.x <= topRightPos.x && loc.y >= bottomLeftPos.y && loc.y <= topRightPos.y) 
            && !(loc.x == bottomLeftPos.x && loc.y == bottomLeftPos.y)
            && !(loc.x == bottomLeftPos.x && loc.y == topRightPos.y)
            && !(loc.x == topRightPos.x && loc.y == bottomLeftPos.y)
            && !(loc.x == topRightPos.x && loc.y == topRightPos.y);
    }
}
