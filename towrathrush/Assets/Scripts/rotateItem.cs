using UnityEngine;

public class rotateItem : MonoBehaviour
{
    [SerializeField] private Vector3 degreesPerSecond = new Vector3(0f, 0f, 90f);
    [SerializeField] private Space rotateSpace = Space.Self;

    [Header("Floating")]
    [SerializeField] private Vector3 floatAxis = Vector3.up;
    [SerializeField] private float floatAmplitude = 0.25f;
    [SerializeField] private float floatFrequency = 1.5f;
    [SerializeField] private bool floatInLocalSpace = true;

    private Vector3 basePosition;
    private float phaseOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        basePosition = floatInLocalSpace ? transform.localPosition : transform.position;
        phaseOffset = Random.value * Mathf.PI * 2f; // desync multiple items
    }

    // Update is called once per frame
    void Update()
    {
        // rotate
        transform.Rotate(degreesPerSecond * Time.deltaTime, rotateSpace);

        // float
        Vector3 axis = floatAxis.sqrMagnitude > 0f ? floatAxis.normalized : Vector3.up;
        float offset = Mathf.Sin(Time.time * floatFrequency + phaseOffset) * floatAmplitude;
        Vector3 target = basePosition + axis * offset;

        if (floatInLocalSpace)
            transform.localPosition = target;
        else
            transform.position = target;
    }
}
