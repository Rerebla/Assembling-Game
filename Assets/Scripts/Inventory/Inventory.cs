using UnityEngine;

public class Inventory : MonoBehaviour {
    public static Inventory instance;
    private void Awake() {
        Singleton();
    }
    private void Singleton() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("Only one Inventory per scene!");
            Destroy(this);
        }
    }
    public System.Action OnEnabled;
    public System.Action OnDisabled;
    private void OnEnable() {
        OnEnabled?.Invoke();
        Debug.Log("Inventory is definetly enabled");
    }
    private void OnDisable() {
        OnDisabled?.Invoke();
        Debug.Log("Inventory is defindetly disabled");
    }
}
