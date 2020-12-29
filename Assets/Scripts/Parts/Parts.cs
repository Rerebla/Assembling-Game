using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parts : MonoBehaviour {
    public bool collisionHandled = false;
    public bool isChild = true;
    public float orderID;
    public bool isUnlocked = false;
    public Sprite shopImage;
    public bool isBuyable;
    public float price;

    //* Deprecated
    public bool isBasePart;

    //* Deprecated
    public bool isFinalPart;
    // public List<GameObject> snapZones = new List<GameObject>();
    public string ID;
    public string GetName() => this.name;

    //* Deprecated
    private int counter;

    //* Deprecated
    public int totalIngredients;

    //* Deprecated
    public GameObject output;
    public List<RecipeEntry> recipes = new List<RecipeEntry>();
    [HideInInspector] public List<RecipeEntry> internalRecipes = new List<RecipeEntry>();

    [HideInInspector] public List<string> internalItems = new List<string>();
    [HideInInspector] public List<GameObject> internalItemGameObjects = new List<GameObject>();

    //* Deprecated
    public void AddedPart() {
        counter++;
        if (counter >= totalIngredients) {
            Instantiate(output, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    public void AddedPartNew(GameObject addedpart) {
        internalItems.Add(addedpart.GetComponent<Parts>().ID);
    }
    public void AddGameObject(GameObject gameObj) {
        internalItemGameObjects.Add(gameObj);
    }
    private void Awake() {
        // if (gameObject.GetComponent<Collider>().enabled) {
        //     Debug.LogWarning("All prefabs should contain disabled colliders");
        // }
        //Sets the layer to "MovablePart" layer
        gameObject.layer = 8;

    }
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
