/**
 *
 *
 * Author: Malcolm Ryan
 * Version: 1.0
 * For Unity Version: 2022.3
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{

#region Parameters
    [SerializeField] private float gravity = -40;       // m/s/s
    [SerializeField] private float maxFallSpeed = -20;  // m/s
    [SerializeField] private float moveSpeed = 10;      // m/s
    [SerializeField] private float jumpInitialSpeed = 10;      // m/s
    [SerializeField] private float jumpRiseTime = 0.1f; // s
    [SerializeField] private float jumpBufferTime = 0.1f; // s
    [SerializeField] private float maxGroundSlope = 60; // degrees
#endregion 

#region Components
    private Rigidbody2D rigidbody;
#endregion

#region Enums
    public enum JumpState { OnGround, RisingNoGravity, Gravity, FallingNoGravity };
#endregion

#region State
    private JumpState jumpState = JumpState.Gravity;
    private float jumpSpeed = 0;
    private float jumpStartTime = float.NegativeInfinity;

    private int nContacts = 0;
    private List<ContactPoint2D> contacts = new List<ContactPoint2D>();
    private float move = 0;
    private float jumpPressedTime = float.NegativeInfinity;
#endregion

#region Properties
    private JumpState State 
    {
        get { return jumpState; }
        set { 
            jumpState = value;
            stateChangePositions.Add(transform.position);
            stateChangeValues.Add(jumpState);
        }
    }
#endregion


#region Debug State
    private Vector3? lastJumpPosition = null;

    private List<Vector3> stateChangePositions = new List<Vector3>();
    private List<JumpState> stateChangeValues = new List<JumpState>();
#endregion


#region Properties
    public Vector2 velocity 
    {
        get {
            return rigidbody.velocity;
        }
    }
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
        // update contacts
        nContacts = rigidbody.GetContacts(contacts);

        UpdateStateMachine();

        Vector2 v = rigidbody.velocity;

        // move horizontally
        v.x = move * moveSpeed;
        v.y = jumpSpeed;

        rigidbody.velocity = v;
    }

    private bool OnGround() 
    {
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

#region State Machine
    private void UpdateStateMachine()
    {
        bool isJumpPressed = jumpAction.IsPressed();

        switch (State) {
            case JumpState.OnGround:
                if (!OnGround())
                {
                    State = JumpState.Gravity;
                    
                }
                else if (Time.fixedTime - jumpPressedTime < jumpBufferTime)
                {
                    jumpSpeed = jumpInitialSpeed;
                    jumpStartTime = Time.fixedTime;

                    // Note: Spend at least one frame in RisingNoGravity to avoid
                    // bug where rigidbody contacts lag a frame behind movement.

                    State = JumpState.RisingNoGravity;
                }
            break;

            case JumpState.RisingNoGravity:
                if (Time.time - jumpStartTime > jumpRiseTime || 
                    !isJumpPressed)
                {
                    State = JumpState.Gravity;
                }
            break;

            case JumpState.Gravity:
                jumpSpeed = jumpSpeed + gravity * Time.fixedDeltaTime;

                if (OnGround())
                {
                    State = JumpState.OnGround;
                    jumpSpeed = 0;
                }
                else if (jumpSpeed < maxFallSpeed)
                {
                    State = JumpState.FallingNoGravity;
                    jumpSpeed = maxFallSpeed;
                }
            break;

            case JumpState.FallingNoGravity:
                if (OnGround())
                {
                    State = JumpState.OnGround;
                    jumpSpeed = 0;
                }
            break;
        }
    }
#endregion


#region Gizmos

    private Color[] stateGizmoColors = { Color.blue, Color.cyan, Color.green, Color.magenta};

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

        for (int i = 0; i < stateChangePositions.Count; i++)
        {
            Vector3 pos = stateChangePositions[i];
            JumpState state = stateChangeValues[i];

            Gizmos.color = stateGizmoColors[(int) state];
            Gizmos.DrawWireSphere(pos, 0.1f);
        }
    }
#endregion Gizmos
}
