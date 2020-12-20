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
    public GameObject Prefab;
    public void SetValues(Sprite DisplayImg, string DisplayName, float Ammount, string IDPrefab, GameObject GOPrefab) {
        displayImg = DisplayImg;
        displayName = DisplayName;
        ammount = Ammount;
        ID = IDPrefab;
        Prefab = GOPrefab;
        UpdateValues();
    }
    public void UpdateValues() {
        for (int i = 0; i < 5; i++) {
            textMeshPro.text = displayName + ammount.ToString();
            image.sprite = displayImg;
        }
    }
    private void OnClick() {
        InventoryManager.instance.SpawnInventoryEntry(Prefab);
    }
}
