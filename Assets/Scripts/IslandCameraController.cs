using UnityEngine;

public class IslandCameraController : MonoBehaviour
{
    [SerializeField] private Transform islandTransform;

    [Header("Camera Distance (Perspective Only)")]
    [SerializeField] private float perspectiveDistance = 20f;

    [Header("Auto-Find Island")]
    [SerializeField] private bool autoFindIsland = true;

    [Header("Camera Movement")]
    [SerializeField] private float speed = 200f;
    [SerializeField] private float sens = 2f;
    private float rotationX = 0f;
    private float rotationY = 0f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        // Auto-find the island if not manually assigned
        if (autoFindIsland && islandTransform == null)
        {
            SimpleProceduralGeneration island = FindFirstObjectByType<SimpleProceduralGeneration>();
            if (island != null)
            {
                islandTransform = island.transform;
                Debug.Log("Island found and camera positioned!");
            }
            else
            {
                Debug.LogError("No RadialIslandGenerator found in scene!");
            }
        }

        SetupCamera();
    }

    public void SetupCamera()
    {
        if (islandTransform == null)
        {
            Debug.LogWarning("Island transform not assigned! Make sure RadialIslandGenerator is in the scene.");
            return;
        }
        // Perspective mode
        cam.fieldOfView = 60f;

        // Position camera in front of the island
        transform.position = new Vector3(
            islandTransform.position.x,
            islandTransform.position.y,
            islandTransform.position.z - perspectiveDistance
        );

        transform.LookAt(islandTransform);

        Debug.Log($"Camera set to Perspective mode at position {transform.position}");
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Resetting camera view...");
            SetupCamera();
        }

        float horizontal = 0f;
        float vertical = 0f;
        float upDown = 0f;

        // WASD movement
        if (Input.GetKey(KeyCode.W)) vertical += 1f;
        if (Input.GetKey(KeyCode.S)) vertical -= 1f;
        if (Input.GetKey(KeyCode.A)) horizontal -= 1f;
        if (Input.GetKey(KeyCode.D)) horizontal += 1f;
        if (Input.GetKey(KeyCode.LeftShift)) upDown += 1f;
        if (Input.GetKey(KeyCode.LeftControl)) upDown -= 1f;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 up = Vector3.up;

        Vector3 movement = (forward * vertical + right * horizontal + up * upDown).normalized;

        transform.position += movement * speed * Time.deltaTime;


        float mouseX = 0f;
        float mouseY = 0f;

        //Mouse Look
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        rotationY += mouseX * sens;
        rotationX += mouseY * sens * -1f;

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }
}