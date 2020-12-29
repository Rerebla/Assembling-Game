using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFunctionManager : MonoBehaviour {
    public static GeneralFunctionManager instance;
    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogWarning("Only one GeneralFunctionManager per scene");
            Destroy(gameObject);
        }
    }
    public GameObject SpawnWithCollider(GameObject prefab, Vector3 position, Quaternion quaternion) {
        GameObject instantiatedGO = Instantiate(prefab, position, quaternion);
        instantiatedGO.AddComponent<MeshCollider>();
        return instantiatedGO;
    }
}
