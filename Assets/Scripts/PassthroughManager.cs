using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughManager : MonoBehaviour
{
	public OVRPassthroughLayer passthrough;

    public OVRInput.Button button;

    public OVRInput.Controller controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(button, controller))
        {
            passthrough.hidden = !passthrough.hidden;
        }
    }
    
    // Passthrough Opacity
    public void SetOpacity(float value)
    {
        passthrough.textureOpacity = value;
    }
}
