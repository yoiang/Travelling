using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

// Based on FPS_Camera
public class WalkController : MonoBehaviour {
    public new Camera camera;
    private Quaternion cameraDefaultRotation;
    // TODO: optional look limits when not in VR?

    [Header("Mouse Look")]
    public bool mouseLook;
    public bool mouseLookInvert;
    public Vector2 mouseLookScale = new Vector2();
    private Vector2 mouseLookOffset = new Vector2();
    public Vector2 mouseLookNormalized {
        get {
            int verticalDirection;
            if (this.mouseLookInvert) {
                verticalDirection = -1;
            } else {
                verticalDirection = 1;
            }
            return new Vector2(
                this.mouseLookOffset.x * this.mouseLookScale.x,
                verticalDirection * this.mouseLookOffset.y * this.mouseLookScale.y
            );
        }
    }

    [Header("WASD Move")]
    public bool wasdMove;
    public float Speed = 5f;

    // [Header("VR")]
    public bool IsUsingVR {
        get => this.isUsingVR;
        set {
            this.isUsingVR = value;
            Debug.Log("IsUsingVR set to " + value, this);

            if (value) {
    #if UNITY_IOS || UNITY_ANDROID
                StartCoroutine(LoadXRDeviceByName(VR.CardboardDeviceName));
    #elif UNITY_STANDALONE
                // TODO:
                StartCoroutine(LoadXRDeviceByName(VR.NoDeviceName));
    #else
                StartCoroutine(LoadXRDeviceByName(VR.NoDeviceName));
    #endif
            } else {
                StartCoroutine(LoadXRDeviceByName(VR.NoDeviceName));
            }
        }
    }
    private bool isUsingVR;

    [Header("Device Orientation")]
    public Vector2 deviceOrientationScale = new Vector2();
    public bool IsUsingDeviceOrientation {
        get => this.isUsingDeviceOrientation || this.IsUsingVR;
        set {
            this.isUsingDeviceOrientation = value;

            // TODO: see if when we're using VR is also is enabled, then we can be more general with our assignment
            if (!this.IsUsingVR) {
                Input.gyro.enabled = value; 
            }

            Debug.Log("IsUsingDeviceOrientation set to " + value, this);
        }
    }
    private bool isUsingDeviceOrientation;
    private Vector3 deviceOrientationOffset = new Vector3();
    public Vector2 deviceOrientationOffsetNormalized {
        get {
            return new Vector2(
                this.deviceOrientationOffset.x * this.deviceOrientationScale.x,
                this.deviceOrientationOffset.y * this.deviceOrientationScale.y
            );
        }
    }
    public Quaternion deviceOrientationOffsetPrepared {
        get {
            Vector2 deviceOrientation = this.deviceOrientationOffsetNormalized;
            return Quaternion.Euler(
                - deviceOrientation.x,
                - deviceOrientation.y,
                0f
            );
        }
    }

    private Vector2 dragOffset = new Vector2();
    public Vector2 dragScale = new Vector2(1, 1);
    public Vector2 dragNormalized {
        get {
            return new Vector2(
                this.dragOffset.x * this.dragScale.x,
                this.dragOffset.y * this.dragScale.y
            );
        }
    }

    void Start() {
        if (this.camera == null) {
            Debug.LogError("No camera provided");

            this.enabled = false;
            return;
        }

        this.cameraDefaultRotation = this.camera.transform.rotation;

        //FPSCam.transform.Rotate(0, 0, 0); 
    }

    private void Translate(float x, float y, float z) {
        Vector3 rotation = Vector3.zero;
        rotation = this.camera.transform.eulerAngles;
        rotation.x = 0;
        rotation.z = 0;
        this.camera.transform.Rotate(rotation);
        this.camera.transform.Translate(x, y, z);
        if (rotation != Vector3.zero) {
            this.camera.transform.Rotate(-1 * rotation);
        }
    }

    void UpdateDrag() {
        // Only want one finger drags
        if (Input.touchCount != 1) {
            return;
        }
        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Moved) {
            return;
        }

        this.dragOffset += new Vector2(
            touch.deltaPosition.x * this.dragScale.x,
            touch.deltaPosition.y * this.dragScale.y
        );
    }

    void UpdateDeviceOrientation() {
        if (Input.gyro.enabled) {
            this.deviceOrientationOffset += Input.gyro.rotationRateUnbiased * Time.deltaTime;
        }
    }

    void UpdateMouseLook() {
        if (this.mouseLook) {
            float verticalDirection;
            if (this.mouseLookInvert) {
                verticalDirection = -1;
            } else {
                verticalDirection = 1;
            }

            this.mouseLookOffset += new Vector2(
                Input.GetAxis("Mouse X") * Time.deltaTime,
                verticalDirection * Input.GetAxis("Mouse Y") * Time.deltaTime
            );
        }
    }

    public void ToggleUseVR() {
        Debug.Log("Toggling VR to " + !this.IsUsingVR);
        this.IsUsingVR = !this.IsUsingVR;
    }

    protected IEnumerator LoadXRDeviceByName(string deviceName) {
        // Some VR Devices do not support reloading when already active, see
        if (String.Compare(XRSettings.loadedDeviceName, deviceName, true) != 0) {
            Debug.Log("Loading XR device " + deviceName);
            XRSettings.LoadDeviceByName(deviceName);

            // Must wait one frame after calling `XRSettings.LoadDeviceByName()`.
            yield return null;
        }
        XRSettings.enabled = deviceName != VR.NoDeviceName;
        // this.ResetCamera();
    }

    protected void ResetCamera() {
        this.camera.transform.localRotation = this.cameraDefaultRotation;
        this.dragOffset = new Vector2();
        this.mouseLookOffset = new Vector2();
    }

    public void ToggleUseDeviceOrientation() {
        Debug.Log("Toggling Using Device Orientation to " + !this.IsUsingDeviceOrientation);
        this.IsUsingDeviceOrientation = !this.IsUsingDeviceOrientation;
    }

    void UpdateWASDMove() {
        // TODO: is actually WASD and arrows
        if (this.wasdMove) {

            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))) {
                this.Translate(-this.Speed * Time.deltaTime, 0, this.Speed * Time.deltaTime);
                return;
            }
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))) {
                this.Translate(this.Speed * Time.deltaTime, 0, this.Speed * Time.deltaTime);
                return;
            }
            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))) {
                this.Translate(-this.Speed * Time.deltaTime, 0, -this.Speed * Time.deltaTime);
                return;
            }
            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))) {
                this.Translate(this.Speed * Time.deltaTime, 0, -this.Speed * Time.deltaTime);
                return;
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                this.Translate(0, 0, this.Speed * Time.deltaTime);
                return;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                this.Translate(0, 0, -this.Speed * Time.deltaTime);
                return;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                this.Translate(-this.Speed * Time.deltaTime, 0, 0);
                return;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                this.Translate(this.Speed * Time.deltaTime, 0, 0);
                return;
            }
        }
    }

    void Update() {
        this.UpdateDrag();
        this.UpdateMouseLook();
        this.UpdateDeviceOrientation();

        this.UpdateCameraRotation();

        this.UpdateWASDMove();
    }

    void UpdateCameraRotation() {
        Vector2 deviceOrientationOffset;
        if (!this.IsUsingVR) {
            deviceOrientationOffset = this.deviceOrientationOffsetNormalized;
        } else {
            deviceOrientationOffset = new Vector2();
        }

        Quaternion localRotation = this.cameraDefaultRotation * 
            Quaternion.Euler(
                - this.dragNormalized.y - this.mouseLookNormalized.y - deviceOrientationOffset.x,
                this.dragNormalized.x + this.mouseLookNormalized.x - deviceOrientationOffset.y,
                0
            ) ;//* 
                // Quaternion.Euler(90, 0, 0);


        // this.target.transform.Rotate(, 0, 0);
        this.camera.transform.rotation = localRotation;
    }
}
