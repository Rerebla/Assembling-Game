using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("Only one InvetoryManager!");
            Destroy(this);
        }
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
}
