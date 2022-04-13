using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Move : MonoBehaviour
{
    public static Move Instance;

    [SerializeField] Vector3 movementTransform;
    PlayerInputController playerInputController;

    Vector2 readingValue;
    Vector3 movementValue;
    private bool onClick = false;
    [SerializeField] float speed = .5f;

    public bool touched = false;

    private void Awake()
    {
        Instance = this;

        readingValue = Vector2.zero;
        playerInputController = new PlayerInputController();

        playerInputController.CharacterControls.Move.started += MovementInput;
        playerInputController.CharacterControls.Move.performed += MovementInput;
        playerInputController.CharacterControls.Move.canceled += MovementInput;
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !touched)
        {
            touched = true;
        }

        if (touched)
        {

            if (Input.GetMouseButtonDown(0))
            {
                onClick = true;
            }
            if (Input.GetMouseButton(0))
            {
                transform.Translate(-movementValue.x * speed, 0, 0);
                transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -19.2f, 17.3f), transform.localPosition.y, transform.localPosition.z);
            }
            if (Input.GetMouseButtonUp(0))
            {
                onClick = false;
                readingValue = Vector2.zero;
            }

        }

    }
    void MovementInput(InputAction.CallbackContext context)
    {
        if (onClick)
        {
            readingValue = context.ReadValue<Vector2>();

            movementValue.x = readingValue.x;

        }


    }

    private void OnEnable()
    {
        playerInputController.Enable();
    }
    private void OnDisable()
    {
        playerInputController.Disable();
    }
}
