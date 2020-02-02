using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buga
{
    public class MinigameController : MonoBehaviour
    {
        [SerializeField]
        protected List<Minigame> minigames = new List<Minigame>();

        protected Minigame activeMinigame = null;

        private void OnEnable()
        {
            RepairController.OnRepairBegun += OnRepairBegun;
        }

        private void OnDisable()
        {
            RepairController.OnRepairBegun -= OnRepairBegun;
        }

        protected void OnRepairBegun()
        {
            activeMinigame = GetRandomMinigame();
            activeMinigame.Initialize();
        }

        protected Minigame GetRandomMinigame()
        {
            Minigame retVal = minigames[Random.Range(0, minigames.Count)];

            return retVal;
        }
    }
}
