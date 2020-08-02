using System.Collections.Generic;
using UnityEngine;

public class SnapZone : MonoBehaviour {
    public List<string> acceptableIDs = new List<string>();
    private void OnTriggerEnter(Collider otherCollider) {
        Parts parts = otherCollider.GetComponent<Parts>();
        Parts parentParts = GetComponentInParent<Parts>();
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
