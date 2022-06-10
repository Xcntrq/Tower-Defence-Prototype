using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace nsButtonPlayAgain
{
    public class ButtonPlayAgain : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("GameplayScene"); });
        }
    }
}
