using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopEntry : MonoBehaviour {
    public Sprite displayImg;
    public string displayName;
    public float price;
    public TMP_Text textMeshPro;
    public Image image;
    public Button button;
    public string ID;
    public GameObject GO;
    public void SetValues() {
        // while (textMeshPro.text == "Placeholder €10") {
        textMeshPro.text = displayName + price.ToString();
        image.sprite = displayImg;
        // }

    }
    //TODO: Possibly add field or other method to control how much you want to buy at a time.
    public void OnClick() {
        if (ShopManager.instance.money >= price) {
            InventoryManager.instance.AddToDictionary(GO, 1);
        }
        print("clickedLOL!");
    }
}
