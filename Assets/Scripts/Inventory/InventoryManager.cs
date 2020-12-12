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
        GameObject inventoryEntryGo = Instantiate(inventoryEntry, inventoryEntryParent.transform);
        InventoryEntry inventoryEntryScript = inventoryEntryGo.GetComponent<InventoryEntry>();
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
    public void Update() {
        spawnLocation = GetLocation();
        if (shouldSpawn && Input.GetMouseButtonDown(0) && spawnLocation != Vector3.zero) {
            shouldSpawn = false;
            spawnLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
            Debug.LogWarning(GetInstanceID());
            InventoryEntry ItemInventoryEntry = item.GetComponent<InventoryEntry>();
            Instantiate(ItemInventoryEntry.GO, spawnLocation, Quaternion.identity);
            ItemInventoryEntry.ammount -= 1;
            ItemInventoryEntry.UpdateValues();
            if (ItemInventoryEntry.ammount <= 0) {
                RemoveFromDictionary(ItemInventoryEntry.GO);
                Destroy(item);
            }
        }
    }
    public void SetValuesFromInventoryEntry() {

    }
    public void StartSpawningItem(GameObject passedItem) {
        Debug.LogWarning(GetInstanceID() + "SPAWN");
        item = passedItem;
        shouldSpawn = true;
        // StartCoroutine(Spawn(item));
    }
    private Vector3 spawnLocation = new Vector3();
    private bool shouldSpawn = false;
    private GameObject _item;
    private GameObject item {
        get {
            return _item;
        }
        set {
            Debug.LogWarning(value);
            _item = value;
        }
    }
    IEnumerator Spawn(GameObject item) {
        yield return StartCoroutine(SetLocation());
        spawnLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
        InventoryEntry ItemInventoryEntry = item.GetComponent<InventoryEntry>();
        Instantiate(ItemInventoryEntry.GO, spawnLocation, Quaternion.identity);
        ItemInventoryEntry.ammount -= 1;
        ItemInventoryEntry.UpdateValues();
        if (ItemInventoryEntry.ammount <= 0) {
            RemoveFromDictionary(ItemInventoryEntry.GO);
            Destroy(item);
        }
    }
    IEnumerator SetLocation() {
        spawnLocation = TouchManager.instance.GetPosition();
        yield return null;
    }
    private Vector3 GetLocation() {
        return TouchManager.instance.GetPosition();
    }
}
