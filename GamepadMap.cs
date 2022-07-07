using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GamepadMap_DataType
{
    /// <summary>
    /// gamepadAndroid itu gamepad bluetooth biasa dengan 2 joystick, sedangkan VRBox gamepad yang dipegang cuma pakai 1 tangan hanya dengan 1 joystick
    /// </summary>
    public enum GamepadType
    {
        gamepadAndroid, VRBox
    }
}

public class GamepadMap : MonoBehaviour
{
    public static GamepadMap_DataType.GamepadType usedGamepadType;

    //=============================================start
    public static bool activatePanel1 = false;
    public static bool activatePanel2 = false;
    public static bool activatePanel3 = false;

    public static bool trigger = false;
    public static bool draw = false;

    public static bool dirUp = false;
    public static bool dirLeft = false;
    public static bool dirRight = false;
    public static bool dirDown = false;

    public static Vector2 leftStick = Vector2.zero;
    //=============================================end

    protected void OnEnable()
    {
        InputSystem.EnableDevice(Gamepad.current);
    }

    protected void OnDisable()
    {
        InputSystem.DisableDevice(Gamepad.current);
    }

    // Start is called before the first frame update
    void Start()
    {
        usedGamepadType = (GamepadMap_DataType.GamepadType)PlayerPrefs.GetInt("usedGamepadType", 0);
    }

    // Update is called once per frame
    void Update()
    {
        var currentData = Gamepad.current;

        leftStick = currentData.leftStick.ReadValue();

        if (usedGamepadType == GamepadMap_DataType.GamepadType.gamepadAndroid)
        {
            activatePanel1 = currentData.xButton.wasPressedThisFrame;
            activatePanel2 = currentData.bButton.wasPressedThisFrame;
            activatePanel3 = currentData.aButton.wasPressedThisFrame;

            trigger = currentData.rightTrigger.wasReleasedThisFrame;
            draw = currentData.leftShoulder.wasPressedThisFrame;

            dirUp = currentData.dpad.up.wasPressedThisFrame;
            dirLeft = currentData.dpad.left.wasPressedThisFrame;
            dirRight = currentData.dpad.right.wasPressedThisFrame;
            dirDown = currentData.dpad.down.wasPressedThisFrame;
        }
        else if (usedGamepadType == GamepadMap_DataType.GamepadType.VRBox)
        {
            activatePanel1 = currentData.xButton.wasPressedThisFrame;
            activatePanel2 = currentData.bButton.wasPressedThisFrame;
            activatePanel3 = currentData.yButton.wasPressedThisFrame;

            trigger = currentData.leftShoulder.wasReleasedThisFrame;
            draw = currentData.rightShoulder.wasPressedThisFrame;

            dirUp = currentData.leftStick.up.wasPressedThisFrame;
            dirLeft = currentData.leftStick.left.wasPressedThisFrame;
            dirRight = currentData.leftStick.right.wasPressedThisFrame;
            dirDown = currentData.leftStick.down.wasPressedThisFrame;
        }
    }
}
