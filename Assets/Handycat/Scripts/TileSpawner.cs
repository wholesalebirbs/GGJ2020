using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Buga
{
    public class TileSpawner : MonoBehaviour
    {
        [SerializeField]
        protected TileDetector detector;

        [SerializeField]
        protected GameObject tilePrefab;


        protected float minSpawnRate = 1.0f;
        protected float maxSpawnRate = 5.0f;


        bool running = false;
        float nextSpawn = 0;

        public static UnityAction OnTileSpawned;

        public void BeginSpawning()
        {
            nextSpawn = Time.time + Random.Range(minSpawnRate, maxSpawnRate);
            running = true;
        }

        public void StopSpawning()
        {
            running = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (running == true)
            {
                if (Time.time > nextSpawn)
                {
                    Spawn();
                }
            }
           
        }


        protected void Spawn()
        {
            GameObject go = Instantiate(tilePrefab, transform.position, Quaternion.identity, this.transform);
            go.GetComponent<MovingObject>().SetTarget(detector.TilesTarget);

            detector.AddTile(go.GetComponent<MovingObject>());

            nextSpawn = Time.time + Random.Range(minSpawnRate, maxSpawnRate);

            Debug.Log($"{gameObject.name} spawned tile");
            OnTileSpawned?.Invoke();
        }
    }
}