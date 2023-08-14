using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hoverboard : MonoBehaviour
{
    public Rigidbody HB; // This is the main rigid body of the hover vehicle.
    public List<GameObject> HoverForcers; // This game object is the reference for the location of the hover force.
    public GameObject com; // This game object is the reference for the center of mass.
    public Transform groundchecker; // The location of this object is where the raycast that checks for the ground originates from.
    public float turntorque = 10f; // This value determines how quickly the vehicle can turn. This value is also linked to spin tricks in the air.
    public float forwardtilt = 10f; // This value determines how much the vehicle tilts forwards and backwards. This value is also linked to front and back flips in the air. 
    public float Sidetilt = 10f; // This value determines how much the vehicle tilts from side to side. 
    public float sidefriction; // This value Determines how much friction there is against sideways movement. 0 is no friction, 1 is full friction.
    public float frontfriction; // This value Determines how much friction there is against front to back movement. 0 is no friction, 1 is full friction.
    public float thrustforce; // This value determines the strength of the force pushing the vehicle forwards and backwards.
    public float selfrightforce; // This value determines the strength of the force that rotates the vehicle to be right side up.

    public float Hoverlength; // This value determines how high the vehicle hovers off the ground.
    public float springstiffness; // This value determines how stiff the vehicle's hovering characteristics are.
    public float damperstrength; // This value reduces the up to down undulation of the hover vehicle.

    // These values are all used to simulate a suspension force;
    private float previoussusdidstance;
    private float currentsusdistance;
    private float springvelocity;
    private float springforce;
    private float damperforce;

    public bool Grounded; // This is a True or false statement that tells the script whether the car is on the ground or not.
    public LayerMask Whatisground; // This layermask Determines what the ground layer is. It can be changed in the Inspector.

    private float sidevelocity; // This value determines how fast the vehicle is going to the left or right.
    private float frontvelocity; // This value determines how fast the vehicle is going forwards or backwards.

    private float thrustcancel; //This variable is used to cancel thrust when the hover vehicle isn't on the ground.

    private float turncancel; //This variable decreases steering while in the air.





    Vector2 movedirection;

    public InputAction Hovercontrols;

    public Animator Doganim;
    // Start is called before the first frame update.
    void Start()
    {
        // This line of code sets the center of mass to a gameobject (it's called COM in scene). you can change the center of gravity by moving the gameobject.
        HB.centerOfMass = com.transform.localPosition;
    }

    void OnGoforward(InputValue value)
    {
        // This asset uses the new input system.
        movedirection = value.Get<Vector2>();
    }

    // Update is called once per frame.
    void FixedUpdate()
    {
        // This line of code simulates gravity
        HB.AddForce(-Vector3.up * 500, ForceMode.Force);

        // This boolean is used to check if the hover vehicle is on the ground.
        Grounded = false;

        //This variable is used to cancel thrust when the hovercar isn't on the ground.
        thrustcancel = 1;
       
        //This variable decreases steering while in the air.
        turncancel = 1;

        //This line of code manually sets the drag value of the rigidbody.
        HB.drag = 0.05f;

        //These lines of code handle the animations of the character.
        Doganim.SetBool("Isturnleft", false);
        Doganim.SetBool("Isturnright", false);
        Doganim.SetBool("Isbraking", false);

        //These lines of code add a friction effect to the rigidbody.
        sidevelocity = transform.InverseTransformDirection(HB.velocity).x;
        frontvelocity = transform.InverseTransformDirection(HB.velocity).y;

        // This raycast checks if the hovercar is on the ground.
        RaycastHit groundcheck;
        if (Physics.Raycast(groundchecker.position, -transform.up, out groundcheck, (Hoverlength + 3f), Whatisground))
        {
            Grounded = true;

            //Prints "we cool" when the hovercar is on the ground. 
            Debug.Log("weCool");
        }

        //If the vehicle is not on the ground, these lines of code change the handling characteristics. This is the "tricking" state.
        if (Grounded == false)
        {
            // This line of code prints "Weeeee" if the hovercar is off the ground.
            Debug.Log("WEEEEE");

            // This line of code allows you to do tricks in the air by multiplying the forwardtilt and turntorque by a float to increase their magnitude.
            HB.AddTorque(transform.right * movedirection.y * forwardtilt * 200f, ForceMode.Force);
            HB.AddTorque(transform.up * movedirection.x * turntorque * 7f, ForceMode.Force);

            // This line of code cancels thrust when in the air.
            thrustcancel = 0;

            // This line of code decreases turning when in the air.
            turncancel = 0.25f;

            // This line of code cancels the drag of the rigidbody when in the air.
            HB.drag = 0f;
        }

        // These lines of code rotate the hovervehicle towards the normal of the ground it's on.
        var angle = Vector3.Angle(transform.up, groundcheck.normal);
        if(angle > 0.1)
        {
            var Axis = Vector3.Cross(transform.up, groundcheck.normal);
            HB.AddTorque(Axis * angle * selfrightforce * turncancel);
        }


        //These lines of code handle the tilting and turning of the hover vehicle.
        HB.AddForce((-sidevelocity * sidefriction * transform.right), ForceMode.VelocityChange);
        HB.AddForce((-frontvelocity * frontfriction * transform.forward), ForceMode.VelocityChange);

        HB.AddTorque(transform.right * movedirection.y * forwardtilt, ForceMode.Force);
        HB.AddTorque(transform.up * movedirection.x * turntorque, ForceMode.Force);
        HB.AddTorque(transform.forward * -movedirection.x * Sidetilt, ForceMode.Force);

        HB.AddForce(transform.forward * movedirection.y * thrustforce * thrustcancel, ForceMode.Force);

        //These lines of code activate the corect animations when appropriate.
        if(movedirection.x < 0)
        {
            Doganim.SetBool("Isturnleft", true);
        }

        if (movedirection.x > 0)
        {
            Doganim.SetBool("Isturnright", true);
        }
        if (movedirection.y < 0)
        {
            Doganim.SetBool("Isbraking", true);
        }
            //And finally, These lines of code simulate car suspension using a single raycast, and a function called Hooke's law.

            foreach (GameObject HoverForcer in HoverForcers)
        {
            RaycastHit hit;

            if(Physics.Raycast(HoverForcer.transform.position, transform.TransformDirection(Vector3.down), out hit, (Hoverlength)))
            {
                previoussusdidstance = currentsusdistance;

                currentsusdistance = Hoverlength - (hit.distance);

                springvelocity = (currentsusdistance - previoussusdidstance) / Time.fixedDeltaTime;

                springforce = springstiffness * currentsusdistance;

                damperforce = damperstrength * springvelocity;


                HB.AddForceAtPosition(transform.TransformDirection(Vector3.up) * (springforce + damperforce), HoverForcer.transform.position);
            }
        }
    }
}
