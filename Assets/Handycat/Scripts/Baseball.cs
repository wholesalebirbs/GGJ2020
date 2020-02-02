using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buga
{
    public class Baseball : Minigame
    {
        [Header("Baseball Settings")]
        [SerializeField]
        protected Transform ballTransform;
        [SerializeField]
        protected Transform beginBallTransform;
        [SerializeField]
        protected Transform endBallTransform;
        [SerializeField]
        protected float pitchMinWaitTime = 2.0f;
        [SerializeField]
        protected float pitchMaxWaitTime = 8.0f;
        [SerializeField]
        protected float ballTravelTime = 2.0f;


        [Header("Batter")]
        [SerializeField]
        protected Animator batterAnimator;

        [Header("Controls")]
        protected string swingButton = string.Empty;

        public override void Initialize()
        {
            base.Initialize();
            ballTransform.position = beginBallTransform.position;
            ballTransform.localScale = beginBallTransform.localScale;

            miniGameMainPanel.SetActive(true);

            StartCoroutine(PitchCoroutine());
        }


        IEnumerator PitchCoroutine()
        {
            Debug.Log("Beginning pitch coroutine.");
            yield return new WaitForSeconds(Random.Range(pitchMinWaitTime, pitchMaxWaitTime));

            acceptingInput = true;

            Debug.Log("Pitching");
            StartCoroutine(LerpTransform(ballTransform, beginBallTransform, endBallTransform, ballTravelTime, true));
        }


        private void Update()
        {
            if (acceptingInput)
            {
                if (Input.GetButtonDown(swingButton))
                {
                    Swing();
                }
            }
        }
        protected override IEnumerator CleanUp()
        {
            bool success = EvaluateResults();
            if (success)
            {
                successfulVisual.SetActive(true);
                audioSource.clip = successfulClip;
            }
            else
            {
                successfulVisual.SetActive(false);
                audioSource.clip = failureClip;
            }

            audioSource.Play();
            
            yield return new WaitForSeconds(cleanUpTime);

            // turn off panel
            miniGameMainPanel.SetActive(false);

            // broadcast results
            OnMinigameEnded?.Invoke(success, score);
            

        }

        protected void Swing()
        {
            CleanUp();
        }

        public override bool EvaluateResults()
        {
            return true;
        }
    }

}