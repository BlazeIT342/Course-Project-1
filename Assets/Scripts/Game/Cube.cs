using Project.Managing;
using UnityEngine;

namespace Project.Game
{
    public class Cube : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                GameEventManager.Instance.CollisionWall();

                StartCoroutine(GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CubeHolder>().RemoveCube(this));
            }

            if (collision.gameObject.CompareTag("CubePickup"))
            {
                GameEventManager.Instance.AddNewCube();
                Destroy(collision.gameObject);
            }
        }
    }
}