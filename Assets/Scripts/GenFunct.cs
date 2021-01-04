using System.Collections.Generic;
using UnityEngine;

public class GenFunct : MonoBehaviour {
    public static GenFunct instance;
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
    public Dictionary<string, int> ListToDictionary(List<string> list) {
        Dictionary<string, int> dict = new Dictionary<string, int>();
        foreach (var item in list) {
            if (dict.TryGetValue(item, out int ammount)) {
                dict[item] += 1;
            } else {
                dict.Add(item, 1);
            }
        }
        return dict;
    }
}
