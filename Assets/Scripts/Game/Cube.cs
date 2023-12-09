using Project.Managing;
using UnityEngine;

namespace Project.Game
{
    /// <summary>
    /// Controls actions and events associated with cube objects upon collision with other game elements.
    /// </summary>
    public class Cube : MonoBehaviour
    {
        private GameEventManager _gameEventManager;

        private void OnEnable()
        {
            _gameEventManager = GameEventManager.Instance;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                // Signals the game manager that a collision with a wall occurred.
                _gameEventManager.CollisionWall();

                // Initiates the removal of the cube from the CubeHolder component.
                StartCoroutine(GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CubeHolder>().RemoveCube(this));
            }

            if (collision.gameObject.CompareTag("CubePickup"))
            {
                // Signals the game manager to add a new cube.
                _gameEventManager.AddNewCube();

                // Destroys the CubePickup object upon collision.
                Destroy(collision.gameObject);
            }
        }
    }
}