using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mario Haberle, 2016, Flying Drone, https://www.youtube.com/playlist?list=PLPAgqhxd1Ib1YYqYnZioGyrSUzOwead17, 26/02/2019



public class DroneMovement : MonoBehaviour {

    Rigidbody DroneBody;
    
    // Used in inspector to check forces
    public float UpForce;


    // Speed moving forward
    private float MovementForwardSpeed = 500.0f;
    //Leaning forward variable
    private float TiltForward = 0;
    private float FTiltVelocity;


    // Rotation that shall be achieved
    private float WantedYRo;
    // To move smoothly in rotation
    public float CurrentYRo;
    // How much it rotates when key is pressed
    private float RotateAmount = 2.5f;
    // used to smoothly rotate
    private float RotateYVelocity;

    // Vector to chacnge the force increase to zero
    private Vector3 VelocityDampToZero;


    // force applied when moving left and right
    private float SideMovement = 300.0f;
    // Leaning sideways
    private float TiltSideways;
    // Used to smooth tilt
    private float STiltVelocity;


    // WHen scene is run get the Drones rigidbody
    void Awake()
    {
        DroneBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Methods
        MovementUpDown();
        MovementForward();
        Rotation();
        SpeedClamp();
        Lean();

        // levitate the Drone
        DroneBody.AddRelativeForce(Vector3.up * UpForce);
        // Assign the rotation, Tilt X Axis, changes Y rotation based of Rotaion(), Side rotation
        DroneBody.rotation = Quaternion.Euler(new Vector3(TiltForward, CurrentYRo, TiltSideways));
    }


    // Drones movement Across the Y position axis
    void MovementUpDown()
    {

        // is the drone moving on both vertical and horizontal axis
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            // if I or K is pressed leviate the drone
            if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.K))
            {
                DroneBody.velocity = DroneBody.velocity;
            }
            // if none of the keys are pressed
            if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && !Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.L))
            {
                // the velocity should not have any force up or down
                DroneBody.velocity = new Vector3(DroneBody.velocity.x, Mathf.Lerp(DroneBody.velocity.y, 0, Time.deltaTime * 5), DroneBody.velocity.z);
                UpForce = 281;
            }
            // if I and K and J or L is not pressed
            if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && !Input.GetKey(KeyCode.J) || !Input.GetKey(KeyCode.L))
            {
                // the velocity should not have any force up or down
                DroneBody.velocity = new Vector3(DroneBody.velocity.x, Mathf.Lerp(DroneBody.velocity.y, 0, Time.deltaTime * 5), DroneBody.velocity.z);
                UpForce = 110;
            }
            // if J or L is pressed
            if (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L))
            {
                UpForce = 410;
            }
        }

        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            UpForce = 200;
        }

        // If theres an input of I or Fire 3 The drone will have force applied to move upward
        if (Input.GetKey(KeyCode.I) || Input.GetButton("Fire3"))
        {
            UpForce = 450;
            if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
            {
                UpForce = 200;
            }
        }

        // If theres an input of K or Fire 1 The drone will have force applied to move downward
        else if (Input.GetKey(KeyCode.K) || Input.GetButton("Fire1"))
        {
            UpForce = -200;
        }

        // If nether I or K or the vertical axis is pressed/effected then add force to counter the gravity
        else if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f))
        {
            UpForce = 98.1f;
        }
    }

    void MovementForward()
    {
        // If W/S or vertical is not equal to zero
        if (Input.GetAxis("Vertical") != 0)
        {
            // Slowly increases the drones speed
            DroneBody.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * MovementForwardSpeed);
            // Tilts the drone 20% when moving forward over the time of 0.1f
            TiltForward = Mathf.SmoothDamp(TiltForward, 20 * Input.GetAxis("Vertical"), ref FTiltVelocity, 0.1f);

        }
    }

    // Rotation of the drone across the rotation Y
    void Rotation()
    {
        // if J is pressed the rotation wanted will decrease
        if (Input.GetKey(KeyCode.J) || Input.GetButton("Jump"))
        {
            WantedYRo -= RotateAmount;
        }
        // if L is pressed the rotation wanted will increase
        if (Input.GetKey(KeyCode.L) || Input.GetButton("Fire2"))
        {
            WantedYRo += RotateAmount;
        }

        // Changes the current rotation, smoothly/slowly moves from previous current to wanted rotaion over the time of 0.25f
        CurrentYRo = Mathf.SmoothDamp(CurrentYRo, WantedYRo, ref RotateYVelocity, 0.25f);
    }

    void SpeedClamp()
    {
        // moving forwards and backwards

        // if the absolute value of the vertical is greater than 0.2 AND the absolute value of the Horizontal is greater than 0.2
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            // Drone velocity will be set as the maximum of drone as the square length of the drone velocity
            //Lerp (like SmoothDamp), starting value, end value, over the time(large the number the quicker it is)
            DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }

        // if the absolute value of the vertical is greater than 0.2 AND the absolute value of the Horizontal is less than 0.2
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }


        //moving left to right/right to left

        // if the absolute value of the vertical is less than 0.2 AND the absolute value of the Horizontal is greater than 0.2
        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }

        // Decelerate

        // if the absolute value of the vertical is less than 0.2 AND the absolute value of the Horizontal is less than 0.2
        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            // slowly change the velocity to zero
            DroneBody.velocity = Vector3.SmoothDamp(DroneBody.velocity, Vector3.zero, ref VelocityDampToZero, 0.95f);
        }
    }

    void Lean()
    {
        // if A/D is pressed
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            // add force to the sides
            DroneBody.AddRelativeForce(Vector3.right * Input.GetAxis("Horizontal") * SideMovement);
            // tilt drone sideways over the time of 0.1 by 20%
            TiltSideways = Mathf.SmoothDamp(TiltSideways, -20 * Input.GetAxis("Horizontal"), ref STiltVelocity, 0.1f);
        }
        else
        {
            //if neither A/D is pressed slowly rotate to 0% (back to normal) over 0.1
            TiltSideways = Mathf.SmoothDamp(TiltSideways, 0, ref STiltVelocity, 0.1f);
        }

    }
}
