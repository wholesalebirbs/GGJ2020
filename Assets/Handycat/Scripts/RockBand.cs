using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buga
{
    public class RockBand : Minigame
    {
        [SerializeField]
        protected List<TileDetector> detectors = new List<TileDetector>();

        [SerializeField]
        protected List<TileSpawner> spawners = new List<TileSpawner>();

        [SerializeField]
        protected int maxNumberOfTiles = 10;

        protected int currentTiles = 0;

        bool missedTile = false;

        private void Awake()
        {
            TileSpawner.OnTileSpawned += OnTileSpawned;

            TileDetector.OnTileMissed += OnTileMissed;

            TileDetector.OnTileHit += OnTileHit;

            Initialize();

        }
        public override void Initialize()
        {
            base.Initialize();

            for (int i = 0; i < detectors.Count; i++)
            {
                detectors[i].ToggleInput(true);
            }

            for (int i = 0; i < spawners.Count; i++)
            {
                spawners[i].BeginSpawning();
            }
        }


        protected void OnTileHit()
        {
            bool tilesRemain = false;

            for (int i = 0; i < detectors.Count; i++)
            {
                if (detectors[i].HasTiles)
                {
                    tilesRemain = true;
                    break;
                }
            }

            if (currentTiles == maxNumberOfTiles && !tilesRemain)
            {
                Debug.Log("No tiles remain, cleaning up.");
                StartCoroutine(CleanUp());
            }
        }

        /// <summary>
        /// Check if we need to stop spawners.
        /// </summary>
        protected void OnTileSpawned()
        {
            currentTiles++;

            // if we;ve spawned the max number of tiles, stop spawning tiles.
            if (currentTiles >= maxNumberOfTiles)
            {

                Debug.Log("Max Tiles reached, stopping tile spawning.");
                for (int i = 0; i < spawners.Count; i++)
                {
                    spawners[i].StopSpawning();
                }
            }
        }
        

        protected void OnTileMissed()
        {
            Debug.Log("Tile was missed");
            missedTile = true;
            StartCoroutine(CleanUp());
        }


        protected override IEnumerator CleanUp()
        {
            for (int i = 0; i < detectors.Count; i++)
            {
                detectors[i].ToggleInput(false);
                detectors[i].CleanUp();
            }
            for (int i = 0; i < spawners.Count; i++)
            {
                spawners[i].StopSpawning();
            }

            bool success = EvaluateResults();
            Debug.Log($"Rockband success{success}");

            yield return new WaitForSeconds(1.0f);

            ShowEndVisuals(success);

            yield return new WaitForSeconds(cleanUpTime);

            EndMinigame(success, score);
        }


        public override bool EvaluateResults()
        {
            return !missedTile;
        }


    }
}