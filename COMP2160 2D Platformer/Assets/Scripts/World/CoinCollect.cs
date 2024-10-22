/**
 *
 * Author: Malcolm Ryan
 * Version: 1.0
 * For Unity Version: 2022.3
 */

using UnityEngine;
using WordsOnPlay.Utils;

public class CoinCollect : MonoBehaviour
{

#region FixedUpdate
    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
        GameManager.Instance?.CoinCollected();
    }
#endregion 

}
