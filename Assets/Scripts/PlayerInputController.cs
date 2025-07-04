using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour, PlayerInput.ILocomotionActions
{
    private PlayerInput playerInput { get; set; }
    public Vector2 MoveInput { get; private set; }

    #region Startup
    private void OnEnable()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();

        playerInput.locomotion.Enable();
        playerInput.locomotion.SetCallbacks(this);
    }

    private void OnDisable()
    {
        playerInput.locomotion.Disable();
        playerInput.locomotion.RemoveCallbacks(this);
    }
    #endregion
    
    public void OnMovement(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }
}
