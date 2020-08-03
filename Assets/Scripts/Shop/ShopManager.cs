using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {
    public static ShopManager instance;
    public List<GameObject> Prefabs = new List<GameObject>();
    public List<GameObject> unlockedPrefabs = new List<GameObject>();
    public GameObject shopEntry;
    public GameObject shopEntryParent;
    public GameObject canvas;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else { Destroy(this); }

        Object[] subListObjects = Resources.LoadAll("ObjectPrefabs", typeof(GameObject));
        foreach (GameObject gameObject in subListObjects) { Prefabs.Add(gameObject); }
        ToggleShop(false, false);
    }
    private void Start() {
        foreach (GameObject i in Prefabs) { print(i.GetComponent<Parts>().ID); }
        foreach (GameObject gameObject in ShopManager.instance.Prefabs) {
            if (gameObject.GetComponent<Parts>().isUnlocked) {
                unlockedPrefabs.Add(gameObject);
                Parts parts = gameObject.GetComponent<Parts>();
                Instantiate(shopEntry, shopEntryParent.transform);
                InstantiateShopEntry(parts.shopImage, parts.name, parts.price);
                print(gameObject);
            }
        }
    }
    private void InstantiateShopEntry(Sprite displayImg, string displayName, int price) {
        ShopEntry shopEntryScript = shopEntry.GetComponent<ShopEntry>();
        shopEntryScript.displayImg = displayImg;
        shopEntryScript.displayName = displayName;
        shopEntryScript.price = price;
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
                canvas.SetActive(false);
            } else {
                isShopEnabled = true;
                canvas.SetActive(true);
            }
        } else {
            canvas.SetActive(value);
        }
    }
}
