using UnityEngine;

namespace nsCameraMovement
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private void Update()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            float z = 0;

            Vector3 direction = new(x, y, z);
            direction.Normalize();

            transform.Translate(_speed * Time.deltaTime * direction);
        }
    }
}
