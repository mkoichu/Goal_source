using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CelebrationPanel : MonoBehaviour
{
    [SerializeField] GameSessionManager gameSessionManger;
    public TextMeshProUGUI maxVolumeText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public GameObject firstStar;
    public GameObject secondStar;
    public GameObject thirdStar;


    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(SetFloatValue((float)(gameSessionManger.getElapsedTime()), timeText));
        StartCoroutine(SetIntValue((int)gameSessionManger.getMaxVolume(), maxVolumeText));
        StartCoroutine(SetValueFast((int)gameSessionManger.getScore(), scoreText));
        StartCoroutine(SetStars((float)gameSessionManger.getScore()));

    }

    IEnumerator SetStars(float score)
    {
        yield return new WaitForSeconds(1);
        if(score > 3000)
        {
            firstStar.SetActive(true);
        }
        yield return new WaitForSeconds(1);
        if (score > 6000)
        {
            secondStar.SetActive(true);
        }
        yield return new WaitForSeconds(1);
        if (score > 9000)
        {
            thirdStar.SetActive(true);
        }
    }



    IEnumerator SetIntValue(int finalValue, TextMeshProUGUI uiText)
    {
        var value = 0;
        while (value < finalValue)
        {
            value++;
            uiText.text = value.ToString();
            yield return null;
        }
        uiText.text = finalValue.ToString();
    }

    IEnumerator SetFloatValue(float finalValue, TextMeshProUGUI uiText)
    {
        var value = 0;
        while (value < finalValue)
        {
            value++;
            uiText.text = value.ToString();
            yield return null;
        }
        uiText.text = finalValue.ToString("f2");
    }

    IEnumerator SetValueFast(int finalValue, TextMeshProUGUI uiText)
    {
        var value = 0;
        while (value < finalValue - 100)
        {
            if (value < finalValue / 5)
            { 
                value += (int)(finalValue/200);
            }
            else if (value < 2 * (finalValue / 5))
            {
                value += (int)(finalValue / 100);
            }
            else if (value < 3 * (finalValue / 5))
            {
                value += (int)(finalValue / 50);
            }
            else
            {
                value += (int)(finalValue / 20);

            }

            uiText.text = value.ToString();
            yield return null;
        }
        uiText.text = finalValue.ToString();
    }

}
