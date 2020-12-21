using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeManager : MonoBehaviour {
    [SerializeField]
    public RecipeContainer recipes;
}

[System.Serializable]
public class RecipeContainer {
    [SerializeField]
    public List<RecipeEntry> recipes = new List<RecipeEntry>();
    public List<RecipeEntry> GetRecipeEntry(GameObject key) {
        return recipes.Where(r => r.basePart == key).ToList();
    }
}

[System.Serializable]
public class RecipeEntry {
    public GameObject basePart;
    public GameObject resultPart;
    public List<GameObject> ingredients;
}
