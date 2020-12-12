using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {
    public static TouchManager instance;
    private void Awake() {
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
    public LayerMask defaultLayerMask;
    public LayerMask movablePartsLayerMask;
    public GameObject selectedGO;
    // private void Update() {
    //     if (Input.GetMouseButtonDown(0)) {
    //         if (selectMode) {
    //             RaycastHit hit;
    //             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //             if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
    //                 selectedGO = hit.collider.gameObject;
    //                 ToogleSelectMode(false, false);
    //                 Debug.Log(selectedGO);
    //                 ObjectMoving.instance.movingGO = selectedGO;
    //             }
    //         }
    //     }
    // }
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (selectMode) {
                selectedGO = GetGameObject(movablePartsLayerMask);
                if (selectedGO != null) {
                    this.ToogleSelectMode(false, false);
                    Debug.Log(selectedGO);
                    ObjectMoving.instance.movingGO = selectedGO;
                    selectedGO = null;
                }
            }
        }
    }
    ///<summary>
    ///Used to get the Position of the hit. Specify the layer, which you want to hit.
    ///Returns Vector3.zero if not found
    ///</summary>
    public Vector3 GetPosition(LayerMask layer) {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer)) {
            return hit.point;
        } else {
            return Vector3.zero;
        }
    }
    ///<summary>
    ///Used to get the Position of the hit. No layerMask means default will be used (Default = 0)
    ///Returns Vector3.zero if not found
    ///</summary>
    public Vector3 GetPosition() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, defaultLayerMask)) {
            return hit.point;
        } else {
            return Vector3.zero;
        }
    }
    ///<summary>
    ///Used to get the clicked GameObject. Returns null if nothing is found on first click.
    ///Needs a provided layermask. 
    ///</summary>
    public GameObject GetGameObject(LayerMask layer) {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, movablePartsLayerMask)) {
            selectedGO = hit.collider.gameObject;
            return selectedGO;
        } else {
            return null;
        }
    }
    ///<summary>
    ///Used to get the clicked GameObject. Returns null if nothing is found on first click.
    ///Used layer mask is "default" (0).
    ///</summary>
    public GameObject GetGameObject() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, defaultLayerMask)) {
            selectedGO = hit.collider.gameObject;
            return selectedGO;
        } else {
            return null;
        }
    }
}
