using EnemyAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] GameObject zombiePrefab;

    ZombieManager manager;

    [SerializeField] float spawnDist;
    [SerializeField] int zombieAmount;

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<ZombieManager>();
        SpawnInGrid(zombieAmount);
    }


    void SpawnInGrid(int amount)
    {
        int width = (int)Mathf.Sqrt((amount));
        int rest = amount % width;
        int middle = width / 2;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < width; z++)
            {
                GameObject newZombie = Instantiate(zombiePrefab);
                newZombie.transform.position = transform.position + new Vector3((float)(x - middle) * spawnDist, 0f, (float)(z - middle) * spawnDist);
                newZombie.transform.SetParent(transform);
            }
        }

        manager.UpdateZombieList();
    }
}
