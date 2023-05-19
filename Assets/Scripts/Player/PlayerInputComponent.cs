using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputComponent : MonoBehaviour
{
    [SerializeField] float jumpInputBuffer = 0.1f;
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public bool JumpPressInput { get; private set; } = false;
    public bool JumpHoldInput { get; private set; } = false;
    public bool DropInput { get; private set; } = false;

    enum InputType { Gamepad, Keyboard }
    InputAction gamepadMove, keyboardMove, jump, drop, attack, gamepadAim;
    InputType currentInputType = InputType.Keyboard;

    private void Awake()
    {
        InputMap inputMap = new InputMap();
        gamepadMove = inputMap.FindAction("Move");
        keyboardMove = inputMap.FindAction("Move Keyboard");
        jump = inputMap.FindAction("Jump");
        gamepadAim = inputMap.FindAction("Aim");
        drop = inputMap.FindAction("Drop");
    }

    private void OnEnable()
    {
        keyboardMove.Enable();
        keyboardMove.started += _ => ChangeInputType(InputType.Keyboard);
        keyboardMove.performed += (InputAction.CallbackContext ctx) => MoveInput = ctx.ReadValue<Vector2>();
        keyboardMove.canceled += _ => MoveInput = Vector2.zero;

        jump.Enable();
        jump.started += _ => StartCoroutine(BufferJump());
        jump.performed += _ => JumpHoldInput = true;
        jump.canceled += _ => JumpHoldInput = false;

        drop.Enable();
        drop.performed += _ => DropInput = true;
        drop.canceled += _ => DropInput = false;

    }
    private void OnDisable()
    {
        
    }

    private void ChangeInputType(InputType type)
    {
        if (currentInputType != type)
        {
            currentInputType = type;
            if (currentInputType == InputType.Keyboard)
            {
                Cursor.visible = true;
            }
            else
            {
                Cursor.visible = false;
            }
        }
    }
    IEnumerator BufferJump()
    {
        JumpPressInput = true;
        yield return new WaitForSeconds(jumpInputBuffer);
        JumpPressInput = false;
    }
}
