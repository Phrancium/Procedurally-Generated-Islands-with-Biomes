using UnityEngine;

public class IslandCameraController : MonoBehaviour
{
    [Header("Camera Setup")]
    [SerializeField] private Transform islandTransform;
    [SerializeField] private bool useOrthographicCamera = true;

    [Header("Camera Distance (Perspective Only)")]
    [SerializeField] private float perspectiveDistance = 20f;

    [Header("Orthographic Size")]
    [SerializeField] private float orthographicSize = 8f;

    [Header("Auto-Find Island")]
    [SerializeField] private bool autoFindIsland = true;

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

        if (useOrthographicCamera)
        {
            // Set to orthographic mode (best for 2D top-down view)
            cam.orthographic = true;
            cam.orthographicSize = orthographicSize;

            // Position camera directly above the island, looking down
            transform.position = new Vector3(
                islandTransform.position.x,
                islandTransform.position.y,
                islandTransform.position.z - 10f  // Pull back on Z axis
            );

            transform.rotation = Quaternion.identity; // Look straight ahead (at the quad)

            Debug.Log($"Camera set to Orthographic mode at position {transform.position}");
        }
        else
        {
            // Perspective mode
            cam.orthographic = false;
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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Resetting camera view...");
            SetupCamera();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            useOrthographicCamera = !useOrthographicCamera;
            Debug.Log($"Switched to {(useOrthographicCamera ? "Orthographic" : "Perspective")} camera");
            SetupCamera();
        }
    }
}