using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Transform centerOfMass;
    public float motorTorque = 1500f;
    public float maxSteeringAngle = 30f;
    public float brakeTorque = 3000f;
    public float driftThreshold = 0.5f;

    public WheelCollider frontLeftWheel, frontRightWheel;
    public WheelCollider rearLeftWheel, rearRightWheel;

    public AudioSource engineSoundSource;
    public AudioClip engineIdleClip;
    public AudioClip engineRevClip;
    public AudioSource driftSoundSource;
    public ParticleSystem driftSmoke;
    public TrailRenderer rearLeftTrail;
    public TrailRenderer rearRightTrail;

    private Rigidbody rb;
    private float inputThrottle;
    private float inputSteer;
    private float inputBrake;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;

        engineSoundSource.clip = engineIdleClip;
        engineSoundSource.Play();
    }

    void Update()
    {
        GetInputs();
        ApplySteering();
        ApplyThrottle();
        ApplyBrakes();
        HandleEngineSound();
        HandleDriftEffects();
    }

    void GetInputs()
    {
        inputThrottle = Input.GetAxis("Vertical");
        inputSteer = Input.GetAxis("Horizontal");
        inputBrake = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    void ApplySteering()
    {
        float steeringAngle = maxSteeringAngle * inputSteer;
        frontLeftWheel.steerAngle = steeringAngle;
        frontRightWheel.steerAngle = steeringAngle;
    }

    void ApplyThrottle()
    {
        float torque = motorTorque * inputThrottle;
        rearLeftWheel.motorTorque = torque;
        rearRightWheel.motorTorque = torque;
    }

    void ApplyBrakes()
    {
        float brake = brakeTorque * inputBrake;
        frontLeftWheel.brakeTorque = brake;
        frontRightWheel.brakeTorque = brake;
        rearLeftWheel.brakeTorque = brake;
        rearRightWheel.brakeTorque = brake;
    }

    void HandleEngineSound()
    {
        if (inputThrottle != 0)
        {
            engineSoundSource.clip = engineRevClip;
        }
        else
        {
            engineSoundSource.clip = engineIdleClip;
        }

        if (!engineSoundSource.isPlaying)
        {
            engineSoundSource.Play();
        }

        // Adjust pitch based on the throttle input
        engineSoundSource.pitch = Mathf.Lerp(1f, 2f, Mathf.Abs(inputThrottle));
    }

    void HandleDriftEffects()
    {
        float slipAmount = Mathf.Max(Mathf.Abs(rearLeftWheel.sidewaysFriction.stiffness), Mathf.Abs(rearRightWheel.sidewaysFriction.stiffness));

        if (slipAmount > driftThreshold)
        {
            if (!driftSoundSource.isPlaying)
            {
                driftSoundSource.Play();
            }

            if (!driftSmoke.isPlaying)
            {
                driftSmoke.Play();
            }

            rearLeftTrail.emitting = true;
            rearRightTrail.emitting = true;
        }
        else
        {
            driftSoundSource.Stop();
            driftSmoke.Stop();
            rearLeftTrail.emitting = false;
            rearRightTrail.emitting = false;
        }
    }
}
