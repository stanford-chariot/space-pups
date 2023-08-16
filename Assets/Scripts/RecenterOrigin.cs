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

    public void Recenter()
    {
        XROrigin xrOrigin = GetComponent<XROrigin>();
        xrOrigin.MoveCameraToWorldLocation(target.position);
        xrOrigin.MatchOriginUpCameraForward(target.up, target.forward);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Recenter();
    }
}
