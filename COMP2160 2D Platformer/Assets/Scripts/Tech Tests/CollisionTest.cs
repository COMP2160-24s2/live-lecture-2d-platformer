/**
 *
 * Author: Malcolm Ryan
 * Version: 1.0
 * For Unity Version: 2022.3
 */

using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LogFile))]
public class CollisionTest : MonoBehaviour
{

#region Parameters
    [SerializeField] private float speed = 1;
    [SerializeField] private float riseTime = 2; // s
#endregion 

#region Components
    private LogFile log;
    private Rigidbody2D rigidbody;
#endregion

#region State
    private float startTime; // s
    private List<ContactPoint2D> contacts = new List<ContactPoint2D>();
#endregion

#region Init & Destroy
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;
        rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        log = GetComponent<LogFile>();

        startTime = Time.time;
    }
#endregion 

#region FixedUpdate
    void FixedUpdate()
    {       
        Vector2 v = rigidbody.velocity;
        int nContacts = rigidbody.GetContacts(contacts);

        Vector2 tp = transform.position;
        Vector2 rp = rigidbody.position;

        log.WriteLine(Time.fixedTime, tp.x, tp.y, rp.x, rp.y, v.x, v.y, nContacts);

        if (Time.fixedTime - startTime < riseTime)
        {
            // move down
            v.y = -speed;
        }
        else 
        {
            // move up
            v.y = speed;
        }

        rigidbody.velocity = v;
    }
#endregion 

#region Gizmos
    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            // Don't run in the editor
            return;
        }
    }
#endregion 
}
