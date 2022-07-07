using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Unity.XR.MockHMD;
using System;


public class DataType_vrCam
{
    public enum SensorType
    {
        gyro, accel
    }
}

public class vrCam : MonoBehaviour
{
    //public GameObject textMeshPro;
    public bool isVR;

    DataType_vrCam.SensorType usedSensorType;

    private Queue<float> accelerationDataTemp = new Queue<float>();
    private float accelerationZData{
        get {
            float average = 0;
            foreach (float data in accelerationDataTemp) {
                average += data;
            }
            average /= accelerationDataTemp.Count;

            return average;
        }
        set {
            var numStored = 10;
            if (accelerationDataTemp.Count > numStored)
            {
                accelerationDataTemp.Dequeue();
            }
            accelerationDataTemp.Enqueue(value);
        }
    }


    protected void OnEnable()
    {
        // All sensors start out disabled so they have to manually be enabled first.
        InputSystem.EnableDevice(Accelerometer.current);
        if(UnityEngine.InputSystem.Gyroscope.current != null) InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
    }

    protected void OnDisable()
    {
        InputSystem.DisableDevice(Accelerometer.current);
        if (UnityEngine.InputSystem.Gyroscope.current != null) InputSystem.DisableDevice(UnityEngine.InputSystem.Gyroscope.current);
    }

    // Start is called before the first frame update
    void Start()
    {
        usedSensorType = (DataType_vrCam.SensorType)PlayerPrefs.GetInt("usedVRSensor", 0);

        if (isVR)
        {
            XRSettings.enabled = true;
            XRSettings.gameViewRenderMode = GameViewRenderMode.BothEyes;

            //buat layar gabisa mati
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
        else {
            XRSettings.gameViewRenderMode = GameViewRenderMode.LeftEye;
            XRSettings.enabled = false;
            MockHMD.SetRenderMode(MockHMDBuildSettings.RenderMode.SinglePassInstanced);
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isVR)
        {
            UnityEngine.InputSystem.Gyroscope gyro = UnityEngine.InputSystem.Gyroscope.current;
            //kalau gyroscope ada
            if (gyro != null && usedSensorType == DataType_vrCam.SensorType.gyro)
            {
                var angularVelocity = gyro.angularVelocity.ReadValue();

                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x - angularVelocity.x,
                    transform.eulerAngles.y - angularVelocity.y,
                    0
                );
            }
            //kalau gyroscope tidak ada pakai Accelerometer
            else
            {
                var acceleration = Accelerometer.current.acceleration.ReadValue();

                //x=>y
                //z=>x
                var rotateSpeed = 350f * Time.deltaTime;
                accelerationZData = ((float)decimal.Round(((decimal)acceleration.z), 2));


                if (acceleration.x >= 0.1) transform.eulerAngles = new Vector3(-accelerationZData * 180f, transform.rotation.eulerAngles.y + (Math.Abs(acceleration.x) * rotateSpeed), 0);
                else if (acceleration.x <= -0.1) transform.eulerAngles = new Vector3(-accelerationZData * 180f, transform.rotation.eulerAngles.y - (Math.Abs(acceleration.x) * rotateSpeed), 0);
                else transform.eulerAngles = new Vector3(-accelerationZData * 180f, transform.rotation.eulerAngles.y, 0);
            }
        }
    }
}
