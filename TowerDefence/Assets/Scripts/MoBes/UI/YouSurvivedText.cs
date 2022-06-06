using nsEnemySpawner;
using TMPro;
using UnityEngine;

namespace nsYouSurvivedText
{
    public class YouSurvivedText : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;

        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _enemySpawner.OnWaveNumberChange += EnemySpawner_OnWaveNumberChange;
        }

        private void EnemySpawner_OnWaveNumberChange(int value)
        {
            string text = (value - 1).ToString();
            text = string.Concat("You survived ", text, " waves");
            _text.SetText(text);
        }
    }
}
