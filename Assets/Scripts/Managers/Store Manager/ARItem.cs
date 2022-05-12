using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ARItem : MonoBehaviour
    {
        public string itemName;
        public SpriteRenderer itemImage;
        public int price;
        public Sprite btnIcon;
        


        /*private void Start()
        {
            //itemName = gameObject.name;
            var buyBtn = transform.Find("BuyBtn").GetComponent<Button>();
            if (buyBtn != null)
            {
                buyBtn.onClick.AddListener(BuyItem);
            }
        }

        private void BuyItem()
        {
            Bus.OnBuyARItem.Publish(this);
        }*//*private void Start()
        {
            //itemName = gameObject.name;
            var buyBtn = transform.Find("BuyBtn").GetComponent<Button>();
            if (buyBtn != null)
            {
                buyBtn.onClick.AddListener(BuyItem);
            }
        }

        private void BuyItem()
        {
            Bus.OnBuyARItem.Publish(this);
        }*/

    }

