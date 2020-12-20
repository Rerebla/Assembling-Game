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
    private void Update() {
        SpawnUpdateLoop();
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
    private void InstantiateInventoryEntry(Sprite displayImg, string displayName, float ammount, string ID, GameObject GO) {
        GameObject inventoryEntryInstance = Instantiate(inventoryEntry, inventoryEntryParent.transform);
        InventoryEntry inventoryEntryScript = inventoryEntryInstance.GetComponent<InventoryEntry>();
        inventoryEntryScript.SetValues(displayImg, displayName, ammount, ID, GO);
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
        // for (int i = 0; i <= 2; i++) {
        if (fromButton) {
            if (isInvetoryEnabled) {
                isInvetoryEnabled = false;
                scrollView.SetActive(false);
            } else {
                isInvetoryEnabled = true;
                scrollView.SetActive(true);
            }
        } else {
            isInvetoryEnabled = value;
            scrollView.SetActive(value);
        }
        // }
    }

    [SerializeField]
    private LayerMask layerMask;
    private bool shouldSpawn = false;
    private GameObject InventoryEntryGameObjectPrefab;
    private void SpawnUpdateLoop() {
        if (shouldSpawn && Input.GetMouseButtonDown(0)) {
            Vector3 position = TouchManager.instance.GetPosition(layerMask);
            if (position != Vector3.zero) {
                shouldSpawn = false;
                InstantiateInventoryEntry(position);
            }
        }
    }
    private void InstantiateInventoryEntry(Vector3 position) {
        position = new Vector3(position.x, position.y + 1, position.z);
        Instantiate(InventoryEntryGameObjectPrefab, position, Quaternion.identity);
        Items[InventoryEntryGameObjectPrefab] -= 1;
        if (Items[InventoryEntryGameObjectPrefab] <= 0) {
            RemoveFromDictionary(InventoryEntryGameObjectPrefab);
        }
        InventoryEntryGameObjectPrefab = null;
    }

    public void SpawnInventoryEntry(GameObject gameObject) {
        ToggleInventory(false, false);
        InventoryEntryGameObjectPrefab = gameObject;
        shouldSpawn = true;
    }
}
