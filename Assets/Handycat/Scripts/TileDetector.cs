using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Buga
{

    public enum  TileType
    {
        Red,
        Yellow,
        Blue,
        Green
    }

    public class TileDetector : MonoBehaviour
    {
        [SerializeField]
        protected float maxDetectiondistance = 1;

        [SerializeField]
        protected string triggerDetectionButton;
        [SerializeField]
        protected Transform tilesTarget;
        public Transform TilesTarget
        {
            get => tilesTarget;
        }

        public List<MovingObject> currentTiles;


        public bool HasTiles
        {
            get
            {

                Debug.Log($"Current tiles on {gameObject.name}: {currentTiles.Count}");
                return currentTiles.Count > 0;
            }
        }

        public static UnityAction OnTileMissed;

        public static UnityAction OnTileHit;

        public bool acceptingInput = false;
        public void ToggleInput(bool status)
        {
            acceptingInput = status;
        }

        private void OnEnable()
        {
            MovingObject.OnTargetReached += OnMovingObjectTargetReached;
        }
        private void OnDisable()
        {
            MovingObject.OnTargetReached -= OnMovingObjectTargetReached;
        }

        private void Update()
        {
            if (acceptingInput)
            {
                if (Input.GetButtonDown(triggerDetectionButton))
                {
                    Debug.Log($"{gameObject.name} detecting Input");
                    Detect();
                }
            }
        }

        protected void OnMovingObjectTargetReached(MovingObject m)
        {
            for (int i = 0; i < currentTiles.Count; i++)
            {
                if (currentTiles[i] == m)
                {
                    GameObject tile = currentTiles[i].gameObject;
                    currentTiles.RemoveAt(i);

                    Destroy(tile);

                    OnTileMissed?.Invoke();
                }
            }
        }

        public void Detect()
        {
            for (int i = 0; i < currentTiles.Count; i++)
            {
                if (Vector3.Distance(currentTiles[i].transform.position, transform.position) <= maxDetectiondistance)
                {
                    GameObject tile = currentTiles[i].gameObject;
                    currentTiles.RemoveAt(i);

                    Destroy(tile);

                    Debug.Log("Tile Hit!");
                    OnTileHit?.Invoke();
                    return;
                }
            }

            Debug.Log("Tile Missed");
            OnTileMissed?.Invoke();
        }

        public void CleanUp()
        {
            acceptingInput = false;

            for (int i = currentTiles.Count -1; i > -1; i--)
            {
                Destroy(currentTiles[i].gameObject);
            }

            currentTiles.Clear();
        }


        public void AddTile(MovingObject mo)
        {
            currentTiles.Add(mo);
        }

    }
}

