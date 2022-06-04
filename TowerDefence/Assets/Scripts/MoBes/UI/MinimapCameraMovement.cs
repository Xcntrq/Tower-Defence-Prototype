using UnityEngine;

namespace nsMinimapCameraMovement
{
    public class MinimapCameraMovement : MonoBehaviour
    {
        [SerializeField] private float _scaler;
        [SerializeField] private float _sizeMin;
        [SerializeField] private float _sizeMax;

        private Camera _camera;
        private Camera _mainCamera;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            _mainCamera = Camera.main;
            _camera.orthographicSize = _scaler;
        }

        private void LateUpdate()
        {
            transform.position = _mainCamera.transform.position;
            _camera.orthographicSize = Mathf.Clamp(_mainCamera.orthographicSize * _scaler, _sizeMin, _sizeMax);
        }
    }
}
