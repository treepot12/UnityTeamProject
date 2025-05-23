using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;


public class PuzzleUIManager : MonoBehaviour
{
    public TMP_InputField answerInput;
    public GameObject hintPanel;
    public GameObject wrongPopup;
    public GameObject correctPopup;
    public Button hintButton;
    public Button closeHintButton;
    public Button laterButton;
    public Button confirmButton;
    public TMP_Text confirmButtonText;

    //private bool isAnswerInputActive = false;
    void Start()
    {
        hintButton.onClick.AddListener(OnHintClicked);
        closeHintButton.onClick.AddListener(OnCloseHintClicked);
        laterButton.onClick.AddListener(OnLaterClicked);
        confirmButton.onClick.AddListener(OnConfirmClicked);

        hintPanel.SetActive(false);
        if (wrongPopup == null)
        {
            Debug.LogError(" wrongPopup�� Inspector���� ����Ǿ� ���� �ʽ��ϴ�!");
        }
        else
        {
            Debug.Log(" wrongPopup ���� ����: " + wrongPopup.name);
            wrongPopup.SetActive(false); // ����
        }
        if (correctPopup == null)
        {
            Debug.LogError("correctPopup�� Inspector���� ����Ǿ� ���� �ʽ��ϴ�!");
        }
        else
        {
            correctPopup.SetActive(false);
        }

        //wrongPopup.SetActive(false);
        answerInput.gameObject.SetActive(false);
        
    }


    void OnHintClicked(){ hintPanel.SetActive(true);}
    void OnCloseHintClicked(){hintPanel.SetActive(false);}
    void OnLaterClicked()
    { 
        //Debug.Log("���߿� Ǯ�� Ŭ����");
        SceneManager.LoadScene("skipscene");
    }
    
    bool isAnswerInputActive = false;
    public void OnConfirmClicked()
    {
        if (!isAnswerInputActive)
        {
            // �Է�â�� ���� ��Ȱ��ȭ ���� �� ���� ���̰� �ϱ�
            answerInput.text = "";
            answerInput.gameObject.SetActive(true);
            isAnswerInputActive = true;
            confirmButton.GetComponentInChildren<TMP_Text>().text = "����!!!";
            Debug.Log("������ �Է��ϼ���!");
            return;
        }
        //string userAnswer = answerInput.text.Trim();
        string userAnswer = answerInput.text.Trim().ToLower();
        Debug.Log("�Է°�: [" + userAnswer + "]");

        // �Է°��� ��������� ���� (���� ó������ ����)
        if (string.IsNullOrEmpty(userAnswer))
        {
            Debug.Log("���� ������ �Է����� �ʾҽ��ϴ�.");
            return;
        }

        if (userAnswer == "apple")
        {
            Debug.Log("�����Դϴ�!!!");
            
            StartCoroutine(ShowCorrectPopupAndMoveScene());
        }
        else
        {
            Debug.Log("�����Դϴ�!");
            StartCoroutine(ShowWrongPopupAndRetry());
        }
        //Debug.Log("���� Ŭ����");
    }

    IEnumerator ShowWrongPopupAndRetry()
    {
        wrongPopup.SetActive(true); // ���� �˾� 
        yield return new WaitForSeconds(2f); // 2�� ��ٸ�
        wrongPopup.SetActive(false); // �˾� ����

        // ���� �� �ٽ� �ҷ��ͼ� ���� ��õ���
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator ShowCorrectPopupAndMoveScene()
    {
        correctPopup.SetActive(true); // �˾� ���̱�
        yield return new WaitForSeconds(2f); // 2�� ���
        correctPopup.SetActive(false); // ���Ѵٸ� ����
        SceneManager.LoadScene("CorrectScene"); // ���� �� �̵�
    }

}
