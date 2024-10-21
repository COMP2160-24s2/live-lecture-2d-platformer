/**
 *
 * Author: Malcolm Ryan
 * Version: 1.0
 * For Unity Version: 2022.3
 */

using UnityEngine;

[DefaultExecutionOrder(-10)]
public class GameManager : MonoBehaviour
{

#region Parameters
#endregion 

#region Singleton
    private static GameManager instance;
    public static GameManager Instance 
    {
        get { return instance;}
    }
#endregion

#region State
#endregion

#region Properties
#endregion

#region Actions
#endregion

#region Events
#endregion

#region Init & Destroy
    void Awake()
    {
        if (instance != null) 
        {
            Debug.LogError("There are multiple instances of GameManager in the scene.");
        }

        instance = this;
    }
#endregion 

#region Update
    void Update()
    {
    }
#endregion 

#region FixedUpdate
    void FixedUpdate()
    {        
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
