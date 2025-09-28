using UnityEngine;
using UnityEngine.InputSystem;

public class Mover : Entity
{
    private PlayerInput input;
    private InputAction moveAction;
    private Vector3 direction;
    private Vector3 velocity;
    [SerializeField] private LayerMask mask;
    [SerializeField] private Vector3 playerPosition;
    [SerializeField] private float moveSmooth = 0.05f;

    private enum Axis
    {
        None,
        Horizontal,
        Vertical
    };
    private Axis lastAxis;
    
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        moveAction = input.actions["Move"];
        playerPosition = transform.position;
        lastAxis = Axis.None;
    }

    private void FixedUpdate()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        Vector2 rawInput = moveAction.ReadValue<Vector2>();
        if (rawInput == Vector2.zero)
        {
            transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, moveSmooth);
            return;
        }
        
        direction = Vector3.zero;
        if (rawInput.x != 0 && rawInput.y != 0)
        {
            direction = lastAxis == Axis.Vertical ? Vector3.right * Mathf.Sign(rawInput.x) 
                                                    : Vector3.up * Mathf.Sign(rawInput.y);
        }
        else if (rawInput.x != 0)
        {
            direction = Vector3.right * Mathf.Sign(rawInput.x);
            lastAxis = Axis.Horizontal;
        }
        else
        {
            direction = Vector3.up * Mathf.Sign(rawInput.y);
            lastAxis = Axis.Vertical;
        }
        
        forward = direction != Vector3.zero ? direction : forward;
        if (Vector3.Distance(playerPosition, transform.position) <= moveSmooth + 0.05f)
        {
            
            
            transform.position = playerPosition;

            if (!Physics2D.Raycast(transform.position, direction, 1f, mask))
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
