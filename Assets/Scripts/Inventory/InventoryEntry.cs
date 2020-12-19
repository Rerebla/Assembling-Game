using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEntry : MonoBehaviour {
    public Sprite displayImg;
    public string displayName;
    public float ammount;
    public TMP_Text textMeshPro;
    public Image image;
    public Button button;
    public string ID;
    public GameObject GO;
    public void UpdateValues() {
        for (int i = 0; i < 5; i++) {
            textMeshPro.text = displayName + ammount.ToString();
            image.sprite = displayImg;
        }
    }
    //TODO: Implement spawning Location choosing
    public void OnClick() {
        print(ID + ammount + "In Inventory");
        Vector3 spawningLocation = new Vector3(Random.Range(-4.2f, 4.2f), Random.Range(1.15f, 4f), Random.Range(-4.5f, 3));
        Instantiate(GO, spawningLocation, Quaternion.identity);
        ammount -= 1;
        UpdateValues();
        if (ammount <= 0) {
            InventoryManager.instance.RemoveFromDictionary(GO);
            Destroy(gameObject);
        }
    }
}
