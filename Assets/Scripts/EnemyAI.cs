using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [System.Serializable]
    struct dropOdds
    {
        public float appearanceChance;
        public GameObject drop;
    }

    protected Spawnable spawnComponent;
    protected bool move;
    [SerializeField]
    protected float moveSpeed;
    protected SpriteRenderer sr;
    [SerializeField]
    protected float hitCooldown;
    [SerializeField]
    protected int numHits;
    protected bool hitActive;
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float moveCooldown;
    protected bool moveCooldownActive;
    [SerializeField]
    int dropCount;
    [SerializeField]
    protected float dropChance;
    [SerializeField]
    List<dropOdds> possibleDrops;
    List<dropOdds> normalizedDrops;
    List<dropOdds> finalDrops;

    // Start is called before the first frame update
    void Start()
    {
        move = false;
        moveCooldownActive = false;
        spawnComponent = GetComponent<Spawnable>();
        hitActive = false;
        sr = GetComponent<SpriteRenderer>();
        childStart();
        normalizeDropOdds();
    }
    protected virtual void normalizeDropOdds()
    {
        normalizedDrops = new List<dropOdds>();
        float oddSum = 0;
        foreach(var odd in possibleDrops)
        {
            oddSum += odd.appearanceChance;    
        }
        for(int i = 0; i < possibleDrops.Count; i++)
        {
            normalizedDrops.Add(new dropOdds { appearanceChance = (possibleDrops[i].appearanceChance / oddSum) * 50, drop = possibleDrops[i].drop });
        }
        finalDrops = new List<dropOdds>();
        for(int i = 0; i < normalizedDrops.Count; i++)
        {
            for(int j = 0; j < normalizedDrops[i].appearanceChance; j++)
            {
                finalDrops.Add(normalizedDrops[i]);
            }
        }
        Debug.Log("Possible spawnables: " + finalDrops.Count);
    }
    public virtual void childStart()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.currentRoom == spawnComponent.getRoom() && !moveCooldownActive)
        {
            move = true;
        }
        else
        {
            move = false;
        }
        if (move)
        {
            transform.position = Vector3.Lerp(transform.position, Vector3.MoveTowards(transform.position, GameController.instance.player.transform.position, moveSpeed * Time.deltaTime), 0.6f);
        }
        childUpdate();
    }
    protected virtual void childDie()
    {

    }
    public virtual void childUpdate()
    {

    }
    protected void trySpawnDrop()
    {
        float rand = Random.Range(0f, 1f);
        Debug.Log("Rand is " + rand);
        if (rand > (1-dropChance))
        {
            GameObject drop = finalDrops[Random.Range(0, finalDrops.Count)].drop;
            GameObject dropObj = Instantiate(drop, transform.position, Quaternion.identity);
            dropObj.transform.SetParent(GameController.instance.spawnableParent.transform);
        }
    }
    private IEnumerator flashRed()
    {
        if(--numHits<= 0)
        {
            childDie();
            GameController.instance.enemyKilled();
            trySpawnDrop();
            Destroy(this.gameObject);
        }
        yield return new WaitForSeconds(hitCooldown);
        sr.color = new Color(1, 1, 1);
        hitActive = false;
    }
    protected IEnumerator waitForMoveCooldown()
    {
        yield return new WaitForSeconds(moveCooldown);
        moveCooldownActive = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Projectile"))
        {
            sr.color = new Color(1, 0, 0);
            hitActive = true;
            StartCoroutine(flashRed());
        }
        if(collision.gameObject.CompareTag("Player"))
        {
            GameController.instance.takeDamage(damage);
            moveCooldownActive = true;
            StartCoroutine(waitForMoveCooldown());
        }
    }
}
