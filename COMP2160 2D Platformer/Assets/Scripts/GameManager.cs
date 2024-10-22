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
    private int coinsCollected = 0;
#endregion

#region Properties
    public int CoinsCollected
    {
        get { return coinsCollected; }
    }
#endregion

#region Events
    public delegate void CoinCollectedHandler(int coinsCollected);
    public event CoinCollectedHandler OnCoinCollected;
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

#region Events
    public void CoinCollected()
    {
        coinsCollected++;
        OnCoinCollected.Invoke(coinsCollected);
    }
#endregion 


}
