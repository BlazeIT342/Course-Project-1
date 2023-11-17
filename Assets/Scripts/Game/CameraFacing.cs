using UnityEngine;

namespace Project.Game
{
    /// <summary>
    /// Maintains the forward direction of the object to always face the main camera during the late update phase.
    /// Useful for ensuring that the object is facing the camera in a 3D environment.
    /// </summary>
    public class CameraFacing : MonoBehaviour
    {
        /// <summary>
        /// Adjusts the object's forward direction to match the main camera's forward direction during the late update phase.
        /// </summary>
        private void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}