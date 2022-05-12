using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ARCategory", menuName = "ScriptableObjects/ARCategory", order = 1)]
public class ARCategory : ScriptableObject
{

    public string exactCategoryObjectName;
    public Sprite iconImage;
    public GameObject currElement = null;

    [ReorderableList] public List<ARElement> ARElemetns;

}
