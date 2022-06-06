using nsBuildingPlacer;
using UnityEngine;

namespace nsGameOverDisplay
{
    public class GameOverDisplay : MonoBehaviour
    {
        [SerializeField] private BuildingPlacer _buildingPlacer;

        private void Awake()
        {
            _buildingPlacer.OnGameOver += BuildingPlacer_OnGameOver;
        }

        private void Start()
        {
            //Can't deactivate in Awake because its children need to subscribe to events too
            gameObject.SetActive(false);
        }

        private void BuildingPlacer_OnGameOver()
        {
            gameObject.SetActive(true);
        }
    }
}
