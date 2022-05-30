using nsEnemySpawner;
using TMPro;
using UnityEngine;

namespace nsEnemySpawnerText
{
    public class EnemySpawnerText : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;

        private TextMeshProUGUI _textMeshPro;

        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
            _enemySpawner.OnTextChange += EnemySpawner_OnTextChange;
        }

        private void EnemySpawner_OnTextChange(string text)
        {
            _textMeshPro.SetText(text);
        }
    }
}
