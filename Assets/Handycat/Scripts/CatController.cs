using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buga
{
    public class CatController : MonoBehaviour
    { 
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
        private void FixedUpdate()
        {
            HandleCatRotation();

            HandleUpperbody();

            HandleAnimator();
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
            upperBodyVector.y = Input.GetAxis(upperBodyVertical);

            spine.AddForce(upperBodyVector * upperBodyStrength * Time.fixedDeltaTime);

        }

    }
}