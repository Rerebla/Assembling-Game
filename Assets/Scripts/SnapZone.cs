using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapZone : MonoBehaviour {
    public List<string> acceptableIDs = new List<string>();
    private void OnTriggerEnter(Collider otherCollider) {
        Parts parts = otherCollider.GetComponent<Parts>();
        if (parts == null) {
            return;
        }
        if (acceptableIDs.Contains(parts.ID)) {
            string snappedID = parts.ID;
            //TODO: Delete the GO and make a new one at the snap zones center.
        }
    }
}
