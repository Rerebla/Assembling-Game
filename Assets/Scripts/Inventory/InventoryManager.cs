using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public GameObject inventoryEntryParent;
    public GameObject inventoryEntry;
    public GameObject scrollView;
    public static InventoryManager instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("Only one InvetoryManager!");
            Destroy(this);
        }
    }
    private void Start() {
        ToggleInventory(false, false);
        Inventory.instance.OnEnabled += InventoryEnabled;
        Inventory.instance.OnDisabled += InventoryDisabled;
    }
    public Dictionary<GameObject, float> Items = new Dictionary<GameObject, float>();
    public void AddToDictionary(GameObject gameObject, float ammount) {
        if (Items.TryGetValue(gameObject, out float internalAmmount)) {
            Items[gameObject] += ammount;
        } else {
            Items.Add(gameObject, ammount);
        }
        foreach (KeyValuePair<GameObject, float> i in Items) {
            print(i.Key.GetComponent<Parts>().ID + i.Value);
        }
    }
    public void RemoveFromDictionary(GameObject gameObject) {
        Items.Remove(gameObject);
    }

    private void InventoryEnabled() {
        print("Enabled! InventroyManager");
        foreach (KeyValuePair<GameObject, float> keyValuePair in Items) {
            Parts parts = keyValuePair.Key.GetComponent<Parts>();
            InstantiateInventoryEntry(parts.shopImage, parts.name, keyValuePair.Value, parts.ID, parts.gameObject);
        }
    }
    private void InstantiateInventoryEntry(Sprite displayImg, string displayName, float ammount, string ID, GameObject gameObject1) {
        Instantiate(inventoryEntry, inventoryEntryParent.transform);
        InventoryEntry inventoryEntryScript = inventoryEntry.GetComponent<InventoryEntry>();
        inventoryEntryScript.displayImg = displayImg;
        inventoryEntryScript.displayName = displayName;
        inventoryEntryScript.ammount = ammount;
        inventoryEntryScript.ID = ID;
        inventoryEntryScript.GO = gameObject1;
        inventoryEntryScript.UpdateValues();
    }
    private void InventoryDisabled() {
        foreach (Transform child in inventoryEntryParent.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }
    private bool isInvetoryEnabled = false;
    public void ToggleInventoryButton() {
        ToggleInventory(true, false);
    }
    public void ToggleInventory(bool fromButton, bool value) {
        for (int i = 0; i <= 2; i++) {
            if (fromButton) {
                if (isInvetoryEnabled) {
                    isInvetoryEnabled = false;
                    scrollView.SetActive(false);
                } else {
                    isInvetoryEnabled = true;
                    scrollView.SetActive(true);
                }
            } else {
                scrollView.SetActive(value);
            }
        }
    }
}
