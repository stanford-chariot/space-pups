using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;
public class RecenterOrigin : MonoBehaviour
{
    public InputActionProperty recenterButton;
    public Transform head;
    public Transform origin;
    public Transform target;
    public OVRInput.Button button;
    public OVRInput.Controller controller;
    public void Recenter()
    {
        XROrigin xrOrigin = GetComponent<XROrigin>();
        xrOrigin.MoveCameraToWorldLocation(target.position);
        xrOrigin.MatchOriginUpCameraForward(target.up, target.forward);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(button, controller))
        {
            Recenter();
        }
        // if (recenterButton.action.WasPressedThisFrame())
        // {
        //     Recenter();
        // }
    }
}
