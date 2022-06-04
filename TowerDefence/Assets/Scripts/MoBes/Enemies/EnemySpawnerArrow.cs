using nsColorValue;
using nsEnemySpawner;
using UnityEngine;
using UnityEngine.UI;

namespace nsEnemySpawnerArrow
{
    public class EnemySpawnerArrow : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private ColorValue _visibleColor;
        [SerializeField] private ColorValue _invisibleColor;

        private Image _image;
        private Camera _mainCamera;
        private RectTransform _rectTransform;
        private Transform _mainCameraTransform;
        private Transform _spawnTransform;
        private Vector3 _arrowRotation;
        private Vector3 _arrowDirection;
        private Vector3 _cameraPosition;
        private float _inverseLerp;
        private float _distance;
        private bool _wasVisible;
        private bool _isVisible;

        private void Start()
        {
            _spawnTransform = null;
            _mainCamera = Camera.main;
            _mainCameraTransform = _mainCamera.transform;
            _image = GetComponent<Image>();
            _rectTransform = GetComponent<RectTransform>();
            //Spawner should never try to make a decision where to spawn enemies until after its own Start
            _enemySpawner.OnSpawnPositionChange += EnemySpawner_OnSpawnPositionChange;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            //if (_spawnTransform == null) return;
            _cameraPosition = _mainCameraTransform.position;
            _cameraPosition.z = 0;

            _distance = Vector3.Distance(_spawnTransform.position, _cameraPosition);
            _isVisible = _distance > _mainCamera.orthographicSize * 2;
            if (_isVisible != _wasVisible) _image.color = _isVisible ? _visibleColor.Value : _invisibleColor.Value;
            _wasVisible = _isVisible;
            if (!_isVisible) return;

            _arrowRotation = Vector3.zero;
            _arrowDirection = (_spawnTransform.position - _cameraPosition).normalized;
            _arrowRotation.z = Mathf.Rad2Deg * Mathf.Atan2(_arrowDirection.y, _arrowDirection.x);

            //Temporary lerping the arrow to the screen bounds
            _inverseLerp = 1;
            _arrowDirection *= 1200;
            if (_arrowDirection.y > 480) _inverseLerp = Mathf.InverseLerp(0, _arrowDirection.y, 480);
            if (_arrowDirection.y < -205) _inverseLerp = Mathf.InverseLerp(0, _arrowDirection.y, -205);
            _arrowDirection.x *= _inverseLerp;
            _arrowDirection.y *= _inverseLerp;
            _inverseLerp = 1;
            if (_arrowDirection.x > 890) _inverseLerp = Mathf.InverseLerp(0, _arrowDirection.x, 890);
            if (_arrowDirection.x < -890) _inverseLerp = Mathf.InverseLerp(0, _arrowDirection.x, -890);
            _arrowDirection.x *= _inverseLerp;
            _arrowDirection.y *= _inverseLerp;

            _rectTransform.anchoredPosition = _arrowDirection;
            transform.eulerAngles = _arrowRotation;
        }

        private void EnemySpawner_OnSpawnPositionChange(Transform spawnTransform)
        {
            _spawnTransform = spawnTransform;
            gameObject.SetActive(spawnTransform != null);
        }
    }
}
