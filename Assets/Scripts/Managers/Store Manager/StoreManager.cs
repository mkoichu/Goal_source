using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


    //public static readonly UnityEvent<ARItem> OnBuyARItem;

public class StoreManager : MonoBehaviour
{
    public Sprite redBtnSprite;
    public Sprite greenBtnSprite;
    public Transform itemButtonPrefab;
    [SerializeField] private Transform storeEntryPrefab;

    public GameObject AllARItems;
    //[SerializeField] private GameObject AllARPersonalElements;
    public GameObject ARStoreInventory;
    public GameObject ARPersonalOriginalInv;
    public GameObject ARPersonalNewInventory;

    private void OnEnable()
    {
        UpdateBuyButtons();
    }

    public void ARStoreFirstLoad()
    {
        Debug.Log("First Load");
        foreach (Transform item in AllARItems.transform)
        {
            InstantiateStoreItem(item.gameObject);
        }
    }

    private void InstantiateStoreItem(GameObject item)
    {
        ARItem arItem = item.GetComponent<ARItem>();
        var newStoreEntry = Instantiate(storeEntryPrefab, ARStoreInventory.transform);
        newStoreEntry.Find("ItemImage").GetComponent<Image>().sprite = arItem.btnIcon;
        newStoreEntry.Find("Price").GetComponent<TextMeshProUGUI>().text = arItem.price.ToString();

        var newARItem = newStoreEntry.GetComponent<ARItem>();
        var buyBtn = newStoreEntry.transform.Find("BuyBtn").GetComponent<Button>();
        if (buyBtn != null)
        {
            buyBtn.onClick.AddListener(delegate { BuyItem(newARItem); });
        }
        CopyARItem(arItem, newARItem);
        newStoreEntry.gameObject.SetActive(true);
    }

    public void LoadARStoreElements(AppData data)
    {
        Debug.Log("NOT First Load");
        //Load the items left in the AR Store:
        for (int i = 0; i < data.ARStoreInventory.Length; i++)
        {
            String itemName = data.ARStoreInventory[i];
            var item = AllARItems.transform.Find(itemName).gameObject;
            if(item != null)
            {
                InstantiateStoreItem(item);
            }   
        }

        //Load all the personal items purchesed from the AR Store (+ the original items):
        for (int i = 0; i < data.ARPersonalInventory.Length; i++)
        {
            String itemName = data.ARPersonalInventory[i];
            var item = AllARItems.transform.Find(itemName).gameObject;
            if (item != null)
            {
                ARItem arItem = AllARItems.transform.Find(itemName).GetComponent<ARItem>();
                AddItemToMyInventory(arItem);
            }   
        }
    }

    private void CopyARItem(ARItem sourceItem, ARItem targetItem)
    {
        targetItem.itemName = sourceItem.itemName;
        targetItem.itemImage = sourceItem.itemImage;
        targetItem.price = sourceItem.price;
        targetItem.btnIcon = sourceItem.btnIcon;
    }

    public void UpdateBuyButtons()
        {
            foreach (Transform child in ARStoreInventory.transform)
            {
                var buyBtn = child.transform.Find("BuyBtn").gameObject.GetComponent<Button>();
                var price = child.gameObject.GetComponent<ARItem>().price;
                if (GameManager.Instance.totalScore >= price)
                {
                    buyBtn.GetComponent<Image>().sprite = greenBtnSprite;
                }
                else
                {
                    buyBtn.GetComponent<Image>().sprite = redBtnSprite;
                }
            }

        }




        private void BuyItem(ARItem arItem)
        {
            if (GameManager.Instance.totalScore >= arItem.price)
            {
                //Update total number of coins:
                GameManager.Instance.ModifyTotalScoreBy(-arItem.price);

                //Add item to inventory:
                AddItemToMyInventory(arItem);

                //Cash register sound:
                AudioManager.Instance.PlayCashRegisterSound();

                UpdateBuyButtons();
                //Remove item from store:
                RemoveItem(arItem.gameObject);

            }
        }

        private void RemoveItem(GameObject arItem)
        {
            Destroy(arItem, 0.1f);
        }

        public void AddItemToMyInventory(ARItem arItem)
        {
            var newItemButton = Instantiate(itemButtonPrefab, ARPersonalNewInventory.transform);
            newItemButton.GetComponent<Image>().sprite = arItem.btnIcon;
            var btn = newItemButton.GetComponent<Button>();
            btn.onClick.AddListener(delegate { ToggleARItem(arItem); });

            var newARItem = newItemButton.GetComponent<ARItem>();
            CopyARItem(arItem, newARItem);
        }

        private void ToggleARItem(ARItem arItem)
        {
            arItem.itemImage.enabled = !arItem.itemImage.enabled;
        }


}
