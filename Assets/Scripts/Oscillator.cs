using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    private Vector3 startingPos;
    [SerializeField] private Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [Range(0, 1)] [SerializeField] private float movementFactor; // 0 for not moved, 1 for fully moved
    [SerializeField] private float period = 2f;

    // Start is called before the first frame update
    private void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        autoMove();
    }

    // set movement factor automatically
    private void autoMove()
    {
        if (period <= Mathf.Epsilon) return;

        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2f; // about 6.28 radians

        float rawSinWave = Mathf.Sin(cycles * tau); // sin wave amplitude goes between +1 and -1
        movementFactor = rawSinWave / 2f; // sin wave between +0.5 and -0.5
        movementFactor = rawSinWave + 0.5f; // sin wave between +1 and -0

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}