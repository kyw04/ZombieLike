using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    private PlayerInput input;
    private InputAction moveAction;
    private Vector3 direction;
    private Vector3 velocity;
    [SerializeField] private Vector3 playerPosition;

    [SerializeField] private float moveSmooth = 0.05f;
    
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        moveAction = input.actions["Move"];
        playerPosition = transform.position;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        direction = moveAction.ReadValue<Vector2>();
    
        if (Vector3.Distance(playerPosition, transform.position) <= moveSmooth + 0.01f)
        {
            transform.position = playerPosition;

            if (!Physics2D.Raycast(transform.position, direction, 1f, LayerMask.GetMask("Wall")))
                playerPosition = transform.position + direction;
        }

        transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, moveSmooth);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, direction * 1f);
    }
}
