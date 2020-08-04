using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parts : MonoBehaviour {
    public bool isUnlocked = false;
    public Sprite shopImage;
    public bool isBuyable;
    public float price;
    public bool isBasePart;
    public bool isFinalPart;
    // public List<GameObject> snapZones = new List<GameObject>();
    public string ID;
    public string GetName() => this.name;
    private int counter;
    public int totalIngredients;
    public GameObject output;
    public void AddedPart() {
        counter++;
        if (counter >= totalIngredients) {
            Instantiate(output, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    private void Awake() {
        gameObject.layer = 8;
    }

    public GameObject prefab;
}
