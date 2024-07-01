using UnityEngine;
using UnityEngine.InputSystem;

public class MyPlayerScript : MonoBehaviour
{
    void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
            Debug.Log("No GamePad Detected");

            return; // No gamepad connected.

        if (gamepad.rightTrigger.wasPressedThisFrame)
        {
            // 'Use' code here
            Debug.Log("RightTrigger Pressed");
        }

        Vector2 move = gamepad.leftStick.ReadValue();
        // 'Move' code here
            Debug.Log("Left Stick Pressed");

    }
}
