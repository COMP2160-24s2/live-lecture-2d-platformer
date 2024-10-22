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
public class PlayerReset : MonoBehaviour
{

#region Components
    private Rigidbody2D rigidbody;
#endregion

#region Other objects
    private Transform resetPoint;
#endregion

#region State
    private bool resetPressed = false;
#endregion

#region Actions
    private Actions actions;
    private InputAction resetAction;
#endregion

#region Init & Destroy
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        actions = new Actions();
        resetAction = actions.debug.reset;
    }

    void OnEnable()
    {
        actions.debug.Enable();
    }

    void OnDisable()
    {
        actions.debug.Disable();
    }

    void Start() 
    {
        // instantiate an empty object at our starting point
        resetPoint = new GameObject("Reset point").transform;
        resetPoint.position = transform.position;
    }
#endregion Init

#region Update
    void Update()
    {
        resetPressed = resetPressed || resetAction.WasPressedThisFrame();
    }
#endregion Update

#region FixedUpdate
    void FixedUpdate()
    {       
        if (resetPressed)
        {
            // teleport without collision detection
            rigidbody.position = resetPoint.position;
            resetPressed = false;
        }
    }
#endregion FixedUpdate

}
