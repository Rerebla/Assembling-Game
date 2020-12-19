﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour {
    public static ShopManager instance;
    public List<GameObject> Prefabs = new List<GameObject>();
    public List<GameObject> unlockedPrefabs = new List<GameObject>();
    public GameObject shopEntry;
    public GameObject shopEntryParent;
    public GameObject scrollView;
    // TODO: Display money in Shop view.
    public float money;
    public TMP_Text moneyText;
    public UnityEngine.Object[] subListObjects;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else { Destroy(this); }

        subListObjects = Resources.LoadAll("ObjectPrefabs", typeof(GameObject));

        ToggleShop(false, false);
    }
    private void Start() {
        moneyText.text = money.ToString() + '$';
        List<UnityEngine.GameObject> subListGameObjects = new List<GameObject>();
        // foreach (GameObject i in Prefabs) { print(i.GetComponent<Parts>().ID); }

        subListObjects = Resources.LoadAll("ObjectPrefabs", typeof(GameObject));
        foreach (UnityEngine.Object UnityObject in subListObjects) { subListGameObjects.Add((GameObject) UnityObject); }
        List<UnityEngine.GameObject> sortedListObjects = subListGameObjects.OrderBy(o => o.GetComponent<Parts>().orderID).ToList();
        foreach (GameObject gameObject in sortedListObjects) {
            if (gameObject.GetComponent<Parts>().isUnlocked) {
                unlockedPrefabs.Add(gameObject);
                Parts parts = gameObject.GetComponent<Parts>();
                InstantiateShopEntry(parts.shopImage, parts.name, parts.price, parts.ID, parts.gameObject);
                // print(gameObject);
            }
        }
    }
    private void InstantiateShopEntry(Sprite displayImg, string displayName, float price, string ID, GameObject gameObject1) {
        GameObject shopEntryInstance = Instantiate(shopEntry, shopEntryParent.transform);
        ShopEntry shopEntryScript = shopEntryInstance.GetComponent<ShopEntry>();
        shopEntryScript.displayImg = displayImg;
        shopEntryScript.displayName = displayName;
        shopEntryScript.price = price;
        shopEntryScript.ID = ID;
        shopEntryScript.GO = gameObject1;
        shopEntryScript.SetValues();
    }
    private bool isShopEnabled = false;
    public void ToggleShopButton() {
        ToggleShop(true, false);
    }
    public void ToggleShop(bool fromButton, bool value) {
        if (fromButton) {
            if (isShopEnabled) {
                isShopEnabled = false;
                scrollView.SetActive(false);
            } else {
                isShopEnabled = true;
                scrollView.SetActive(true);
            }
        } else {
            scrollView.SetActive(value);
        }
    }
    public void Buy(float price) {
        money -= price;
        moneyText.text = money.ToString() + '$';
    }
}
