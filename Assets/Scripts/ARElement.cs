using UnityEngine;

[CreateAssetMenu(fileName = "ARElement", menuName = "ScriptableObjects/ARElement", order = 1)]
public class ARElement : ScriptableObject
{
    public string description;
    public string exactElementObjectName;
    public Sprite iconImage;
}