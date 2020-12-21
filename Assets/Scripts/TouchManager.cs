using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {
    public static TouchManager instance;
    private void Awake() {
        Singleton();
    }
    private void Singleton() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("Only one TouchManager per scene!");
            Destroy(this);
        }
    }
    private bool selectMode = false;
    public GameObject joystickLeft;
    public GameObject joystickRight;
    public void ToggleSelectModeButton() {
        ToggleSelectMode(true, false);
    }
    public void ToggleSelectMode(bool toggle, bool value) {
        if (toggle) {
            if (!selectMode) {
                selectMode = true;
                joystickLeft.SetActive(false);
                joystickRight.SetActive(false);
            } else {
                selectMode = false;
                joystickLeft.SetActive(true);
                joystickRight.SetActive(true);
            }
        } else {
            selectMode = value;
            joystickLeft.SetActive(!value);
            joystickRight.SetActive(!value);
        }

    }
    public LayerMask layerMask;
    public LayerMask defaultMask;
    public GameObject selectedGO;
    private void Update() {
        if (selectMode && Input.GetMouseButtonDown(0)) {
            SelectMovingGO();
        }
    }
    private void SelectMovingGO() {
        selectedGO = GetGameObject();
        if (selectedGO != null) {
            ToggleSelectMode(false, false);
            ObjectMoving.instance.movingGO = selectedGO;
        }
    }
    public Vector3 GetPosition() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask | defaultMask)) {
            return hit.point;
        }
        return Vector3.zero;
    }
    public Vector3 GetPosition(LayerMask providedLayer) {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, providedLayer)) {
            return hit.point;
        }
        return Vector3.zero;
    }
    public GameObject GetGameObject() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
            return hit.collider.gameObject;
        }
        return null;
    }
}
