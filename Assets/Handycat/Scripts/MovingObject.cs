using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Buga
{
    public class MovingObject : MonoBehaviour
    {
        [SerializeField]
        protected Transform target;

        [SerializeField]
        protected float movementSpeed = 5;

        public static UnityAction<MovingObject> OnTargetReached;
        

        private void Update()
        {
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, target.transform.position) == 0)
                {
                    Debug.Log("Tile Reached Target");
                    OnTargetReached?.Invoke(this);
                }
            }
        }

        public void SetTarget(Transform t)
        {
            target = t;
        }
    }

}