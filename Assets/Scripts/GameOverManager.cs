using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public Button tryAgainButton;

    void Start()
    {
        tryAgainButton.onClick.AddListener(OnTryAgainClicked);
    }

    void OnTryAgainClicked()
    {
        SceneManager.LoadScene("버튼 구현"); // 문제 푸는 씬 으로 이동
    }
}
