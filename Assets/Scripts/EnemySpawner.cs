using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    EnemyAI[] enemyPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, GameController.instance.level);
        GameController.instance.addEnemiesToRoom(rand);
        for (int i = 0; i <= rand; i++)
        {
            EnemyAI ai = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
            ai.GetComponent<Spawnable>().setRoom(GetComponent<Spawnable>().getRoom());
            ai.transform.SetParent(GameController.instance.spawnableParent.transform);
        }
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
