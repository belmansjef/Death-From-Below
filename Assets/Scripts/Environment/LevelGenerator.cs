using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Direction
{
    up,
    right,
    down,
    left
};

public class LevelGenerator : MonoBehaviour
{
    [Header("Rooms")]
    public GameObject layoutRoom;
    public Color startColor;
    public Color endColor;
    public Color shopColor;
    public Color gunRoomColor;

    [Header("Generation")]
    public LayerMask roomLayer;
    public int numberOfRooms;
    public Transform generationPoint;
    public Direction selectedDirection;
    public RoomCenter centerStart;
    public RoomCenter centerEnd;
    public RoomCenter centerShop;
    public RoomCenter centerGunRoom;
    public RoomCenter[] potentialCenters;
    public RoomPrefabs rooms;

    [Header("Shop")]
    public bool includeShop;
    public int minDistanceShopRoom, maxDistanceShopRoom;

    [Header("Gun Room")]
    public bool inculdeGunRoom;
    public int minDistanceGunRoom, maxDistanceGunRoom;

    // Private variables
    private float xOffset = 18f;
    private float yOffset = 10f;
    private GameObject endRoom, shopRoom, gunRoom;    
    private List<GameObject> layoutRoomObjects = new List<GameObject>();
    private List<GameObject> generatedOutlines = new List<GameObject>();


    private void Start()
    {
        Instantiate(layoutRoom, generationPoint.position, Quaternion.identity).GetComponent<SpriteRenderer>().color = startColor;

        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();

        for (int i = 0; i < numberOfRooms; i++)
        {           
            GameObject newRoom = Instantiate(layoutRoom, generationPoint.position, Quaternion.identity);

            layoutRoomObjects.Add(newRoom);

            if(i + 1 == numberOfRooms)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                endRoom = newRoom;
            }

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            while (Physics2D.OverlapCircle(generationPoint.position, .2f, roomLayer))
            {
                MoveGenerationPoint();
            }
        }

        // Generate shop
        if (includeShop)
        {
            int shopSelector = Random.Range(minDistanceShopRoom, maxDistanceShopRoom + 1);
            shopRoom = layoutRoomObjects[shopSelector];
            layoutRoomObjects.RemoveAt(shopSelector);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
        }
    
        // Generate gunroom
        if (inculdeGunRoom)
        {
            int grSelector = Random.Range(minDistanceGunRoom, maxDistanceGunRoom + 1);
            gunRoom = layoutRoomObjects[grSelector];
            layoutRoomObjects.RemoveAt(grSelector);
            gunRoom.GetComponent<SpriteRenderer>().color = gunRoomColor;
        }

        // Create room outline
        CreateRoomOutline(Vector3.zero);
        foreach (GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }

        CreateRoomOutline(endRoom.transform.position);
        if (includeShop) CreateRoomOutline(shopRoom.transform.position);
        if (includeShop) CreateRoomOutline(gunRoom.transform.position);

        foreach (GameObject outline in generatedOutlines)
        {
            bool generateCenter = true;

            if(outline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }

            if(outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }

            if (includeShop)
            {
                if (outline.transform.position == shopRoom.transform.position)
                {
                    Instantiate(centerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
            }

            if(inculdeGunRoom)
            {
                if (outline.transform.position == gunRoom.transform.position)
                {
                    Instantiate(centerGunRoom, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
            }
                
            if (generateCenter)
            {
                int centerSelect = Random.Range(0, potentialCenters.Length);
                Instantiate(potentialCenters[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }            
        }
    }

    private void Update()
    {
    #if UNITY_EDITOR
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    #endif
    }

    public void MoveGenerationPoint()
    {
        switch (selectedDirection)
        {
            case Direction.up:
                generationPoint.position += new Vector3(0f, yOffset, 0f);
                break;
            case Direction.down:
                generationPoint.position += new Vector3(0f, -yOffset, 0f);
                break;
            case Direction.right:
                generationPoint.position += new Vector3(xOffset, 0f, 0f);
                break;
            case Direction.left:
                generationPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
    }

    public void CreateRoomOutline(Vector3 roomPos)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPos + new Vector3(0f, yOffset, 0f), .2f, roomLayer);
        bool roomBelow = Physics2D.OverlapCircle(roomPos + new Vector3(0f, -yOffset, 0f), .2f, roomLayer);
        bool roomRight = Physics2D.OverlapCircle(roomPos + new Vector3(xOffset, 0f, 0f), .2f, roomLayer);
        bool roomLeft = Physics2D.OverlapCircle(roomPos + new Vector3(-xOffset, 0f, 0f), .2f, roomLayer);

        int directionCount = 0;
        if (roomAbove)
        {
            directionCount++;
        }
        if (roomBelow)
        {
            directionCount++;
        }
        if (roomRight)
        {
            directionCount++;
        }
        if (roomLeft)
        {
            directionCount++;
        }

        switch (directionCount)
        {
            case 0:
                Debug.LogError("Found no room exists!!");
                break;
            case 1:
                if (roomAbove) generatedOutlines.Add(Instantiate(rooms.singleUp, roomPos, Quaternion.identity));
                if (roomBelow) generatedOutlines.Add(Instantiate(rooms.singleDown, roomPos, Quaternion.identity));
                if (roomRight) generatedOutlines.Add(Instantiate(rooms.singleRight, roomPos, Quaternion.identity));
                if (roomLeft) generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPos, Quaternion.identity));
                break;
            case 2:
                if (roomAbove && roomBelow) generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPos, Quaternion.identity));
                if (roomAbove && roomRight) generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPos, Quaternion.identity));
                if (roomAbove && roomLeft) generatedOutlines.Add(Instantiate(rooms.doubleLeftUp, roomPos, Quaternion.identity));
                if (roomBelow && roomRight) generatedOutlines.Add(Instantiate(rooms.doubleRightDown, roomPos, Quaternion.identity));
                if (roomBelow && roomLeft) generatedOutlines.Add(Instantiate(rooms.doubleDownLeft, roomPos, Quaternion.identity));
                if (roomRight && roomLeft) generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPos, Quaternion.identity));
                break;
            case 3:
                if (roomAbove && roomBelow && roomRight) generatedOutlines.Add(Instantiate(rooms.tripleUpRightDown, roomPos, Quaternion.identity));
                if (roomAbove && roomBelow && roomLeft) generatedOutlines.Add(Instantiate(rooms.tripleDownLeftUp, roomPos, Quaternion.identity));
                if (roomAbove && roomRight && roomLeft) generatedOutlines.Add(Instantiate(rooms.tripleLeftRightUp, roomPos, Quaternion.identity));
                if (roomBelow && roomRight && roomLeft ) generatedOutlines.Add(Instantiate(rooms.tripleRightDownLeft, roomPos, Quaternion.identity));
                break;
            case 4:
                if (roomAbove && roomBelow && roomRight && roomLeft) generatedOutlines.Add(Instantiate(rooms.fourway, roomPos, Quaternion.identity));
                break;
        }
    }
}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleUp, singleDown, singleRight, singleLeft,
        doubleUpDown, doubleLeftRight, doubleUpRight, doubleRightDown, doubleLeftUp, doubleDownLeft,
        tripleUpRightDown, tripleRightDownLeft, tripleDownLeftUp, tripleLeftRightUp,
        fourway;
}
