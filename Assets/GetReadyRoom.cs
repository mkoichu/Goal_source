using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using System;

public class GetReadyRoom : MonoBehaviour
{
    [ReorderableList] public List<ARCategory> ARCategories;

    [SerializeField] private Transform categoryMenuContent;
    [SerializeField] private Transform elementsMenuContent;

    [SerializeField] private GameObject categoryButtonPrefab;
    [SerializeField] private GameObject elementButtonPrefab;
   

     [SerializeField] private GameObject zapparElements;

    // Start is called before the first frame update
    void Start()
    {
        //Clear all menu content:
        ClearContent(categoryMenuContent);
        ClearContent(elementsMenuContent);
    //    Debug.Log($"=========== ZAPPER ELEMENTS: ===============");
    //     foreach(Transform e in zapparElements.transform){
    //         Debug.Log($"zap el: {e.gameObject.name}");
    //     }

        //Create new ar category buttons:
        foreach(var cat in ARCategories){
            var catButton = Instantiate(categoryButtonPrefab, categoryMenuContent);
            catButton.GetComponent<Image>().sprite = cat.iconImage;
            catButton.GetComponent<Button>().onClick.AddListener(() => OpenCategory(cat));
        }
    }

    private void OpenCategory(ARCategory cat)
    {
        ClearContent(elementsMenuContent);
        foreach(var elem in cat.ARElemetns){
            var elemButton = Instantiate(elementButtonPrefab, elementsMenuContent);
            elemButton.GetComponent<Image>().sprite = elem.iconImage;
            elemButton.GetComponent<Button>().onClick.AddListener(() => EnableARElement(cat, elem));
        }
    }

    private void EnableARElement(ARCategory cat, ARElement elem)
    {
        var zapparElem = zapparElements.transform.Find(elem.exactElementObjectName);
        Debug.Log($"cat name: {cat.exactCategoryObjectName}");
        Debug.Log($"elem name: {elem.exactElementObjectName}");
        Debug.Log($"curr elem name: {cat.currElement}");
        Debug.Log($"zapparElem: {zapparElem}");
        Debug.Log($"zapparElem name: {zapparElem.gameObject.name}");
        if(zapparElem != null){
            if(cat.currElement != null){
                cat.currElement.SetActive(false);
            }
            
            if(cat.currElement != null && cat.currElement.name == elem.exactElementObjectName){
                cat.currElement.SetActive(false);
                cat.currElement = null;  
            }else{
                zapparElem.gameObject.SetActive(true);
                cat.currElement = zapparElem.gameObject;
            }
        }else{
            Debug.LogError("This AR Element does not exist in the Zappar Tracker Object! \nCheck the name again.");
        }
    }

    private void ClearContent(Transform transform){
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
    }
}
