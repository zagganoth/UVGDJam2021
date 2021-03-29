using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public Room currentRoom;
    [SerializeField]
    public GameObject player;
    [SerializeField]
    public RoomLayout defaultLayout;
    [SerializeField]
    public GameObject colliderParent;
    [SerializeField]
    public GameObject spawnableParent;
    [SerializeField]
    public GameObject ladderPrefab;
    public int enemiesToClear;
    GameObject ladder;
    public int level;
    [SerializeField]
    public float baseHealth;
    [SerializeField]
    float health;
    [SerializeField]
    TMP_Text levelText;
    [SerializeField]
    RectTransform healthPanel;
    [SerializeField]
    float damageCooldown;
    bool damageActive;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        level = 0;
        health = baseHealth;
        instance = this;
        damageActive = false;
        levelText.text = "Level: 1";
    }

    public void setRoom(Room room, bool teleport)
    {
        currentRoom = room;
        if(teleport)
        {
            player.transform.position = new Vector3(room.bottomLeftCorner.x + Mathf.FloorToInt(room.topRightCorner.x - room.bottomLeftCorner.x)/2,
                                                    room.bottomLeftCorner.y + Mathf.FloorToInt(room.topRightCorner.y - room.bottomLeftCorner.y) / 2,-1);
            GetComponent<RoomGenerator>().unlockExits(room);
        }

    }
    public void transitionToRoom(Room room, Vector3Int entrancePos)
    {
        currentRoom = room;
        player.transform.position = entrancePos;
        enemiesToClear = room.layout.enemyCount;
    }
    public void addEnemiesToRoom(int enemies)
    {
        enemiesToClear += enemies;
    }
    public void spawnLadder(Room room)
    {
        Vector3 pos = new Vector3(room.bottomLeftCorner.x + Mathf.FloorToInt(room.topRightCorner.x - room.bottomLeftCorner.x) / 2,
                                                    room.bottomLeftCorner.y + Mathf.FloorToInt(room.topRightCorner.y - room.bottomLeftCorner.y) / 2, -1);
        ladder = Instantiate(ladderPrefab, pos, Quaternion.identity);
    }
    public void takeDamage(float damage)
    {
        if(damageActive)
        {
            return;
        }
        health -= damage;
        healthPanel.localScale = new Vector3(health/baseHealth, transform.localScale.y);
        if (health <= 0)
        {
            changeLevel(true);
            level = 0;
        } else
        {
            damageActive = true;
            player.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
            StartCoroutine(takeDamageCooldown());
        }

    }
    private IEnumerator takeDamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        damageActive = false;
        player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }
    public bool canAddHealth()
    {
        return health < baseHealth;
    }
    public void addHealth(float h)
    {
        health += h;
        if(health > baseHealth)
        {
            health = baseHealth;
        }
        healthPanel.localScale = new Vector3(health / baseHealth, transform.localScale.y);
    }
    public void changeLevel(bool reset)
    {
        foreach(Transform child in colliderParent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in spawnableParent.transform)
        {
            Destroy(child.gameObject);
        }
        if (!reset)
        {
            level += 1;
            levelText.text = "Level: " + (level + 1);
            Destroy(ladder.gameObject);
        }
        else
        {
            level = 0;
            health = baseHealth;
            levelText.text = "Level: " + (level + 1);
            healthPanel.localScale = new Vector3(1,healthPanel.localScale.y);
        }

        GetComponent<FloorGenerator>().GenerateFloor();
    }
    public void unlockExits()
    {
        GetComponent<RoomGenerator>().unlockExits(currentRoom);
    }
    public void enemyKilled()
    {
        enemiesToClear--;
        if(enemiesToClear <= 0)
        {
            unlockExits();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
