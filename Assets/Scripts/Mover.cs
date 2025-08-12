using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    private PlayerInput input;
    private InputAction moveAction;

    [SerializeField] private float speed = 7.5f;
    
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        moveAction = input.actions["Move"];
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        transform.Translate(direction * (speed * Time.deltaTime));
    }
}
