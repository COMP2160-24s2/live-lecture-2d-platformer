/**
 *
 * Author: Malcolm Ryan
 * Version: 1.0
 * For Unity Version: 2022.3
 */

using UnityEngine;
using WordsOnPlay.Utils;

public class Follower : MonoBehaviour
{

#region Parameters
    [SerializeField] private float leadDistance = 5;
    [SerializeField] private Rect bounds;
    [SerializeField] private float minTargetSpeed = 0.1f;
    [SerializeField] private AnimationCurve cameraSpeed;
#endregion 

#region Connected object
    [SerializeField] private PlayerMove target;
#endregion 

#region State
    private float offset = 0;
#endregion

#region Init & Destroy
    void Awake()
    {
        UpdateBounds();
    }

    void Start()
    {
        UpdatePosition();
    }

    private void UpdateBounds()
    {
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        bounds.x += cameraWidth;
        bounds.width -= cameraWidth * 2;
    }
#endregion 

#region Update
    void LateUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector2 v = target.velocity;

        float targetOffset = 0;

        if (v.x > minTargetSpeed) {
            // moving to the right
            targetOffset = leadDistance;
        }
        else if (v.x < -minTargetSpeed) 
        {
            // moving to the left
            targetOffset = -leadDistance;
        }

        float delta = targetOffset - offset;
        // Implement easing based on the distance to the target
        float speed = cameraSpeed.Evaluate(Mathf.Abs(delta));

        if (delta > 0)
        {
            offset += Mathf.Min(delta, speed * Time.deltaTime);
        }
        else if (delta < 0)
        {
            offset += Mathf.Max(delta, -speed * Time.deltaTime);
        }

        Vector3 p = transform.position;
        p.x = target.transform.position.x + offset;
        p = bounds.Clamp(p);
        transform.position = p;
    }
#endregion 

#region Gizmos
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 0.1f);

        Gizmos.color = Color.red;
        bounds.DrawGizmo();
    }
#endregion 
}
