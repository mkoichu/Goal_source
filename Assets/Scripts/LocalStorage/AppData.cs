using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This class holds the relevant data for saving and loading the game.
/// </summary>
[System.Serializable]
public class AppData
{
    public int score;
    public string[] ARPersonalInventory;
    public string[] ARStoreInventory;


    /// <summary>
    /// Initializes a new instance of the <see cref="AppData"/> class.
    /// </summary>
    /// <param name="appManager">The game manager class</param>
    public AppData()
    {
        score = GameManager.Instance.totalScore;
        var ARPersonalNewInvSize = GameManager.Instance.storeManager.ARPersonalNewInventory.transform.childCount;
        ARPersonalInventory = new string[ARPersonalNewInvSize];
        for (int i = 0; i < ARPersonalNewInvSize; i++)
        {
            ARItem item = GameManager.Instance.storeManager.ARPersonalNewInventory.transform.GetChild(i).GetComponent<ARItem>();
            var name = item.itemName;
            ARPersonalInventory[i] = name;

        }
        var StoreInventorySize = GameManager.Instance.storeManager.ARStoreInventory.transform.childCount;
        ARStoreInventory = new string[StoreInventorySize];
        for (int i = 0; i < StoreInventorySize; i++)
        {
            ARItem item = GameManager.Instance.storeManager.ARStoreInventory.transform.GetChild(i).GetComponent<ARItem>();
            var name = item.itemName;
            ARStoreInventory[i] = name;
        }


    }
}
