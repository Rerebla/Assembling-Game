using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour {
    public bool collisionHandled = false;
    public bool isChild = true;
    public float orderID;
    public bool isUnlocked = false;
    public Sprite shopImage;
    public bool isBuyable;
    public float price;
    public string ID;
    public string GetName() => this.name;
    public List<RecipeEntry> recipes = new List<RecipeEntry>();
    [HideInInspector] public List<RecipeEntry> internalRecipes = new List<RecipeEntry>();

    [HideInInspector] public List<string> internalItems = new List<string>();
    [HideInInspector] public List<GameObject> internalItemGameObjects = new List<GameObject>();

    public void AddedPart(GameObject addedpart) {
        internalItems.Add(addedpart.GetComponent<Parts>().ID);
    }
    private void Awake() => gameObject.layer = 8;
    private void Start() {
        Collider collider = gameObject.GetComponent<Collider>();
        collider.enabled = true;
        List<RecipeEntry> tempRecipeList = new List<RecipeEntry>();
        tempRecipeList = RecipeManager.instance.recipes.GetRecipeEntry(prefab);
        recipes.AddRange(tempRecipeList);
        internalRecipes.AddRange(recipes);
        // foreach (RecipeEntry entry in tempRecipeList) { recipes.Add(entry); }
        if (transform.parent == null) {
            isChild = false;
        }
    }

    public GameObject prefab;
}
