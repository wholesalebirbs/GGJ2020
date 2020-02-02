using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buga
{

    public enum BrokenStatusType
    {
        Idle,
        Broken,
        Repaired
    }

    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Breakable : Interactable
    {
        public BrokenStatusType status = BrokenStatusType.Idle;


        public bool canBeBrokenMultipleTimes = false;

        [SerializeField]
        [Tooltip("The force required to break this breakable.")]
        protected float damageForce = 5.0f;

        protected string damageableTag = "Cat";

        [SerializeField]
        [Tooltip("Broken prefab particles")]
        protected GameObject brokenPrefab;

        [SerializeField]
        [Range(0, 10)]
        protected int hitpoints = 3;

        protected float currentHitpoints;

        protected GameObject currentBrokenParts;

        MeshRenderer meshRenderer;
        Rigidbody rb;


        private void Awake()
        {
            Initialize();
        }
        protected override void Initialize()
        {
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            rb = GetComponentInChildren<Rigidbody>();
            currentHitpoints = hitpoints;
        }
        public override void Interact()
        {
            Repair();
            base.Interact();
        }


        private void Update()
        {
            //if (Input.GetButtonDown("Fire1"))
            //{
            //    Repair();
            //}
        }
        public virtual void Break()
        {
            if (status == BrokenStatusType.Idle)
            {
                status = BrokenStatusType.Broken;

                meshRenderer.enabled = false;
                rb.isKinematic = true;

                currentBrokenParts = Instantiate(brokenPrefab, transform.position, Quaternion.identity);
                Debug.Log($"{gameObject.name} is now broken.");
            }
        }


        public virtual void Repair()
        {
            if (status != BrokenStatusType.Broken)
            {
                return;
            }
            if (canBeBrokenMultipleTimes)
            {
                status = BrokenStatusType.Idle;
            }
            else
            {
                status = BrokenStatusType.Repaired;
            }
            currentHitpoints = hitpoints;
            Destroy(currentBrokenParts);
            meshRenderer.enabled = true;


            Debug.Log($"{gameObject.name} has been repaired.");
        }

        public virtual void TakeDamage(int damage)
        {
            currentHitpoints -= damage;

            Debug.Log($"{name} took damage.");
            if (currentHitpoints < 0)
            {
                currentHitpoints = 0;

                Break();
            }

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(damageableTag))
            {
                if (status == BrokenStatusType.Idle)
                {

                    Debug.Log($"Hit force: {collision.relativeVelocity.magnitude}");

                    if (collision.relativeVelocity.magnitude > damageForce)
                    {
                        TakeDamage(1);
                    }
                }
            }


        }
    }

}