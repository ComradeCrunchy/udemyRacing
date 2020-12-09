using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float forwardAccel = 8f, reversAccel = 2f;
    private float speedInput;
    private Rigidbody rb;
    public float turnStrength = 180f;
    private float turnInput;
    private bool grounded;
    public Transform groundCheck, groundCheck2;
    public LayerMask ground;
    public float raycastRange = .75f;
    private float dragOnGround;
    public float gravityMod = 10f;

    public Transform leftFrontWheel, rightFrontWheel;
    public float maxWheelTurn = 25f;

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        rb.transform.parent = null;
        dragOnGround = rb.drag;
    }

    private void Update()
    {
        speedInput = 0f;
        if (Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel;
        }
        else if(Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * reversAccel;
        }

        turnInput = Input.GetAxis("Horizontal");
        if (Input.GetAxis("Vertical") != 0 && grounded)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f,turnInput * 
                turnStrength * Time.deltaTime * Mathf.Sign(speedInput) * (rb.velocity.magnitude / maxSpeed),0f));
        }
        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn), rightFrontWheel.localRotation.eulerAngles.z);
        transform.position = rb.position;
    }

    private void FixedUpdate()
    {
        grounded = false;
        RaycastHit hit;
        Vector3 normalTarget = Vector3.zero;
        grounded = Physics.Raycast(groundCheck.position, Vector3.down, out hit, raycastRange, ground);
        if (grounded)
        {
            rb.drag = dragOnGround;
            normalTarget = hit.normal;
        }
        if (Physics.Raycast(groundCheck2.position, Vector3.down, out hit, raycastRange, ground))
        {
            grounded = true;
            normalTarget = (normalTarget + hit.normal) / 2f;
        }
        
        else
        {
            rb.drag = .1f;
            rb.AddForce(-Vector3.up * (gravityMod * 100f));
        }
        
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        if (grounded)
        {
            rb.AddForce(transform.forward * (speedInput * 1000f));
            transform.rotation = Quaternion.FromToRotation(transform.up, normalTarget) * transform.rotation;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundCheck.position, Vector3.down);
    }
}
