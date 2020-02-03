using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buga
{
    public class SpiderSwat : Minigame
    {
        [Header("Spider Swat")]
        [SerializeField]
        protected float timeOutTime = 3.0f;

        protected float currentTime = 0;

        [SerializeField]
        List<Transform> possibleLocations = new List<Transform>();

        [SerializeField]
        Transform swatterStart;

        [SerializeField]
        GameObject swatter;


        [SerializeField]
        GameObject spider;

        [SerializeField]
        protected string swatButton;

        [SerializeField]
        protected float movementSpeed = 2.0f;

        public override void Initialize()
        {
            currentTime = 0;
            swatter.transform.position = swatterStart.position;


            spider.transform.position = possibleLocations[Random.Range(0, possibleLocations.Count)].position;

           

            base.Initialize();
            miniGameMainPanel.SetActive(true);
            Debug.Log("Spider Swat Initialized");
            StartCoroutine(SetUp());
        }

        protected override IEnumerator SetUp()
        {

            yield return new WaitForSeconds(setUpTime);
            acceptingInput = true;
        }

        private void Update()
        {
            if (acceptingInput)
            {
                currentTime += Time.deltaTime;

                if (currentTime >= timeOutTime)
                {
                    Debug.Log($"SpiderSwat: {currentTime } is longer than {timeOutTime}");
                    StartCoroutine(CleanUp());
                }
                else
                {
                    float h = Input.GetAxis("Horizontal");
                    float v = Input.GetAxis("Vertical");

                    swatter.transform.position += new Vector3(h, v) * movementSpeed *Time.deltaTime;


                    if (Input.GetButtonDown(swatButton))
                    {
                        Debug.Log("Swatted");
                        StartCoroutine(CleanUp());
                    }
                }
            }
        }

        protected override IEnumerator CleanUp()
        {
            acceptingInput = false;
            bool success = EvaluateResults();

            if (success)
            {
                score = -1;
            }
            //yield return new WaitForSeconds(1.0f);

            ShowEndVisuals(success);

            yield return new WaitForSeconds(cleanUpTime);

            EndMinigame(success, score);
        }


        public override bool EvaluateResults()
        {
            return Vector3.Distance(spider.transform.position, swatter.transform.position) < .5f;
        }
    }
}
