using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Buga
{
    public class RepairController : MonoBehaviour
    {
        public static UnityAction OnRepairBegun;
        public static UnityAction<bool, int> OnRepairCompleted;

        protected Interactable currentInteractable;
        private void Awake()
        {
        }

        private void OnEnable()
        {

            Interactable.OnInteractCalled += OnInteractCalled;
        }

        private void OnDisable()
        {

            Interactable.OnInteractCalled -= OnInteractCalled;
        }


        public void BeginRepair()
        {
            Debug.Log("Repair Begun");
            OnRepairBegun?.Invoke();
        }

        public void CompleteRepair(bool status, int score)
        {
            OnRepairCompleted?.Invoke(status, score);
        }

        protected void OnInteractCalled(Interactable interactable)
        {
            currentInteractable = interactable;
            BeginRepair();
        }
    }
}