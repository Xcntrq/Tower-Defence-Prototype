using Cinemachine;
using UnityEngine;

namespace nsCameraSize
{
    public class CameraSize : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
        [SerializeField] private float _zoomSpeed;
        [SerializeField] private float _lerpSpeed;
        [SerializeField] private float _lerpLimit;
        [SerializeField] private float _sizeMin;
        [SerializeField] private float _sizeMax;
        [SerializeField] private float _sizeDefault;

        private float _sizeCurrent;
        private float _sizeTarget;

        private void Start()
        {
            _sizeTarget = _sizeDefault;
            _sizeCurrent = _sizeDefault;
            _cinemachineVirtualCamera.m_Lens.OrthographicSize = _sizeDefault;
        }

        private void Update()
        {
            float mouseScrollDelta = Input.mouseScrollDelta.y;
            if ((mouseScrollDelta == 0) && (_sizeCurrent == _sizeTarget)) return;

            _sizeTarget -= mouseScrollDelta * _zoomSpeed;
            _sizeTarget = Mathf.Clamp(_sizeTarget, _sizeMin, _sizeMax);
            _sizeCurrent = Mathf.Lerp(_sizeCurrent, _sizeTarget, Time.deltaTime * _lerpSpeed);
            float difference = Mathf.Abs(_sizeCurrent - _sizeTarget);
            if (difference <= _lerpLimit) _sizeCurrent = _sizeTarget;
            _cinemachineVirtualCamera.m_Lens.OrthographicSize = _sizeCurrent;
        }
    }
}
