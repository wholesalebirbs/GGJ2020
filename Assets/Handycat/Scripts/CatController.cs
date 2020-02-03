using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buga
{
    public class CatController : MonoBehaviour
    {
        CatStates state = CatStates.OK;
        public Animator staticAnimator;
        public Rigidbody ragdollRigidbody;

        public Rigidbody spine;
        //public Rigidbody hips;

        public float rotationSpeed = 1;
        public float upperBodyStrength = 1;

        [Header("Controls")]
        public string movementAxis = "";
        public string rotationAxis = "";
        public string upperBodyVertical = "";
        public string upperBodyHorizontal = "";
        public string jumpButton = "";

        [Header("Debug Information")]
        public float rotationInputValue;

        public Vector3 upperBodyVector = Vector3.zero;

        bool canMove = false;

        float stunTime = 3.0f;
        float currentStunTime = 0.0f;

        private void Awake()
        {
            RepairController.OnRepairBegun += OnRepairBegun;
            RepairController.OnRepairCompleted += OnRepairComplete;

            GameManager.OnGameStarted += OnGameStarted;
            GameManager.OnGameEnded += OnGameEnded;
            canMove = false;
        }

        private void OnDestroy()
        {
            RepairController.OnRepairBegun -= OnRepairBegun;
            RepairController.OnRepairCompleted -= OnRepairComplete;

            GameManager.OnGameStarted -= OnGameStarted;
            GameManager.OnGameEnded -= OnGameEnded;
        }

        private void FixedUpdate()
        {
            switch (state)
            {
                case CatStates.OK:
                    if (canMove)
                    {
                        HandleCatRotation();

                        HandleUpperbody();

                        HandleAnimator();
                    }
                    break;
                case CatStates.Stunned:

                    currentStunTime += Time.fixedDeltaTime;
                    if (currentStunTime > stunTime)
                    {
                        state = CatStates.OK;
                        currentStunTime = 0;
                    }

                    break;
                default:
                    break;
            }


        }

        protected void OnGameStarted()
        {
            canMove = true;
        }
        protected void OnGameEnded()
        {
            canMove = false;
        }


        private void HandleAnimator()
        {
            staticAnimator.SetFloat("Forward", Input.GetAxis(movementAxis));
            staticAnimator.SetBool("Jump", Input.GetButton(jumpButton));
        }

        protected void HandleCatRotation()
        {
            rotationInputValue = Input.GetAxis(rotationAxis);

            Vector3 rotationAsvector = new Vector3(0, rotationInputValue * rotationSpeed, 0) * Time.fixedDeltaTime;
            Quaternion desiredRotation = Quaternion.Euler(rotationAsvector);

            ragdollRigidbody.MoveRotation(ragdollRigidbody.rotation * desiredRotation);
        }


        protected void HandleUpperbody()
        {
            upperBodyVector.x = Input.GetAxis(upperBodyHorizontal);
            upperBodyVector.z = -Input.GetAxis(upperBodyVertical);

            spine.AddForce(upperBodyVector * upperBodyStrength * Time.fixedDeltaTime, ForceMode.Impulse );

        }

        protected void OnRepairBegun()
        {
            
        }

        protected void OnRepairComplete(bool status, int score)
        {
            if (status)
            {
                state = CatStates.Stunned;
            }
        }

    }
}