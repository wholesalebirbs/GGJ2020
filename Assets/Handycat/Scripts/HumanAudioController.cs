using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buga
{
    public class HumanAudioController : RandomAudioPlayer
    {

        protected override void Awake()
        {
            base.Awake();
            Breakable.OnBreakableBroken += OnBreakableBroken;
        }

        protected void OnBreakableBroken()
        {
            PlayRandomSound();
        }


        private void OnDestroy()
        {
            Breakable.OnBreakableBroken -= OnBreakableBroken;

        }

    }
}