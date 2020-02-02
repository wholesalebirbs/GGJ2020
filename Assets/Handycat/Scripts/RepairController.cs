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

        protected Breakable currentBreakable;
        private void Awake()
        {
        }

        private void OnEnable()
        {

            Interactable.OnInteractCalled += OnInteractCalled;

            Minigame.OnMinigameEnded += CompleteRepair;
        }

        private void OnDisable()
        {

            Interactable.OnInteractCalled -= OnInteractCalled;

            Minigame.OnMinigameEnded -= CompleteRepair;
        }


        public void BeginRepair()
        {
            Debug.Log("Repair Begun");
            OnRepairBegun?.Invoke();
        }

        public void CompleteRepair(bool status, int score)
        {
            if (currentBreakable != null)
            {
                currentBreakable.Repair();
            }

            OnRepairCompleted?.Invoke(status, score);
        }

        protected void OnInteractCalled(Interactable interactable)
        {
            currentBreakable = interactable as Breakable;
            BeginRepair();
        }
    }
}