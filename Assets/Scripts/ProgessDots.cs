using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgessDots : MonoBehaviour
{
    private int currDotIndex = 0;
    public List<GameObject> dots;
    private float LOW_ALPHA = 0.2f;
    private float HIGH_ALPHA = 1f;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform dot in transform)
        {
            dots.Add(dot.gameObject);
        }
    }

    public void NextDot()
    {
        ChangeAlpha(currDotIndex, LOW_ALPHA);
        currDotIndex++;
        ChangeAlpha(currDotIndex, HIGH_ALPHA);
    }

    public void PrevDot()
    {
        ChangeAlpha(currDotIndex, LOW_ALPHA);
        currDotIndex--;
        ChangeAlpha(currDotIndex, HIGH_ALPHA);
    }

    private void ChangeAlpha(int index, float alphaIntensity)
    {
        Image image = dots[index].GetComponent<Image>();
        var newColor = image.color;
        newColor.a = alphaIntensity;
        image.color = newColor;
    }


}
