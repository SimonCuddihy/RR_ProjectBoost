using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    new Rigidbody rigidbody; // added 'new' keyword to suppress warning. might need to delete
    [SerializeField] float rcsThrust = 150f;
    [SerializeField] float mainThrust = 2000f;

    // particles
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    private bool isTransitioning = false;

    [SerializeField] private float levelLoadDelay = 1f;
    private bool collisionsTurnedOff = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>(); 
        rigidbody.mass = 1;
    }


    // Update is called once per frame
    void Update()
    {
        if (!isTransitioning)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }

        // debug mode
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    // debug functions
    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();

        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsTurnedOff = !collisionsTurnedOff;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionsTurnedOff) return; // ignore collisions when dead or when off (in debug mode)

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        isTransitioning = true;
        successParticles.Play();
        // co-routine - invoke/start LoadNextScene after 1 second
        Invoke("LoadNextLevel", levelLoadDelay); 
    }

    private void StartDeathSequence()
    {
        isTransitioning = true;
        deathParticles.Play();
        // co-routine - wait 1 second after death for restart
        Invoke("LoadFirstLevel", levelLoadDelay); 
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void ApplyThrust()
    {
        rigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        mainEngineParticles.Play();
    }

    private void StopApplyingThrust()
    {
        mainEngineParticles.Stop();
    }

    private void RespondToRotateInput()
    {
        rigidbody.freezeRotation = true; // take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidbody.freezeRotation = false; // resume physics control of rotation
    }



    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        currentScene = currentScene + 1;
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        if (currentScene == sceneCount)
        {
            currentScene = 0;
        }
        SceneManager.LoadScene(currentScene);
    }
}
