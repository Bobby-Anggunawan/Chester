using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SettingPage : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI namaGamepad;
    public TextMeshProUGUI sensorAktif;

    protected void OnEnable()
    {
        InputSystem.EnableDevice(Keyboard.current);
    }

    protected void OnDisable()
    {
        InputSystem.DisableDevice(Keyboard.current);
    }


    void Start()
    {
        var usedSensorType = (DataType_vrCam.SensorType)PlayerPrefs.GetInt("usedVRSensor", 0);
        var usedGamepadType = (GamepadMap_DataType.GamepadType)PlayerPrefs.GetInt("usedGamepadType", 0);

        namaGamepad.SetText(usedGamepadType.ToString());
        sensorAktif.SetText(usedSensorType.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            SceneManager.LoadScene("Home");
        }
    }

    public void setGamepad() {
        var usedGamepadType = (GamepadMap_DataType.GamepadType)PlayerPrefs.GetInt("usedGamepadType", 0);

        if (usedGamepadType == GamepadMap_DataType.GamepadType.gamepadAndroid)
        {
            PlayerPrefs.SetInt("usedGamepadType", 1);
            namaGamepad.SetText(GamepadMap_DataType.GamepadType.VRBox.ToString());
        }
        else
        {
            PlayerPrefs.SetInt("usedGamepadType", 0);
            namaGamepad.SetText(GamepadMap_DataType.GamepadType.gamepadAndroid.ToString());
        }
    }

    public void setSensorVR() {
        var usedVRSensor = (DataType_vrCam.SensorType)PlayerPrefs.GetInt("usedVRSensor", 0);

        if (usedVRSensor == DataType_vrCam.SensorType.gyro)
        {
            PlayerPrefs.SetInt("usedVRSensor", 1);
            sensorAktif.SetText(DataType_vrCam.SensorType.accel.ToString());
        }
        else
        {
            PlayerPrefs.SetInt("usedVRSensor", 0);
            sensorAktif.SetText(DataType_vrCam.SensorType.gyro.ToString());
        }
    }
}
