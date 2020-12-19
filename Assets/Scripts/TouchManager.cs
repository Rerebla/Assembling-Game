using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {
    private bool selectMode = false;
    public GameObject joystickLeft;
    public GameObject joystickRight;
    public void ToggleSelectModeButton() {
        ToogleSelectMode(true, false);
    }
    public void ToogleSelectMode(bool toggle, bool value) {
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
        if (Input.GetMouseButtonDown(0)) {
            if (selectMode) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
                    selectedGO = hit.collider.gameObject;
                    ToogleSelectMode(false, false);
                    Debug.Log(selectedGO);
                    ObjectMoving.instance.movingGO = selectedGO;
                }
            }
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
    public GameObject GetGameObject() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
            return hit.collider.gameObject;
        }
        return null;
    }
}
