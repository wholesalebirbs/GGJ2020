using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buga
{
    public class CatAudioController : RandomAudioPlayer
    {
        [SerializeField]
        protected string meowButton = "P2XButton";

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown(meowButton))
            {
                PlayRandomSound();
            }
        }
    }
}