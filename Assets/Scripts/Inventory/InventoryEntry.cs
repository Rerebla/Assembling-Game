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
    public void OnClick() {
        print(ID + ammount + "In Inventory");
        Instantiate(GO, new Vector3(0, 0, 3), Quaternion.identity);
        ammount -= 1;
        UpdateValues();
        if (ammount <= 0) {
            InventoryManager.instance.RemoveFromDictionary(GO);
            Destroy(gameObject);
        }
    }
}
