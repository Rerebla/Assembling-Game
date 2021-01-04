using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnapZone : MonoBehaviour {
    //! One collider could trigger two collisions and then get processed as if it were two different parts. In essence one collider in, two ore more gameObjects out. 
    public List<string> acceptableIDs = new List<string>();
    Parts parentParts;
    private void Awake() => gameObject.layer = 9;
    private void Start() {
        parentParts = GetComponentInParent<Parts>();
        CheckIfAcceptableIDs();
        gameObject.layer = 9;
    }

    private void OnTriggerEnter(Collider otherCollider) => CheckCollision(otherCollider);
    private void CheckIfAcceptableIDs() {
        if (acceptableIDs.Count > 0) { return; }
        Debug.LogWarning(this + "in" + parentParts.ID + "does not have any acceptable ID");
    }
    private bool rudimentaryChecks(SnapZone otherSnapZone, Parts otherParts) {
        if (otherSnapZone || !otherParts || otherParts.isChild || !acceptableIDs.Contains(otherParts.ID)) { return false; }

        if (acceptableIDs.Count <= 0) {
            Debug.LogWarning(this + "in" + parentParts.ID + "does not have any acceptable ID");
            return false;
        }
        return true;
    }
    private void CheckCollision(Collider otherCollider) {
        SnapZone otherSnapZone = otherCollider.gameObject.GetComponent<SnapZone>();
        Parts otherParts = otherCollider.gameObject.GetComponent<Parts>();
        if (!rudimentaryChecks(otherSnapZone, otherParts)) { return; }

        List<RecipeEntry> validEntryList = makeValidEntryList();

        List<string> neededPartsIDList = new List<string>();
        foreach (RecipeEntry validEntry in validEntryList) {
            List<string> validIDList = new List<string>();
            validIDList.AddRange(validEntry.ingredientList.Select(ingredient => ingredient.GetComponent<Parts>().ID));
            List<string> itemIDList = new List<string>();
            itemIDList.AddRange(parentParts.internalItems);
            List<string> sortedIDList = new List<string>();
            sortedIDList.AddRange(itemIDList);
            sortedIDList.Add(otherParts.ID);
            if (validIDList.Count == sortedIDList.Count) {
                checkIfCompleteRecipe(validEntry, sortedIDList, otherCollider, otherParts);
            }
            neededPartsIDList.AddRange(makeNeededPartsIDList(validIDList, itemIDList));
        }
        if (neededPartsIDList.Contains(otherParts.ID) && !otherParts.collisionHandled) {
            addIngredient(otherParts, otherCollider);
        }
    }
    private void checkIfCompleteRecipe(RecipeEntry validEntry, List<string> sortedIDList, Collider otherCollider, Parts otherParts) {
        List<string> sortedIngredientIDList = new List<string>();
        sortedIngredientIDList.AddRange(validEntry.ingredientList.Select(ingredient => ingredient.GetComponent<Parts>().ID));
        sortedIDList.Sort((x, y) => string.Compare(x, y));
        sortedIngredientIDList.Sort((x, y) => string.Compare(x, y));
        //!Probably is fixed.
        //*Potential bug hazard. If the addedIDs and the recipeIDs are in the same order and have the same count, but do not contain exactly the same stuff. 
        //*If there are two required components compA and one compB, it would trigger both if there are two compB and one compA and vice versa (I think).
        //*Can be mitigated with the validID list in the Snap Zone, but is only a workaround. Will have to find a better solution
        bool sequenceEqual = sortedIDList.SequenceEqual(sortedIngredientIDList);
        bool countEqual = sortedIDList.Count == sortedIngredientIDList.Count;
        bool ammountEqual = GenFunct.instance.ListToDictionary(sortedIDList).Except(GenFunct.instance.ListToDictionary(sortedIngredientIDList)).Count() <= 0;
        if (sequenceEqual && countEqual && ammountEqual && !otherParts.collisionHandled) {
            CompleteRecipe(otherParts, validEntry, otherCollider);
        }
    }
    private List<RecipeEntry> makeValidEntryList() {
        List<RecipeEntry> validEntryList = new List<RecipeEntry>();
        foreach (RecipeEntry entry in parentParts.recipes) {
            //Loops over the items in the parent and doesn't add the entries that don't contain them, to the validEntryList
            if (parentParts.internalItems.All(elem => entry.ingredientList.Select(ent => ent.gameObject.GetComponent<Parts>().ID).Contains(elem))) {
                validEntryList.Add(entry);
            }
        }
        return validEntryList;
    }
    private List<string> makeNeededPartsIDList(List<string> validIDList, List<string> itemIDList) {
        List<string> neededPartsIDList = new List<string>();
        List<string> removedIDList = new List<string>();
        foreach (string validID in validIDList) {
            if (itemIDList.Contains(validID)) {
                removedIDList.Add(validID);
                itemIDList.Remove(validID);
            }
        }
        foreach (string removedID in removedIDList) {
            validIDList.Remove(removedID);
        }
        neededPartsIDList.AddRange(validIDList);
        return neededPartsIDList;
    }
    private void CompleteRecipe(Parts otherParts, RecipeEntry validEntry, Collider otherCollider) {
        otherParts.collisionHandled = true;
        //!Complete Recipe found
        Debug.Log(validEntry.resultPart + "is complete");
        GenFunct.instance.SpawnWithCollider(validEntry.resultPart, transform.parent.transform.position, Quaternion.identity);
        Destroy(otherCollider.gameObject);
        Destroy(transform.parent.gameObject);
        return;
    }
    private void addIngredient(Parts otherParts, Collider otherCollider) {
        otherParts.collisionHandled = true;
        parentParts.AddedPart(otherParts.prefab);
        GameObject instantiantedObj = Instantiate(otherParts.prefab, transform.position, Quaternion.identity, transform.parent);
        instantiantedObj.GetComponent<Parts>().isChild = true;
        Destroy(instantiantedObj.GetComponent<Collider>());
        foreach (SnapZone snap in instantiantedObj.GetComponentsInChildren<SnapZone>()) { Destroy(snap.gameObject); }
        instantiantedObj.layer = 9;
        Destroy(otherCollider.gameObject);
        Destroy(gameObject);
    }
}
