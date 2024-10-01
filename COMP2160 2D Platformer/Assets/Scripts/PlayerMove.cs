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
    [SerializeField] private float speed = 10;          // m/s
#endregion 

#region Components
    private Rigidbody2D rigidbody;
#endregion

#region State
    private float move = 0;
#endregion

#region Actions
    private Actions actions;
    private InputAction moveAction;
#endregion

#region Init & Destroy
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        actions = new Actions();
        moveAction = actions.movement.move;
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
        v.x = move * speed;

        rigidbody.velocity = v;
    }
#endregion FixedUpdate

#region Gizmos

    private ContactPoint2D[] contacts = new ContactPoint2D[100];
    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            // Don't run in the editor
            return;
        }

        Gizmos.color = Color.red;

        int nContacts = rigidbody.GetContacts(contacts);

        for (int i = 0; i < nContacts; i++)
        {
            Vector3 p = contacts[i].point;
            Vector3 n = contacts[i].normal;
            Gizmos.DrawWireSphere(p, 0.1f);
            Gizmos.DrawLine(p, p + n);
        }
    }
#endregion Gizmos
}
