using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanels : MonoBehaviour
{
    private int currPanelIndex = 0;
    public List<GameObject> panels;
    public List<GameObject> dots;
    public GameObject listOfDots;

    private float LOW_ALPHA = 0.2f;
    private float HIGH_ALPHA = 1f;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform panel in transform)
        {
            panels.Add(panel.gameObject);
        }

        foreach (Transform dot in listOfDots.transform)
        {
            dots.Add(dot.gameObject);
        }
    }

    public void NextPanel()
    {
        panels[currPanelIndex].gameObject.SetActive(false);
        ChangeAlpha(currPanelIndex, LOW_ALPHA);
        currPanelIndex++;
        ChangeAlpha(currPanelIndex, HIGH_ALPHA);
        panels[currPanelIndex].gameObject.SetActive(true);
    }

    public void PrevPanel()
    {
        panels[currPanelIndex].gameObject.SetActive(false);
        ChangeAlpha(currPanelIndex, LOW_ALPHA);
        currPanelIndex--;
        ChangeAlpha(currPanelIndex, HIGH_ALPHA);
        panels[currPanelIndex].gameObject.SetActive(true);
    }



    private void ChangeAlpha(int index, float alphaIntensity)
    {
        Image image = dots[index].GetComponent<Image>();
        var newColor = image.color;
        newColor.a = alphaIntensity;
        image.color = newColor;
    }

    public void OnDisable()
    {
        currPanelIndex = 0;
        panels[0].gameObject.SetActive(true);
        ChangeAlpha(0, HIGH_ALPHA);

        for (int i = 1; i<panels.Count; i++)
        {
            panels[i].gameObject.SetActive(false);
            ChangeAlpha(i, LOW_ALPHA);
        }
    }
}
