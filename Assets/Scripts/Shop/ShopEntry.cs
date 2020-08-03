using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopEntry : MonoBehaviour {
    public Sprite displayImg;
    public string displayName;
    public int price;
    public TMP_Text textMeshPro;
    public Image image;
    public Button button;
    public void SetValues() {
        textMeshPro.text = displayName + price.ToString();
        image.sprite = displayImg;
    }
    // private void Update() {
    //     textMeshPro.text = displayName + price.ToString();
    //     image.sprite = displayImg;
    // }
}
