using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour {
    public static ShopManager instance;
    public List<GameObject> Prefabs = new List<GameObject>();
    /// <summary>Unsure why I implemented that. I assume I prepared that for saving.</summary>
    public List<GameObject> unlockedPrefabs = new List<GameObject>();
    public GameObject shopEntry;
    /// <summary>Named content in Unity</summary>
    public GameObject shopEntryParent;
    /// <summary>Named Shop in Unity</summary>
    public GameObject scrollView;
    /// <summary>Named Viewport in Unity</summary>
    public GameObject viewPort;
    // TODO: Display money in Shop view.
    /// <summary>The ammount of money the player currently owns.</summary>
    public float money;
    /// <summary>The text that is displayed in the shop-view.</summary>
    TMP_Text moneyText;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else { Destroy(this); }
        ToggleShop(false, false);
    }
    ///<summary>
    ///Loads the items from Assets/Rescources/ObjectPrefabs.Currently using a weird method to load them.
    /// Sometimes doesn't work on the first start of the game in the Editor.
    ///</summary>
    private void LoadItemsFromFile() {
        Object[] subListObjects = Resources.LoadAll("ObjectPrefabs", typeof(GameObject));
        foreach (GameObject gameObject in subListObjects) { Prefabs.Add(gameObject); }
    }
    private void Start() {
        LoadItemsIntoShopView();
        //Gets the moneyTextComponent and sets it to the current money ammount
        moneyText = viewPort.GetComponentInChildren<TMP_Text>();
        moneyText.text = money.ToString() + '$';
    }
    /// <summary>a</summary>
    private void LoadItemsIntoShopView() {
        foreach (GameObject gameObject in ShopManager.instance.Prefabs) {
            if (gameObject.GetComponent<Parts>().isUnlocked) {
                Debug.LogWarning(gameObject);
                unlockedPrefabs.Add(gameObject);
                Parts parts = gameObject.GetComponent<Parts>();
                InstantiateShopEntry(parts.shopImage, parts.name, parts.price, parts.ID, parts.gameObject);
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
