using nsBuildingPlacer;
using nsBuildingType;
using nsMousePositionHelper;
using nsNearbyResourceNodeFinder;
using nsResourceNode;
using TMPro;
using UnityEngine;

namespace nsGhostBuilding
{
    public class GhostBuilding : MonoBehaviour
    {
        [SerializeField] private BuildingPlacer _buildingPlacer;

        private NearbyResourceNodeFinder _nearbyResourceNodeFinder;
        private MousePositionHelper _mousePositionHelper;
        private BuildingType _currentBuildingType;
        private SpriteRenderer _spriteRenderer;
        private TextMeshProUGUI _textMeshPro;

        private int _nearbyResourceNodeCount;
        private int _totalAmountPerCycle;
        private string _text;

        private void OnEnable()
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (_textMeshPro == null) _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
            if (_spriteRenderer == null) return;
            if (_textMeshPro == null) return;
            _buildingPlacer.OnCurrentBuildingTypeChange += BuildingPlacer_OnCurrentBuildingTypeChange;
        }

        private void OnDisable()
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (_textMeshPro == null) _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
            if (_spriteRenderer == null) return;
            if (_textMeshPro == null) return;
            _buildingPlacer.OnCurrentBuildingTypeChange -= BuildingPlacer_OnCurrentBuildingTypeChange;
        }

        private void BuildingPlacer_OnCurrentBuildingTypeChange(BuildingType buildingType)
        {
            _currentBuildingType = buildingType;
            if (_currentBuildingType == null)
            {
                _spriteRenderer.enabled = false;
                _textMeshPro.enabled = false;
            }
            else
            {
                _spriteRenderer.sprite = _currentBuildingType.Sprite;
                _spriteRenderer.enabled = true;
                _textMeshPro.enabled = true;
            }
        }

        private void Awake()
        {
            _nearbyResourceNodeFinder = new NearbyResourceNodeFinder();
            _currentBuildingType = null;
            _spriteRenderer = null;
            _textMeshPro = null;
        }

        private void Start()
        {
            _mousePositionHelper = new MousePositionHelper(Camera.main);
        }

        private void Update()
        {
            if (_spriteRenderer == null) return;
            if (_spriteRenderer.enabled == false) return;
            transform.position = _mousePositionHelper.MouseWorldPosition;

            _nearbyResourceNodeCount = _nearbyResourceNodeFinder.OverlapCircleAll(transform.position, _currentBuildingType.ResourceGeneratorData.NodeDetectionRadius, _currentBuildingType.ResourceGeneratorData.ResourceType);
            _totalAmountPerCycle = _nearbyResourceNodeCount * _currentBuildingType.ResourceGeneratorData.AmountPerCycle;
            _text = string.Concat('+', _totalAmountPerCycle, ' ', '(', _nearbyResourceNodeCount, ')');
            _textMeshPro.SetText(_text);
        }
    }
}
