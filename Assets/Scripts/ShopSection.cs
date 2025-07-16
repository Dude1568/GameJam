using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSection : MonoBehaviour
{

    [SerializeField] ItemType type;
    PurchasableItem[] allPurchasables;
    [SerializeField] GameObject shopButton;
    
    Image buttonImage;

    void Awake()
    {
       
        if (type == ItemType.Monster)
        {
            allPurchasables = Resources.LoadAll<PurchasableItem>("Monsters");
        }
        else
        {
            allPurchasables = Resources.LoadAll<PurchasableItem>("Traps");
        }
        GenerateShopTiles();
        
    }
    void GenerateShopTiles()
    {
        foreach (var purchasableItem in allPurchasables)
        {
            
            // Instantiate the Shop Button prefab
            GameObject newButton = Instantiate(shopButton, transform);
            Debug.Log($"shopbutton: ",newButton);
            if (newButton)
            {

            }
            // Set the placeable prefab on the child component
            PlaceableIcon icon = newButton.GetComponentInChildren<PlaceableIcon>();
            if (icon != null)
            {
                icon.placeablePrefab = purchasableItem.prefab;
            }
            TextMeshProUGUI price;
            price =newButton.GetComponentInChildren<TextMeshProUGUI>();

            if (price != null)
            {
                price.text = purchasableItem.cost.ToString();
            }
            // Now find the child that holds the image
            ShopIconImage shopImage = newButton.GetComponentInChildren<ShopIconImage>();
            if (shopImage != null)
            {
                    Sprite sprite = purchasableItem.prefab.GetComponent<SpriteRenderer>()?.sprite;
                    if (sprite != null)
                        shopImage.image.sprite = sprite;
                
            }

            Debug.Log("Loaded " + type + ": " + purchasableItem.prefab.name);
        }
    }

}

