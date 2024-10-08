/**
 *
 *
 * Author: Malcolm Ryan
 * Version: 1.0
 * For Unity Version: 2022.3
 */

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{

#region Parameters
    [SerializeField] private float gravity = -40;       // m/s/s
    [SerializeField] private float maxFallSpeed = -20;  // m/s
    [SerializeField] private float moveSpeed = 10;      // m/s
    [SerializeField] private float jumpSpeed = 10;      // m/s
    [SerializeField] private float jumpBufferTime = 0.1f; // s
    [SerializeField] private float maxGroundSlope = 60; // degrees
#endregion 

#region Components
    private Rigidbody2D rigidbody;
#endregion

#region State
    private int nContacts = 0;
    private ContactPoint2D[] contacts = new ContactPoint2D[100];
    private float move = 0;
    private float jumpPressedTime = float.NegativeInfinity;

    private Vector3? lastJumpPosition = null;
#endregion


#region Actions
    private Actions actions;
    private InputAction moveAction;
    private InputAction jumpAction;
#endregion

#region Init & Destroy
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;
        rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        actions = new Actions();
        moveAction = actions.movement.move;
        jumpAction = actions.movement.jump;
    }

    void OnEnable() 
    {
        actions.movement.Enable();
    }

    void OnDisable() 
    {
        actions.movement.Disable();
    }
#endregion Init

#region Update
    void Update()
    {
        move = moveAction.ReadValue<float>();

        if (jumpAction.WasPressedThisFrame())
        {
            jumpPressedTime = Time.time;
            lastJumpPosition = transform.position;
        }
    }

#endregion Update

#region FixedUpdate
    void FixedUpdate()
    {        
        Vector2 v = rigidbody.velocity;

        // apply gravity
        v.y += gravity * Time.fixedDeltaTime;
        v.y = Mathf.Max(v.y, maxFallSpeed);

        // move horizontally
        v.x = move * moveSpeed;

        // jump
        if (Time.fixedTime - jumpPressedTime < jumpBufferTime && OnGround())
        {
            v.y = jumpSpeed;
            jumpPressedTime = float.NegativeInfinity;
        }

        rigidbody.velocity = v;
    }

    private bool OnGround() 
    {
        nContacts = rigidbody.GetContacts(contacts);

        for (int i = 0; i < nContacts; i++)
        {
            if (Vector2.Angle(contacts[i].normal, Vector2.up) < maxGroundSlope)
            {
                return true;
            }
        }

        return false;
    }
#endregion FixedUpdate


#region Gizmos

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            // Don't run in the editor
            return;
        }

        Gizmos.color = Color.white;

        for (int i = 0; i < nContacts; i++)
        {
            Vector3 p = contacts[i].point;
            Vector3 n = contacts[i].normal;
            Gizmos.DrawWireSphere(p, 0.1f);
            Gizmos.DrawLine(p, p + n);
        }

        if (lastJumpPosition != null) 
        {
            Gizmos.color = Color.yellow;
            Vector3 p = lastJumpPosition.Value;
            Gizmos.DrawWireSphere(p, 0.1f);
        }
    }
#endregion Gizmos
}
