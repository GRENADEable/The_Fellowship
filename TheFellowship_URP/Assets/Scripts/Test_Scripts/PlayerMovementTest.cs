using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementTest : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private CharacterController2D _charController;
    [SerializeField] private float playerSpeed = 1f;
    #endregion

    #region Private Variables
    private float _horizontal;
    private Rigidbody2D _rg2D;
    private Vector2 _moveDirection;
    private bool _isJumping = false;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        _rg2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _moveDirection = new Vector2(_horizontal, 0f).normalized;
        _charController.Move(_moveDirection.x * Time.fixedDeltaTime * playerSpeed, false, _isJumping);
        _isJumping = false;
    }
    #endregion

    #region My Functions

    #endregion

    #region Events

    #region Input Systems
    /// <summary>
    /// Tied to Player Input event for movement;
    /// </summary>
    /// <param name="context"> Callback context for checking if the input presed state; </param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
            _horizontal = context.ReadValue<Vector2>().x;
        else if (context.canceled)
            _horizontal = 0f;
    }

    /// <summary>
    /// Tied to Player Input event for jumping;
    /// </summary>
    /// <param name="context"> Callback context for checking if the input presed state; </param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            _isJumping = true;
    }
    #endregion

    #endregion
}