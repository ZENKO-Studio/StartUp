/** @SAMI 06-06-24
 *  This script handles movement and other stuff related to Nell (Player Controller particularly)
 **/
using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static EventBus;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class NellController : CharacterBase
{

    CharacterController characterController;
    Animator animator;
    
    #region Character Control Values
    [Header("Character Controls")]
    
    public bool bEnableMovement = true;
    [SerializeField] float rotSpeed = 300f;

    //Can be used if jumping required
    [SerializeField] float jumpSpeed = 4f;

    float ogStepOffset;
    float ySpeed = 0f;

    [Tooltip("Height of Character Collider when crouched (can be tweaked with animation if needed)")]
    [SerializeField] float crouchHeight = 1.28f;
    private float crouchCenter;
    private float defaultHeight;
    private float defaultCenter;
    #endregion

    #region Sound And Audio

    [Header("How far should the sound be heard")]
    [SerializeField] float crouchSound = 0f;
    [SerializeField] float walkSound = 5f;
    [SerializeField] float runSound = 8f;

    float soundRange = 0f;

    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 1f;

    #endregion

    #region Camera Stuff
    [Header("Cinemachine")]

    [SerializeField] private CinemachineVirtualCamera cineCam;

    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject camTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    public float camYUp = 5.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float camYDown = -15.0f;

    [Tooltip("Additional degrees to override the camera. Useful for fine-tuning camera position when locked")]
    [HideInInspector] public float CameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axes")]
    [HideInInspector] public bool LockCameraPosition = false;

    [Tooltip("How Close the camera should be when zoomed in, default distance is 3")]
    [SerializeField] float camZoomInDistance = 1f;

    [Tooltip("How Far the camera should be when zoomed out, default distance is 3")]
    [SerializeField] float camZoomOutDistance = 1f;

    [Tooltip("How fast the camera should Zoom In and Out")]
    [Range(0.5f, 3f)]
    float camZoomSpeed = 2f;

    private float currCamDist = 3;

    // Cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    #endregion
    
    #region Input Values
    [Header("Player Input Values")]
    public Vector2 moveInput;
    public Vector2 lookInput;
    public bool jump;
    public bool sprint;
    public bool crouch;
    public float zoom;

    public bool cursorLocked = true;
	public bool cursorInputForLook = true;
    #endregion

    #region Other Vars

    //Temp #TODO Replace later with the Interactable Script
    private InventoryItem _itemInRange = null;

    private bool isInventoryOpen = false;
    private bool isFlashOn = false;
    private bool isCamMode = false;

    Flashlight flashlight;

    #endregion

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        defaultHeight = characterController.height;
        defaultCenter = characterController.center.y;
        crouchCenter = (crouchHeight / 2) + characterController.skinWidth; 
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ogStepOffset = characterController.stepOffset;

    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        //#TODO: Remove later just for trial purpose
        if (health <= 0)
            SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (characterController != null)
        {
            PlayerMovement();
        }
    }

    private void LateUpdate()
    {
        CameraRotation();
        CameraZoom();
    }

    private void PlayerMovement()
    {
        if (!bEnableMovement)
            return;

        Vector3 movDir = new Vector3(moveInput.x, 0, moveInput.y);
        movDir = Quaternion.AngleAxis(camTarget.transform.rotation.eulerAngles.y, Vector3.up) * movDir;

        float inputMag = Mathf.Clamp01(movDir.magnitude);

        if (sprint && Stamina > 0)
        {
            inputMag *= 2;
            soundRange = runSound;
            DepleteStamina();
        }
        else
        {
            soundRange = walkSound;
        }

        PlayerJump();

        animator.SetFloat("InputMagnitude", inputMag, 0.05f, Time.deltaTime);   //This is to smmoth out blend value for sharp input changes in WASD

        if (movDir != Vector3.zero)
        {
             animator.SetBool("IsMoving", true);

            Quaternion toRotation = Quaternion.LookRotation(movDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotSpeed * Time.deltaTime);
        }
        else
        {
             animator.SetBool("IsMoving", false);
             if(GetStamina() < 100)
                GenerateStamina();
        }

    }

    private void PlayerJump()
    {
        if(crouch) return;

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            characterController.stepOffset = ogStepOffset;
            ySpeed = -0.5f;

            if (jump)
            {
                ySpeed = jumpSpeed;
                jump = false;
            }
        }
        else
        {
            characterController.stepOffset = 0;
        }
    }


    private const float _threshold = 0.01f;


    private void CameraRotation()
    {
        // If there is an input and camera position is not fixed
        if (lookInput.sqrMagnitude >= _threshold)
        {
            // Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = 1.0f;

            _cinemachineTargetYaw += lookInput.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += lookInput.y * deltaTimeMultiplier;
        }

        // Clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, camYDown, camYUp);

        // Cinemachine will follow this target
        camTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }

    private void CameraZoom()
    {
        if(cineCam == null)
        {
            Debug.Log("Nell Character needs reference to the Cinemachine Virtual Camera for Zoom to work!");
            return;
        }

        currCamDist += zoom * camZoomSpeed * Time.deltaTime;

        currCamDist = Mathf.Clamp(currCamDist, camZoomInDistance, camZoomOutDistance);

        cineCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = currCamDist;
    }

    private void Crouch()
    {
        animator.SetBool("IsCrouching", crouch);

        if (crouch)
        {
            characterController.center = new Vector3(0f, crouchCenter, 0f);
            characterController.height = crouchHeight;
            soundRange = crouchSound;
            camTarget.transform.position -= new Vector3(0, .5f, 0);
        }
        else
        {
            characterController.center = new Vector3(0f, defaultCenter, 0f);
            characterController.height = defaultHeight;
            soundRange = walkSound;
            camTarget.transform.position += new Vector3(0, .5f, 0);
        }
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {

        Debug.Log("Footstep");
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = UnityEngine.Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, crouch ? FootstepAudioVolume / 2 : FootstepAudioVolume);
            }
            
            var sound = new Sound(transform.position, soundRange);

            Sounds.MakeSound(sound);
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 velocity = animator.deltaPosition;
        velocity.y = ySpeed * Time.deltaTime;

        characterController.Move(velocity);

        
    }

    public void Teleport(Transform t)
    {
        characterController.enabled = false;
        transform.SetPositionAndRotation(t.position, t.rotation);
        characterController.enabled = true;
    }

    //Set things that are in range and interactable
    internal void SetInteractable(InventoryItem inventoryItem)
    {
        _itemInRange = inventoryItem;
    }

    #region Read Inputs
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        if (cursorInputForLook)
        {
            lookInput = value.Get<Vector2>();
        }
    }

    public void OnJump(InputValue value)
    {
        jump = value.isPressed;
    }

    public void OnSprint(InputValue value)
    {
        sprint = value.isPressed;
    }

    public void OnCrouch(InputValue value)
    {
        crouch = !crouch;
        Crouch();
    }

    public void OnInteract(InputValue value)
    {
        Debug.Log($"{name} is Interacting");
        if (_itemInRange)
            _itemInRange.Interact(this);
    }

    public void OnCamZoom(InputValue value)
    {
        zoom = Mathf.Clamp(value.Get<float>(), -1, 1) * -1;
    }

    public void OnInventory(InputValue value)
    {
        isInventoryOpen = !isInventoryOpen;
        EventBus.Publish(new ToggleInventoryEvent(isInventoryOpen));
    }

    public void OnCamMode(InputValue value)
    {
        isCamMode = !isCamMode;
        //Call to Capture Script Function
    }

    public void  OnFlashlight(InputValue value)
    {
        if(flashlight)
        {
            flashlight.ToggleFlashlight();
        }
    }

    #endregion

    #region HelperMethods
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    #endregion
}
