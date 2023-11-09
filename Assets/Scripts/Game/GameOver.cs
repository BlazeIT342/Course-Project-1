using Project.Managing;
using UnityEngine;

namespace Project.Game
{
    public class GameOver : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                GameEventManager.Instance.EndGame();
            }
        }
    }
}