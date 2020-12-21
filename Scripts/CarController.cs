using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CarController : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float forwardAccel = 8f, reversAccel = 2f;
    private float speedInput;
    public Rigidbody rb;
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
    public ParticleSystem[] dustTrail;
    public float maxEmission = 25f;
    public float emissionFadeSpeed = 20f;
    private float emissionRate;
    public AudioSource engineSound;
    public AudioSource skidSFX;
    public float skidFadeSpeed = 2f;

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
        
        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn), rightFrontWheel.localRotation.eulerAngles.z);
        

        emissionRate = Mathf.MoveTowards(emissionRate, 0f, emissionFadeSpeed * Time.deltaTime);
        if (grounded && (Mathf.Abs(turnInput) > 0.5f || (rb.velocity.magnitude < maxSpeed * 0.5f && rb.velocity.magnitude != 0)))
        {
            emissionRate = maxEmission;
        }
        for (int i = 0; i < dustTrail.Length; i++)
        {
            var emissionModule = dustTrail[i].emission;

            emissionModule.rateOverTime = emissionRate;
        }
        if (speedInput <= 0.5f && Mathf.Abs(turnInput) <= 0.2f)
        {
            emissionRate = 0f;
        }

        if (engineSound != null)
        {
            engineSound.pitch = 1f + ((rb.velocity.magnitude / maxSpeed) * 2f);
        }

        if (skidSFX != null)
        {
            if (Mathf.Abs(turnInput) > 0.5f && speedInput > 3.5f && grounded)
            {
                skidSFX.volume = 1f;
            }
            else
            {
                skidSFX.volume = Mathf.MoveTowards(skidSFX.volume, 0f, skidFadeSpeed * Time.deltaTime);
            }
        }
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
        transform.position = rb.position;
        if (Input.GetAxis("Vertical") != 0 && grounded)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f,turnInput * 
                turnStrength * Time.deltaTime * Mathf.Sign(speedInput) * (rb.velocity.magnitude / maxSpeed),0f));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundCheck.position, Vector3.down);
    }
}
