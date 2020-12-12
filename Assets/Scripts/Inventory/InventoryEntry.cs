using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEntry : MonoBehaviour {
    public Sprite displayImg;
    public string displayName;
    public float ammount;
    public TMP_Text textInInventoryEntry;
    public Image image;
    public Button button;
    public string ID;
    public GameObject GO;

    ///<summary>
    ///Updates the Values of the InventoryEntry.
    ///</summary>
    public void UpdateValues() {
        //Loop is needed to mitigate an issue, where it wouldn't update when only updating it once
        for (int i = 0; i < 5; i++) {
            textInInventoryEntry.text = displayName + ammount.ToString();
            image.sprite = displayImg;
        }
    }
    private bool shouldSpawn = false;
    //! New Method; currently disabled because of testing purpouses
    // public void OnClick() {
    //     print(ID + ammount + "In Inventory");
    //     InventoryManager.instance.StartSpawningItem(gameObject); //!Hands the task over to the Inventory Manager
    //     InventoryManager.instance.ToggleInventory(false, false);
    // }
    // private void Update() {
    //     if (shouldSpawn) {
    //         StartSpawning();
    //     }
    // }
    public void OnClick() {
        Debug.Log(ID + ammount + "currently in Inventory");
        InventoryManager.instance.ToggleInventory(false, false);
        Spawn();
    }
    //TODO: Move the spawning to the Inventory Manager
    private void StartSpawning() {
        if (Input.GetMouseButtonDown(0)) {
            spawnLocation = TouchManager.instance.GetPosition();
            Spawn();
        }
    }
    private Vector3 spawnLocation = Vector3.zero;
    public void Spawn() {
        //Lets the player pick the spawn location
        //Spawns the Object at the postition the player selected, only moved up a bit in height
        spawnLocation = new Vector3(Random.Range(3f, 5f), Random.Range(3f, 5f), Random.Range(3f, 5f));
        spawnLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z + 4);
        Instantiate(GO, spawnLocation, Quaternion.identity);
        //Reduces the ammount of items in the inventory and Updates the Values to represent the changes
        ammount -= 1;
        UpdateValues();
        //Destroys the inventoryEntry if none are in the Inventory
        if (ammount <= 0) {
            InventoryManager.instance.RemoveFromDictionary(GO);
            Destroy(gameObject);
        }
        shouldSpawn = false;
    }
}
