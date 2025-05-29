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
        SceneManager.LoadScene("��ư ����"); // ���� Ǫ�� �� ���� �̵�
    }
}
