using Cinemachine;
using nsMousePositionHelper;
using UnityEngine;

namespace nsCameraControls
{
    public class CameraControls : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _mouseScrollMagnitude;
        [SerializeField] private float _orthoSizeLerpSpeed;
        [SerializeField] private float _orthoSizeLerpLimit;
        [SerializeField] private float _minOrthoSize;
        [SerializeField] private float _defaultOrthoSize;
        [SerializeField] private float _maxOrthoSize;
        [SerializeField] private float _movementLimits;

        private MousePositionHelper _mousePositionHelper;
        private Vector3 _memorizedPosition;
        private Vector3 _desiredPosition;
        private float _currentOrthoSize;
        private float _desiredOrthoSize;
        private float _memorizedOrthoSize;
        private float _previouslyDesiredOrthoSize;

        private void Start()
        {
            _currentOrthoSize = _defaultOrthoSize;
            _desiredOrthoSize = _defaultOrthoSize;
            _memorizedOrthoSize = _defaultOrthoSize;
            _previouslyDesiredOrthoSize = _defaultOrthoSize;
            _cinemachineVirtualCamera.m_Lens.OrthographicSize = _defaultOrthoSize;
            _mousePositionHelper = new MousePositionHelper(Camera.main);
        }

        private void Update()
        {
            //Movement
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            float z = 0;

            if ((x != 0) || (y != 0))
            {
                Vector3 direction = new(x, y, z);
                direction.Normalize();
                transform.position += _movementSpeed * Time.deltaTime * direction;
                Vector3 position = transform.position;
                position.x = Mathf.Clamp(position.x, -_movementLimits, _movementLimits);
                position.y = Mathf.Clamp(position.y, -_movementLimits, _movementLimits);
                transform.position = position;
                _desiredPosition = position;
            }

            //Zoom
            float mouseScrollDelta = Input.mouseScrollDelta.y;

            if (mouseScrollDelta != 0)
            {
                _desiredOrthoSize -= mouseScrollDelta * _mouseScrollMagnitude;
                _desiredOrthoSize = Mathf.Clamp(_desiredOrthoSize, _minOrthoSize, _maxOrthoSize);
            }

            if (_desiredOrthoSize != _previouslyDesiredOrthoSize)
            {
                _previouslyDesiredOrthoSize = _desiredOrthoSize;
                _memorizedPosition = transform.position;
                _memorizedOrthoSize = _cinemachineVirtualCamera.m_Lens.OrthographicSize;
                Vector3 mouseWorldPosition = _mousePositionHelper.MouseWorldPosition;
                _desiredPosition = mouseWorldPosition + _desiredOrthoSize / _memorizedOrthoSize * (_memorizedPosition - mouseWorldPosition);
            }

            if (_currentOrthoSize != _desiredOrthoSize)
            {
                //Size
                _currentOrthoSize = Mathf.Lerp(_currentOrthoSize, _desiredOrthoSize, Time.deltaTime * _orthoSizeLerpSpeed);
                float orthoSizeDifference = Mathf.Abs(_currentOrthoSize - _desiredOrthoSize);

                //Position
                float _orthoSizeLerp = Mathf.InverseLerp(_memorizedOrthoSize, _desiredOrthoSize, _currentOrthoSize);
                transform.position = Vector3.Lerp(_memorizedPosition, _desiredPosition, _orthoSizeLerp);

                //Rounding
                if (orthoSizeDifference <= _orthoSizeLerpLimit)
                {
                    _currentOrthoSize = _desiredOrthoSize;
                    transform.position = _desiredPosition;
                }

                _cinemachineVirtualCamera.m_Lens.OrthographicSize = _currentOrthoSize;
            }
        }
    }
}
