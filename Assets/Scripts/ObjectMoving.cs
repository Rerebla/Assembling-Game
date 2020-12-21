using UnityEngine;

public class ObjectMoving : MonoBehaviour {
    public static ObjectMoving instance;
    private void Awake() {
        Singleton();
    }
    private void Singleton() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("Only one ObjectMover per scene!");
            Destroy(this);
        }
    }

    [SerializeField]
    float magnitude = 1;
    [SerializeField]
    FixedJoystick fixedJoystickLeft;
    [SerializeField]
    FixedJoystick fixedJoystickRight;
    [HideInInspector]
    public GameObject movingGO;
    private void Update() {
        if (movingGO != null) {
            float movingModifier = magnitude * Time.deltaTime;
            Vector3 movement = new Vector3(fixedJoystickLeft.Direction.x * movingModifier, fixedJoystickRight.Direction.y * movingModifier, fixedJoystickLeft.Direction.y * movingModifier);
            movingGO.transform.Translate(movement, Space.World);
            movingGO.transform.Rotate(0, fixedJoystickRight.Direction.x, 0);
        }
    }
}
