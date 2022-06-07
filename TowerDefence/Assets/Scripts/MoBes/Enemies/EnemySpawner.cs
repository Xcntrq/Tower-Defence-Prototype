using nsBuildingPlacer;
using nsDirection;
using nsEnemy;
using nsIntValue;
using nsSpawnPosition;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace nsEnemySpawner
{
    public enum SpawnerState
    {
        Cooldown,
        Spawning,
        Attacking
    }

    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private BuildingPlacer _buildingPlacer;
        [SerializeField] private IntValue _seed;
        [SerializeField] private Enemy _enemy;
        [SerializeField] private SpawnPosition _spawnPosition;
        [SerializeField] private int _spawnPositionsCount;
        [SerializeField] private float _spawnPositionsDistance;
        [SerializeField] private float _spawnRadius;
        [SerializeField] private float _cooldownTime;
        [SerializeField] private float _spawnInterval;

        private SpawnerState _spawnerState;
        private System.Random _randomPosition;
        private System.Random _randomLength;
        private List<SpawnPosition> _spawnPositions;
        private List<Enemy> _spawnedEnemies;
        private int _nextSpawnPositionIndex;
        private float _timeLeft;
        private int _waveNumber;
        private Direction _direction;
        private int _spawnsRemaining;
        private string _text;

        public List<SpawnPosition> SpawnPositions => _spawnPositions;

        public event Action<string> OnTextChange;
        public event Action<int> OnWaveNumberChange;
        public event Action<Transform> OnSpawnPositionChange;

        private void Awake()
        {
            _buildingPlacer.OnGameOver += BuildingPlacer_OnGameOver;
            _randomPosition = _seed.Value == 0 ? new System.Random() : new System.Random(_seed.Value);
            _randomLength = new System.Random();
            _spawnedEnemies = new List<Enemy>();
            _direction = new Direction();
            _waveNumber = 0;
            _spawnPositions = new List<SpawnPosition>();
            float averageAngle = 360f / _spawnPositionsCount;
            for (int i = 0; i < _spawnPositionsCount; i++)
            {
                float currentAngle = averageAngle * (i + 0.5f);
                Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
                Vector3 position = Vector3.up * _spawnPositionsDistance;
                position = rotation * position;
                SpawnPosition newSpawnPosition = Instantiate(_spawnPosition, position, Quaternion.identity, transform);
                _spawnPositions.Add(newSpawnPosition);
            }
            _spawnerState = SpawnerState.Attacking;
        }

        private void Update()
        {
            switch (_spawnerState)
            {
                case SpawnerState.Cooldown:
                    _text = string.Concat("Wave #", _waveNumber, " in ", _timeLeft.ToString("0.0"));
                    OnTextChange?.Invoke(_text);
                    _timeLeft -= Time.deltaTime;
                    if (_timeLeft <= 0)
                    {
                        _spawnsRemaining = _waveNumber;
                        _text = string.Concat("Wave #", _waveNumber, " spawning...");
                        OnTextChange?.Invoke(_text);
                        _spawnerState = SpawnerState.Spawning;
                    }
                    break;
                case SpawnerState.Spawning:
                    _timeLeft -= Time.deltaTime;
                    if (_timeLeft <= 0)
                    {
                        _timeLeft = _spawnInterval;
                        Vector3 spawnPosition = _spawnPositions[_nextSpawnPositionIndex].transform.position;
                        spawnPosition += (float)_randomLength.NextDouble() * _spawnRadius * _direction.GetRandom();
                        Enemy enemy = Instantiate(_enemy, spawnPosition, Quaternion.identity, transform);
                        enemy.Initialize(_buildingPlacer, this);
                        _spawnedEnemies.Add(enemy);
                        _spawnsRemaining--;
                    }
                    if (_spawnsRemaining == 0)
                    {
                        _text = string.Concat("Wave #", _waveNumber, " attacking!");
                        OnTextChange?.Invoke(_text);
                        _spawnPositions[_nextSpawnPositionIndex].gameObject.SetActive(false);
                        OnSpawnPositionChange?.Invoke(null);
                        _spawnerState = SpawnerState.Attacking;
                    }
                    break;
                case SpawnerState.Attacking:
                    if (_spawnedEnemies.Count == 0)
                    {
                        _waveNumber++;
                        OnWaveNumberChange?.Invoke(_waveNumber);
                        _nextSpawnPositionIndex = _randomPosition.Next(_spawnPositions.Count);
                        _spawnPositions[_nextSpawnPositionIndex].gameObject.SetActive(true);
                        OnSpawnPositionChange?.Invoke(_spawnPositions[_nextSpawnPositionIndex].transform);
                        _timeLeft = _cooldownTime;
                        _spawnerState = SpawnerState.Cooldown;
                    }
                    break;
            }
        }

        private void OnDestroy()
        {
            _buildingPlacer.OnGameOver -= BuildingPlacer_OnGameOver;
        }

        private void BuildingPlacer_OnGameOver()
        {
            OnTextChange?.Invoke("");
            Destroy(gameObject);
        }

        public void ForgetEnemy(Enemy enemy)
        {
            _spawnedEnemies.Remove(enemy);
        }
    }
}
