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

    [SerializeField] ParticleSystem thrustParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;


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
        PlayThrustSound();
        PlayThrustParticles();
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

    private void PlayThrustParticles()
    {
        thrustParticles.Play();
    }

    private void StopThrustParticles()
    {
        if (thrustParticles.isPlaying)
        {
            thrustParticles.Stop();
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
        }
        else
        {
            StopThrustSound();
            StopThrustParticles();
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
        StopThrustSound();
        StopThrustParticles();
        audioSource.PlayOneShot(winSound);
        successParticles.Play();
        Invoke("LoadNextLevel", 1f);
    }

    private void ProcessLoss()
    {
        state = State.DYING;
        StopThrustSound();
        StopThrustParticles();
        audioSource.PlayOneShot(deathSound);
        deathParticles.Play();
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
