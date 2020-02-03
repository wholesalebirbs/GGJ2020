using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buga
{
    [RequireComponent(typeof(BoxCollider))]
    public class Interactor : MonoBehaviour
    {
        private bool canInteract = false;

        private Interactable activeInteractable;

        public void InteractWithInteractable()
        {
            if (activeInteractable != null && canInteract)
            {
                Debug.Log($"{name} interacting with {activeInteractable.gameObject.name}");
                activeInteractable.Interact();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Interactable temp = other.GetComponent<Interactable>();
            if (temp != null)
            {
                Breakable breakable = temp as Breakable;

                if (breakable.status == BrokenStatusType.Broken)
                {
                    Debug.Log($"Triggered by: {other.name}");
                    activeInteractable = temp;
                    canInteract = true;
                }
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Interactable>() == activeInteractable)
            {
                canInteract = false;
                activeInteractable = null;
            }
        }
    }
}
