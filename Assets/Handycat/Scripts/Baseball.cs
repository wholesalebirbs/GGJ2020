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
        protected Transform targetTransform;
        [SerializeField]
        protected Transform beginBallTransform;
        [SerializeField]
        protected Transform endBallTransform;
        [SerializeField]
        protected Transform hitBallTransform;
        [SerializeField]
        protected float maxDistanceToTarget = 1;
        protected float currentDistanceToTarget = 0;

        [Header("Pitcher")]
        [SerializeField]
        protected Animator pitcherAnimator;

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
        [SerializeField]
        protected string swingButton = string.Empty;


        Coroutine currentballRoutine;

        public override void Initialize()
        {
            base.Initialize();
            ballTransform.position = beginBallTransform.position;
            ballTransform.localScale = beginBallTransform.localScale;

            ballTransform.gameObject.SetActive(false);
            miniGameMainPanel.SetActive(true);

            StartCoroutine(PitchCoroutine());
        }


        IEnumerator PitchCoroutine()
        {
            Debug.Log("Beginning pitch coroutine.");
            yield return new WaitForSeconds(Random.Range(pitchMinWaitTime, pitchMaxWaitTime));

            pitcherAnimator.SetBool("Pitch", true);
            acceptingInput = true;

            Debug.Log("Pitching");
            ballTransform.gameObject.SetActive(true);
            currentballRoutine =  StartCoroutine(LerpTransform(ballTransform, beginBallTransform, endBallTransform, ballTravelTime, true));
        }


        private void Update()
        {
            if (acceptingInput)
            {
                if (Input.GetButtonDown(swingButton))
                {
                    Swing();
                }

                if (ballTransform.position == endBallTransform.position)
                {
                    StartCoroutine(CleanUp());
                }
            }

            currentDistanceToTarget = Vector3.Distance(ballTransform.position, targetTransform.position);


        }
        protected override IEnumerator CleanUp()
        {
            acceptingInput = false;
            bool success = EvaluateResults();
            if (success)
            {
                StopCoroutine(currentballRoutine);
                currentballRoutine = StartCoroutine(LerpTransform(ballTransform, ballTransform, hitBallTransform,1, false));
            }

            yield return new WaitForSeconds(1.0f);

            ShowEndVisuals(success);
            
            yield return new WaitForSeconds(cleanUpTime);

            EndMinigame(success, score);         

        }

        protected void Swing()
        {
            batterAnimator.SetBool("Swing", true);
            StartCoroutine(CleanUp());
        }

        public override bool EvaluateResults()
        {
            if (currentDistanceToTarget > maxDistanceToTarget)
            {
                return false;
            }
            return true;
        }

    }

}