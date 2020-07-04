using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    new Rigidbody rigidbody; // added 'new' keyword to suppress warning. might need to delete
    [SerializeField] float rcsThrust = 150f;
    [SerializeField] float mainThrust = 20f;
    const float deathRcsThrust = 1000f;
    const float deathMainThrust = 500f;
    private string[] deathDirection = { "NW", "N", "NE" };

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>(); 
        rigidbody.mass = 1;
    }


    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * mainThrust);
        }
    }

    private void Rotate()
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

    void OnCollisionEnter(Collision collision)
    {
        int currentScene = SceneManager.sceneCount - 1;
        
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Next Level":
                SceneManager.LoadScene(currentScene + 1);
                break;
            case "Finish":
                currentScene += 1;
                SceneManager.LoadScene(currentScene);
                break;
            default:
                SceneManager.LoadScene(currentScene);
                break;
        }
    }


}
