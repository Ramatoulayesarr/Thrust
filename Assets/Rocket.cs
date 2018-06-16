using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
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

    private void thrust()
    {
        rigidBody.AddRelativeForce(Vector3.up);
    }

    private void rotate(RotateDirection direction)
    {
        switch (direction)
        {
            case RotateDirection.LEFT:
                transform.Rotate(Vector3.forward * 90 * Time.deltaTime);
                break;
            case RotateDirection.RIGHT:
                transform.Rotate(Vector3.back * 90 * Time.deltaTime);
                break;
        }
    }

    private void playThrustSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void stopThrustSound()
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
            thrust();
            playThrustSound();
        }
        else
        {
            stopThrustSound();
        }
        if (Input.GetKey(KeyCode.A))
        {
            rotate(RotateDirection.LEFT);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotate(RotateDirection.RIGHT);
        }
    }
}
