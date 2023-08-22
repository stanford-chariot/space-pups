using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform resetTransform;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera playerHead;
    public OVRInput.Button buttonHorizontal;
    public OVRInput.Button buttonVertical;
    public OVRInput.Controller controller;
    
    public void ResetPositionVertical()
    {
        var rotationAngleX = playerHead.transform.rotation.eulerAngles.x - resetTransform.rotation.eulerAngles.x;
        player.transform.Rotate(-rotationAngleX, 0, 0);

        var distanceDiff = resetTransform.position - playerHead.transform.position;
        player.transform.position += distanceDiff;
    }

    public void ResetPositionHorizontal()
    {
        var rotationAngleY = playerHead.transform.rotation.eulerAngles.y - resetTransform.rotation.eulerAngles.y;
        player.transform.Rotate(0, -rotationAngleY, 0);
        var distanceDiff = resetTransform.position - playerHead.transform.position;
        player.transform.position += distanceDiff;
    }
    
    void Update()
    {
        if (OVRInput.GetDown(buttonHorizontal, controller))
        {
            ResetPositionVertical();
        }
        else if (OVRInput.GetDown(buttonVertical, controller))
        {
            ResetPositionHorizontal();
        }
    }
}
