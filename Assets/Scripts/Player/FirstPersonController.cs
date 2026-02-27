using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;

    [Header("Audio")]
    public float walkStepInterval = 0.5f;
    public float moveThreshold = 0.1f;

    [Header("Camera Feel")]
    public Transform cameraPivot;
    public float bobAmount = 0.05f;
    public float bobFrequency = 10f;
    public float bobSmoothing = 10f;

    public float turnSwayAmount = 2.0f;
    public float turnSwaySmoothing = 12f;

    private Vector3 camBaseLocalPos;
    private float bobTime;
    private float targetRoll;
    private float currentRoll;



    public float stepTimer = 0f;

    private Vector3 lastPos;
    private CharacterController controller;
    public Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        lastPos = transform.position;

        if (cameraPivot == null) cameraPivot = cameraTransform;
        camBaseLocalPos = cameraPivot.localPosition;

        lastPos = transform.position;


    }

    void Update()
    {
        // Ground check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Movement input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        Vector3 delta = transform.position - lastPos;
        lastPos = transform.position;
        //Igrnore vertical
        delta.y = 0f;
        bool moving = delta.sqrMagnitude > 0.00001f;
        bool actuallyMoving = delta.sqrMagnitude > 0.00001f;



        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        targetRoll = Mathf.Clamp(-mouseX * turnSwayAmount, -turnSwayAmount, turnSwayAmount);
        currentRoll = Mathf.Lerp(currentRoll, targetRoll, turnSwaySmoothing * Time.deltaTime);

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);

        //Camera bobbing
        bool doBob = isGrounded && actuallyMoving;

        if (doBob)
        {
            bobTime += Time.deltaTime * bobFrequency;

            // sideways + up bob
            float x = Mathf.Sin(bobTime) * bobAmount * 0.5f;
            float y = Mathf.Abs(Mathf.Cos(bobTime)) * bobAmount;

            Vector3 bobOffset = new Vector3(x, y, 0f);
            Vector3 targetPos = camBaseLocalPos + bobOffset;

            cameraPivot.localPosition = Vector3.Lerp(cameraPivot.localPosition, targetPos, bobSmoothing * Time.deltaTime);
        }
        else
        {
            bobTime = 0f;
            cameraPivot.localPosition = Vector3.Lerp(cameraPivot.localPosition, camBaseLocalPos, bobSmoothing * Time.deltaTime);
        }

        // Apply roll (sway) without affecting pitch/yaw
        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, currentRoll);



        if (!isGrounded || !moving)
        {
            //Debug.Log($"No steps. grounded={isGrounded} moving={moving} vel={controller.velocity}");
            stepTimer = walkStepInterval * 0.5f;
            return;
        }


        float interval = walkStepInterval;

        stepTimer += Time.deltaTime;
        if (stepTimer >= interval)
        {
            stepTimer = 0f; // reset timer

            //pick one of the sounds
            string stepName = (Random.value < 0.5f) ? "Step1" : "Step2";
            AudioManager.instance.Play(stepName);
            Debug.Log("Played footstep sound: " + stepName);
        }

    }
}
