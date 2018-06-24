using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float thrustMultiplier = 100f;
    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip winSound;

    enum RotateDirection { LEFT, RIGHT };
    enum State { ALIVE, DYING, TRANSCENDING };
    State state = State.ALIVE;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.ALIVE)
        {
            ProcessInput();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.ALIVE) return;
   
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Collided with friend");
                break;
            case "Finish":
                ProcessVictory();
                break;
            default:
                ProcessLoss();
                break;
        }
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
            audioSource.PlayOneShot(mainEngineSound);
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
        ProcessThrustInput();
        ProcessRotateInput();
    }

    private void ProcessThrustInput()
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
    }

    private void ProcessRotateInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Rotate(RotateDirection.LEFT);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Rotate(RotateDirection.RIGHT);
        }
    }

    private void ProcessVictory()
    {
        state = State.TRANSCENDING;
        audioSource.Stop();
        audioSource.PlayOneShot(winSound);
        Invoke("LoadNextLevel", 1f);
    }

    private void ProcessLoss()
    {
        state = State.DYING;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        Invoke("LoadFirstLevel", 1f);
    }


    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
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
