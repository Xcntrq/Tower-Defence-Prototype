using nsBuildingPlacer;
using nsBuildingType;
using nsMousePositionHelper;
using UnityEngine;

namespace nsGhostBuilding
{
    public class GhostBuilding : MonoBehaviour
    {
        [SerializeField] private BuildingPlacer _buildingPlacer;

        private MousePositionHelper _mousePositionHelper;
        private SpriteRenderer _spriteRenderer;

        private void OnEnable()
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
            _buildingPlacer.OnCurrentBuildingTypeChange += BuildingPlacer_OnCurrentBuildingTypeChange;
        }

        private void OnDisable()
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
            _buildingPlacer.OnCurrentBuildingTypeChange -= BuildingPlacer_OnCurrentBuildingTypeChange;
        }

        private void BuildingPlacer_OnCurrentBuildingTypeChange(BuildingType buildingType)
        {
            if (buildingType == null)
            {
                _spriteRenderer.enabled = false;
            }
            else
            {
                _spriteRenderer.sprite = buildingType.Sprite;
                _spriteRenderer.enabled = true;
            }
        }

        private void Awake()
        {
            _spriteRenderer = null;
        }

        private void Start()
        {
            _mousePositionHelper = new MousePositionHelper(Camera.main);
        }

        private void Update()
        {
            if (_spriteRenderer == null) return;
            if (_spriteRenderer.enabled == true) transform.position = _mousePositionHelper.MouseWorldPosition;
        }
    }
}
