using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlyingMovement : MonoBehaviour
{
    [SerializeField] public float flySpeed = 15;
    public float leftRightSpeed = 10;
    
    // Update is called once per frame
    void Update()
    {
        // Continuously fly forward
        transform.Translate(Vector3.forward * Time.deltaTime * flySpeed, Space.World);
    }
}
