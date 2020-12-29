using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnapZone : MonoBehaviour {
    //! One collider could trigger two collisions and then get processed as if it were two different parts. In essence one collider in, possibly two gameObjects out. 
    public List<string> acceptableIDs = new List<string>();
    Parts parentParts;
    private void Awake() {
        gameObject.layer = 9;
    }
    bool finishedStuff = false;
    private void Start() {
        parentParts = GetComponentInParent<Parts>();
        CheckIfAcceptableIDs();
        gameObject.layer = 9;
        finishedStuff = true;
    }
    private IEnumerator WaitForRandom(Collider otherCollider) {
        yield return new WaitForSeconds(Random.Range(0.00001f, 0.0001f));
        lock(isColliding) {
            NewAddPart(otherCollider);
        }
    }
    private void OnTriggerEnter(Collider otherCollider) {
        StartCoroutine(WaitForRandom(otherCollider));
        // OldAddPart(otherCollider);

    }
    //Recipe List - AddedItems = "What i need"
    private void CheckIfAcceptableIDs() {
        if (acceptableIDs.Count <= 0) {
            Debug.LogWarning(this + "in" + parentParts.ID + "does not have any acceptable ID");
        }
    }
    public static object isColliding = new object();
    private void NewAddPart(Collider otherCollider) {
        if (!finishedStuff) {
            return;
        }
        SnapZone otherSnapZone = otherCollider.gameObject.GetComponent<SnapZone>();
        if (otherSnapZone) {
            return;
        }
        Parts otherParts = otherCollider.gameObject.GetComponent<Parts>();
        if (!otherParts) {
            return;
        }
        if (otherParts.isChild) {
            return;
        }
        if (acceptableIDs.Count <= 0) {
            Debug.LogWarning(this + "in" + parentParts.ID + "does not have any acceptable ID");
            return;
        }
        if (!acceptableIDs.Contains(otherParts.ID)) {
            return;
        }
        if (parentParts.internalItemGameObjects.Contains(otherCollider.gameObject)) {
            Debug.Log("Collided with child");
            return;
        }
        Debug.Log(Time.time);

        List<RecipeEntry> validEntries = new List<RecipeEntry>();
        foreach (RecipeEntry entry in parentParts.recipes) {
            //Loops over the items in the parent and doesn't add the entries that don't contain them to the validEntries list
            if (parentParts.internalItems.All(elem => entry.ingredients.Select(ent => ent.gameObject.GetComponent<Parts>().ID).Contains(elem))) {
                validEntries.Add(entry);
            }
        }
        List<string> neededPartsIDs = new List<string>();
        foreach (RecipeEntry validEntry in validEntries) {
            List<string> validIDs = new List<string>();
            validIDs.AddRange(validEntry.ingredients.Select(ingredient => ingredient.GetComponent<Parts>().ID));
            List<string> itemIDs = new List<string>();
            itemIDs.AddRange(parentParts.internalItems);
            List<string> tempIDs = new List<string>();
            tempIDs.AddRange(itemIDs);
            tempIDs.Add(otherParts.ID);
            List<string> tempIngredientIDs = new List<string>();
            tempIngredientIDs.AddRange(validEntry.ingredients.Select(ingredient => ingredient.GetComponent<Parts>().ID));
            tempIDs.Sort((x, y) => string.Compare(x, y));
            tempIngredientIDs.Sort((x, y) => string.Compare(x, y));
            //!Potential bug hazard. If the addedIDs and the recipeIDs are in the same order and have the same count, but do not contain exactly the same stuff. 
            //!If there are two required components compA and one compB, it would trigger both if there are two compB and one compA and vice versa (I think).
            if (tempIDs.SequenceEqual(tempIngredientIDs) && tempIDs.Count == tempIngredientIDs.Count) {
                //!Complete Recipe found
                Debug.Log(validEntry.resultPart + "is complete");
                GeneralFunctionManager.instance.SpawnWithCollider(validEntry.resultPart, transform.parent.transform.position, Quaternion.identity);
                Destroy(otherCollider.gameObject);
                Destroy(transform.parent.gameObject);
                return;
            }
            // neededPartsIDs.AddRange(validIDs.Except(itemIDs));
            List<string> removedIDs = new List<string>();
            foreach (string validID in validIDs) {
                if (itemIDs.Contains(validID)) {
                    //sanitized validIDS
                    // validIDs.Remove(validID);
                    removedIDs.Add(validID);
                    itemIDs.Remove(validID);
                }
            }
            foreach (string removedID in removedIDs) {
                validIDs.Remove(removedID);
            }
            neededPartsIDs.AddRange(validIDs);
        }
        // foreach (RecipeEntry validEntry in validEntries) {
        //     foreach (GameObject ingredient in validEntry.ingredients) {
        //         if (parentParts.internalItems.Select(item => item.GetComponent<Parts>().ID).ToList().FindAll((ingredient.GetComponent<Parts>().ID) >= 2)) {

        //         }
        //     }
        // }
        if (neededPartsIDs.Contains(otherParts.ID)) {
            parentParts.AddedPartNew(otherParts.prefab);
            GameObject instantiantedObj = Instantiate(otherParts.prefab, transform.position, Quaternion.identity, transform.parent);
            instantiantedObj.GetComponent<Parts>().isChild = true;
            Destroy(instantiantedObj.GetComponent<Collider>());
            foreach (SnapZone snap in instantiantedObj.GetComponentsInChildren<SnapZone>()) { Destroy(snap); }
            instantiantedObj.layer = 9;
            Destroy(otherCollider.gameObject);
            parentParts.AddGameObject(instantiantedObj);
            Destroy(gameObject);
        }
    }
    private void OldAddPart(Collider otherCollider) {
        Parts parts = otherCollider.gameObject.GetComponent<Parts>();

        if (parts == null) {
            return;
        }
        if (acceptableIDs.Contains(parts.ID)) {
            Instantiate(parts.prefab, transform.position, Quaternion.identity, transform.parent);
            Destroy(otherCollider.gameObject);
            parentParts.AddedPart();
            Destroy(gameObject);
        }
    }
}
