using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoving : MonoBehaviour {
    public static ObjectMoving instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("Only one ObjectMover per scene!");
            Destroy(this);
        }
    }
    public float magnitude;
    public FixedJoystick fixedJoystickLeft;
    public FixedJoystick fixedJoystickRight;
    public GameObject movingGO;
    void Update() {
        if (movingGO != null) {
            movingGO.transform.Translate(fixedJoystickLeft.Direction.x * magnitude * Time.deltaTime, fixedJoystickRight.Direction.y * magnitude * Time.deltaTime, fixedJoystickLeft.Direction.y * magnitude * Time.deltaTime);
            movingGO.transform.Rotate(0, fixedJoystickRight.Direction.x, 0);
            Debug.Log(movingGO + "in Update");
        }
    }
}
