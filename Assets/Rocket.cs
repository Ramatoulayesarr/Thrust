using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float thrustMultiplier = 100f;

    enum RotateDirection { LEFT, RIGHT };

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void Thrust()
    {
        FreezeRotation();
        rigidBody.AddRelativeForce(Vector3.up * thrustMultiplier * 10 * Time.deltaTime);
        UnfreezeRotation();
    }

    private void Rotate(RotateDirection direction)
    {
        switch (direction)
        {
            case RotateDirection.LEFT:
                transform.Rotate(Vector3.forward * thrustMultiplier * Time.deltaTime);
                break;
            case RotateDirection.RIGHT:
                transform.Rotate(Vector3.back * thrustMultiplier * Time.deltaTime);
                break;
        }
    }

    private void PlayThrustSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void StopThrustSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Thrust();
            PlayThrustSound();
        }
        else
        {
            StopThrustSound();
        }
        if (Input.GetKey(KeyCode.A))
        {
            Rotate(RotateDirection.LEFT);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Rotate(RotateDirection.RIGHT);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Collided with friend");
                break;
            default:
                print("Lost!");
                break;
        }
    }

    private void FreezeRotation()
    {
        rigidBody.freezeRotation = true;
    }

    private void UnfreezeRotation()
    {
        rigidBody.freezeRotation = false;
    }
}
