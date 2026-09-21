using UnityEngine;

public class PlayerCam : MonoBehaviour
{

    public static PlayerCam Instance;
    [Header("Sensitivity")]
    [SerializeField] private float senX = 2f;
    [SerializeField] private float senY = 2f;
    public bool updatingRotation;

    [Header("Player")]
    [SerializeField] private Transform player;

    private float xRotation = 0f;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!updatingRotation) return;
        float mouseX = Input.GetAxisRaw("Mouse X") * senX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * senY;

        player.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Only rotate camera vertically
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
