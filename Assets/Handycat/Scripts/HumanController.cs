﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

namespace Buga
{
    public class HumanController : MonoBehaviour
    {

        Interactor interactor;
        ThirdPersonCharacter thirdpersonCharacter;

        public Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

        [SerializeField]
        private string interactButton = string.Empty;

        bool canMove = true;

        private void Awake()
        {
            interactor = GetComponentInChildren<Interactor>();
            thirdpersonCharacter = GetComponentInChildren<ThirdPersonCharacter>();

            RepairController.OnRepairBegun += OnRepairBegun;
            RepairController.OnRepairCompleted += OnRepairComplete;
        }
        private void OnDestroy()
        {
            RepairController.OnRepairBegun -= OnRepairBegun;
            RepairController.OnRepairCompleted -= OnRepairComplete;
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            if (!canMove)
            {
                // pass all parameters to the character control script
                thirdpersonCharacter.Move(Vector3.zero, false, false);
                return;

            }
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            if (Input.GetButtonDown(interactButton))
            {
                interactor.InteractWithInteractable();
            }

            // pass all parameters to the character control script
            thirdpersonCharacter.Move(m_Move, crouch, m_Jump);

            m_Jump = false;
        }

        protected void OnRepairBegun()
        {
            canMove = false;
        }

        protected void OnRepairComplete(bool success, int score)
        {
            canMove = true;
        }
    }
}
