using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChanger : MonoBehaviour
{
    public List<Camera> cams;

    public Light mainLight;

    public List<Color> possibleColors;

    public float timebetweenSwitching = 1f;

    public float currentTime = 0;

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > timebetweenSwitching)
        {
            Switch();

            currentTime = 0;
        }
    }


    public void Switch()
    {
        Color color = possibleColors[Random.Range(0, possibleColors.Count)];

        for (int i = 0; i < cams.Count; i++)
        {
            cams[i].backgroundColor = color;
        }

        mainLight.color = color;
    }
}
