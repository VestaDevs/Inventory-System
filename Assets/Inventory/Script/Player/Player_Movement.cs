using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Vector3 Velocity;
    private Vector3 PlayerMovementInput;
    private bool Sneaking = false;

    [SerializeField] private InventoryManager inventoryManager;

    [Header("Components Needed")]
    [SerializeField] private CharacterController Controller;
    [SerializeField] private Transform Player;
    [SerializeField] private Transform Orientation;

    [Space]

    [Header("Movement")]
    [SerializeField] private float Speed = 5f;
    [SerializeField] private float JumpForce = 5f;
    [SerializeField] private float Gravity = 9.81f;

    [Space]

    [Header("Sneaking")]
    [SerializeField] private bool Sneak = false;
    [SerializeField] private float SneakSpeed = 2.5f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        PlayerMovementInput = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0f,
            Input.GetAxisRaw("Vertical")
        );

        MovePlayer();

        // Sneaking
        if (Input.GetKey(KeyCode.RightShift) && Sneak)
        {
            Player.localScale = new Vector3(1f, 0.5f, 1f);
            Sneaking = true;
        }

        if (Input.GetKeyUp(KeyCode.RightShift))
        {
            Player.localScale = new Vector3(1f, 1f, 1f);
            Sneaking = false;
        }
    }

    private void MovePlayer()
    {
        // Movement follows the Orientation direction
        Vector3 MoveVector =
     Orientation.forward * PlayerMovementInput.z +
     Orientation.right * PlayerMovementInput.x;

        MoveVector.y = 0f;

        if (MoveVector.magnitude > 1f)
        {
            MoveVector.Normalize();
        }

        // Prevent diagonal movement from being faster
        if (MoveVector.magnitude > 1f)
        {
            MoveVector.Normalize();
        }

        // Grounded
        if (Controller.isGrounded)
        {
            Velocity.y = -1f;

            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && !Sneaking)
            {
                Velocity.y = JumpForce;
            }
        }
        else
        {
            // Gravity
            Velocity.y -= Gravity * 2f * Time.deltaTime;
        }

        // Horizontal movement
        if (Sneaking)
        {
            Controller.Move(MoveVector * SneakSpeed * Time.deltaTime);
        }
        else
        {
            Controller.Move(MoveVector * Speed * Time.deltaTime);
        }

        // Vertical movement
        Controller.Move(Velocity * Time.deltaTime);
    }
    // CharacterController ပါသော်လည်း Trigger Collider ကို ဝင်တိုက်ပါက OnTriggerEnter အလုပ်လုပ်ပါသည်
    private void OnTriggerEnter(Collider other)
    {
        Items worldItem = other.GetComponent<Items>();
        if (worldItem == null)
        {
            worldItem = other.GetComponent<Items>();
        }
        if (worldItem != null && worldItem.item != null && inventoryManager != null)
        {
            int remainingAmount = inventoryManager.Additem(worldItem.item, worldItem.amount);

            if (remainingAmount <= 0)
            {
                Destroy(worldItem.gameObject);
                Debug.Log($"[Picked Up] Added {worldItem.amount}x {worldItem.item.ItemName()}!");
            }
            else
            {
                worldItem.amount = remainingAmount;
            }
        }
    }
}
