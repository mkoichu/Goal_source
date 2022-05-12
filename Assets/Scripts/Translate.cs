using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Translate : MonoBehaviour
{
    [TextArea]
    public string hebrewText;
    public int fontSize = 0;
    public bool bold = false;
    private TextMeshProUGUI label;

    
 

    // Start is called before the first frame update
    void Start()
    {
        label = GetComponent<TextMeshProUGUI>();
        if (Application.systemLanguage == SystemLanguage.Hebrew)
        {
            label.text = hebrewText;
            label.font = GameManager.Instance.hebrewFont;
            label.fontSize = label.fontSize-2;
            if (bold || label.isUsingBold)
            {
                label.fontStyle = (TMPro.FontStyles)FontStyle.Bold;
            }
            
            if (fontSize > 0)
            {
                label.fontSize = fontSize;
            }
        }
    }


}
