using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

namespace Buga
{

    public class Interactable : MonoBehaviour
    {
        public static UnityAction<Interactable> OnInteractCalled;

        bool canInteract = true;

        protected virtual void Initialize()
        {

        }

        public virtual void Interact()
        {
            if (!canInteract)
            {
                return;
            }

            OnInteractCalled?.Invoke(this);

        }
    }

}
