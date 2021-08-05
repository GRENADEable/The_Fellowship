using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Serialized Variables
    [Space, Header("Datas")]
    [SerializeField] private GameManagerData gmData;

    [SerializeField] private float playerSpeed = 1f;
    #endregion

    #region Private Variables
    private CharacterController2D _charController;
    [SerializeField] private float _horizontal;
    [SerializeField] private Vector2 _moveDirection;
    private bool _isJumping = false;
    #endregion

    #region Unity Callbacks

    void Start()
    {
        _charController = GetComponent<CharacterController2D>();
    }

    void Update()
    {
        if (gmData.currGameState == GameManagerData.GameState.Game)
        {
            PlayerMovement();
        }
        else
            _charController.Move(0, false, false);
    }
    #endregion

    #region My Functions
    void PlayerMovement()
    {
        _moveDirection = new Vector2(_horizontal, 0f).normalized;
        _charController.Move(_moveDirection.x * Time.fixedDeltaTime * playerSpeed, false, _isJumping);
        _isJumping = false;
    }
    #endregion

    #region Events

    #region Input Systems
    /// <summary>
    /// Tied to Player Input event for movement;
    /// </summary>
    /// <param name="context"> Callback context for checking if the input presed state; </param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started && gmData.currGameState == GameManagerData.GameState.Game)
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
        if (context.started && gmData.currGameState == GameManagerData.GameState.Game)
            _isJumping = true;
    }
    #endregion

    void OnGamePausedEventReceived(bool isPaused)
    {
        //if(isPaused)
        //    hori
    }

    #endregion
}