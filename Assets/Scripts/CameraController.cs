using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    GameController gameStance;
    [SerializeField]
    Vector2 posOffset;
    [SerializeField]
    float timeOffset;
    [SerializeField]
    Vector2 maxOffsetsX;
    [SerializeField]
    Vector2 maxOffsetsY;
    // Start is called before the first frame update
    void Start()
    {
        gameStance = GameController.instance;
    }

    // Update is called once per frame
    void Update()
    {
        //With some help from https://www.youtube.com/watch?v=05VX2N9_2_4 to clamp camera to surrounding room
        transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x + posOffset.x, player.transform.position.y + posOffset.y, -10), timeOffset * Time.deltaTime);
        if(gameStance.currentRoom != null)
        {
            transform.position = new Vector3
            (
                Mathf.Clamp(transform.position.x, gameStance.currentRoom.bottomLeftCorner.x + maxOffsetsX.x, gameStance.currentRoom.topRightCorner.x - maxOffsetsX.y),
                Mathf.Clamp(transform.position.y, gameStance.currentRoom.bottomLeftCorner.y + maxOffsetsY.x, gameStance.currentRoom.topRightCorner.y - maxOffsetsY.y),
                transform.position.z
            );
        }
    }
}
