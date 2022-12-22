using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.DrivingActions drivingActions;
    private PlayerMotor motor;


    void Awake()
    {
        playerInput = new PlayerInput();
        drivingActions = playerInput.Driving;
        motor = GetComponent<PlayerMotor>();
    }

    void FixedUpdate()
    {
        motor.ProcessMove(drivingActions.Movement.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        drivingActions.Enable();
    }

    private void OnDisable()
    {
        drivingActions.Disable();
    }
}
