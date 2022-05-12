using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// This script is assigned to the progress bar. It is responsible for its functionality.
/// </summary>
public class VolumeMeter : MonoBehaviour
{
    public Image volumeForeground;

    [SerializeField] private float updateSpeedSeconds = 0.05f;
    [SerializeField] private float currVolume = 0f;
    [SerializeField] private float MAX_VOLUME = 100f;


    /// <summary>
    /// Starts this instance and initializes important parameters and variables
    /// </summary>
    private void OnEnable()
    {
        SetVolume(0);
    }

    

    public void SetVolume(float value)
    {
        if (value < MAX_VOLUME)
        {
            currVolume = Math.Max(0, value);
        }
        else
        {
            currVolume = MAX_VOLUME;
        }

        float currEnergyPercent = (currVolume / MAX_VOLUME);
        if (currEnergyPercent < 0.99)
        {
            StartCoroutine(ChangeToPercent(currEnergyPercent));
        }


    }

    /// <summary>
    /// Gradually changes the viual to the given percentage.
    /// </summary>
    /// <param name="percent">The target percentage of the progress bar.</param>
    /// <returns></returns>
    private IEnumerator ChangeToPercent(float percent)
    {
        float preChangePercent = volumeForeground.fillAmount;
        float elapsed = 0f;
        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            volumeForeground.fillAmount = Mathf.Lerp(preChangePercent, percent, elapsed / updateSpeedSeconds);
            yield return null;
        }
        volumeForeground.fillAmount = percent;
    }

}
