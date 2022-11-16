using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EnemyAI
{
    public class ZombieManager : MonoBehaviour
    {
        [SerializeField] int updatePerFrame;
        List<ZombieFlocking> zombieList = new List<ZombieFlocking>();
        // Start is called before the first frame update
        void Start()
        {
            zombieList = GetComponentsInChildren<ZombieFlocking>().ToList();
        }

        public void UpdateZombieList()
        {
            zombieList = GetComponentsInChildren<ZombieFlocking>().ToList();
        }

        void AddZombieToList(GameObject zombie)
        {
            zombieList.Add(zombie.GetComponent<ZombieFlocking>());
        }

        int lastUpdated = 0;
        // Update is called once per frame
        void Update()
        {
            int startPoint = lastUpdated;
            bool stopLooping = false;
            int amountUpdated = 0;

            //loops through only a few zombies starting where it left off
            for (int i = startPoint + 1; !stopLooping; i++)
            {
                lastUpdated = i;
                zombieList[i].UpdateDir();
                if (i >= zombieList.Count - 1)
                {
                    i = -1;
                    lastUpdated = -1;
                }

                amountUpdated++;
                if (amountUpdated > updatePerFrame)
                {
                    stopLooping = true;
                }
            }
        }
    }
}