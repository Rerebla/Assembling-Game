using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeManager : MonoBehaviour {
    public static RecipeManager instance;
    private void Awake() {
        Singleton();
    }
    private void Singleton() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("Only one RecipeManager per Scene");
            Destroy(gameObject);
        }
    }

    [SerializeField]
    public RecipeContainer recipes;
}

[System.Serializable]
public class RecipeContainer {
    [SerializeField]
    private List<RecipeEntry> InternalRecipes = new List<RecipeEntry>();
    public List<RecipeEntry> GetRecipeEntry(GameObject key) {
        return InternalRecipes.Where(r => r.basePart.GetComponent<Parts>().ID == key.GetComponent<Parts>().ID).ToList();
    }
}

[System.Serializable]
public class RecipeEntry {
    public List<string> GetIDsOfIngredients() {
        List<string> IDList = new List<string>();
        foreach (GameObject gameObject in ingredients) {
            IDList.Add(gameObject.GetComponent<Parts>().ID);
        }
        return IDList;
    }
    public GameObject basePart;
    public GameObject resultPart;
    public List<GameObject> ingredients;
}
