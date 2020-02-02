using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Buga
{
    public class Minigame : MonoBehaviour
    {
        [Header("Base Settings")]
        [SerializeField]
        protected GameObject miniGameMainPanel;

        [SerializeField]
        protected float setUpTime = 3.0f;

        [SerializeField]
        protected float cleanUpTime = 2.0f;

        [SerializeField]
        protected bool acceptingInput = false;

        protected virtual int score = 0;

        protected AudioSource audioSource;


        [Header("End Results UI")]
        [SerializeField]
        protected GameObject successfulVisual;
        [SerializeField]
        protected GameObject failureVisual;
        [SerializeField]
        protected AudioClip successfulClip;
        [SerializeField]
        protected AudioClip failureClip;

        public static UnityAction<bool, int> OnMinigameEnded;

        protected virtual IEnumerator SetUp()
        {
            yield return null;
        }

        protected virtual IEnumerator CleanUp()
        {
            yield return null;
        }

        public virtual void Initialize()
        {
            score = 0;
            successfulVisual.SetActive(false);
            failureVisual.SetActive(false);
            audioSource = GetComponentInChildren<AudioSource>();
        }


        public IEnumerator LerpTransform(Transform moving, Transform from, Transform to, float time, bool lerpScale)
        {
            float elapsedTime = 0;
            Vector3 startingPosition = from.position;
            Vector3 startingScale = from.localScale;

            while (elapsedTime < time)
            {
                moving.position = Vector3.Lerp(startingPosition, to.position, elapsedTime / time);
                if (lerpScale)
                {
                    moving.localScale = Vector3.Lerp(startingScale, to.localScale, elapsedTime / time);
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            moving.position = to.position;
        }

        public virtual bool EvaluateResults()
        {
            return false;
        }


        protected virtual void EndMinigame(bool results, int score)
        {

            // turn off panel
            miniGameMainPanel.SetActive(false);

            // broadcast results
            OnMinigameEnded?.Invoke(results, score);
        }

    }
}
