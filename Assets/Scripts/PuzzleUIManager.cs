using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Linq;


public class PuzzleUIManager : MonoBehaviour
{
    private int wrongCount = 0; // Ʋ�� Ƚ�� ����
    int maxWrongCount = 3; // �ִ� Ʋ�� �� �ִ� Ƚ��

    public TMP_InputField answerInput;
    public GameObject hintPanel;
    public GameObject wrongPopup;
    public GameObject correctPopup;
    public Button hintButton;
    public Button closeHintButton;
    public Button laterButton;
    public Button confirmButton;
    public TMP_Text confirmButtonText;
    public TMP_Text lifeText;

    private bool isAnswerInputActive = false;
    private string[] correctAnswers = { "���ڿ����� ó������ �� ���� ������ ���ڸ� ã�Ƴ�", "o(n)", "a"};
    void Start()
    {
        isAnswerInputActive = false;
        answerInput.gameObject.SetActive(false);
        wrongCount = 0;
        if (confirmButton == null)
        {
            Debug.LogError(" confirmButton�� Inspector���� ����Ǿ� ���� �ʽ��ϴ�!");
        }
        else
        {
            Debug.Log(" confirmButton ���� ����: " + confirmButton.name);
        }
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

        answerInput.gameObject.SetActive(false);
        UpdateLifeText();
    }


    void OnHintClicked(){ hintPanel.SetActive(true);}
    void OnCloseHintClicked(){hintPanel.SetActive(false);}

    void OnLaterClicked()
    { 
        //Debug.Log("���߿� Ǯ�� Ŭ����");
        SceneManager.LoadScene("skipscene");
    }
    
   
    void UpdateLifeText()
    {
        int remain = Mathf.Max(0, maxWrongCount - wrongCount);
        lifeText.text = $"Life: {remain}/{maxWrongCount}";
    }

    private bool isCoroutineRunning = false;

    public void OnConfirmClicked()
    {
        Debug.Log("���� ��ư ����!");
        //if (isCoroutineRunning || correctPopup.activeSelf || wrongPopup.activeSelf) return;
        if (isCoroutineRunning) return;

        if (!isAnswerInputActive || !answerInput.gameObject.activeSelf)
        {
            
            answerInput.text = "";
            answerInput.gameObject.SetActive(true);
            
            isAnswerInputActive = true;
            confirmButtonText.text = "����!!!";
            return;
        }

        string userAnswer = answerInput.text.Trim().ToLower();
       

        // �Է°��� ��������� ���� (���� ó������ ����)
        if (string.IsNullOrEmpty(userAnswer))
        {
            Debug.Log("���� ������ �Է����� �ʾҽ��ϴ�.");
            return;
        }

        bool isCorrect = correctAnswers.Any(ans => ans.ToLower() == userAnswer);

        if (isCorrect)
        {
            Debug.Log("�����Դϴ�!!!");
            StartCoroutine(ShowCorrectPopupAndMoveScene());
        }
        else
        {
            
            wrongCount++;
            UpdateLifeText();

            if (wrongCount >= maxWrongCount)
            {
                
                StartCoroutine(ShowGameOverPopupAndMoveScene());
            }
            else
            {
                StartCoroutine(ShowWrongPopupAndOnly());
            }
        }

    }

    IEnumerator ShowWrongPopupAndOnly()
    {
        isCoroutineRunning = true;
        wrongPopup.SetActive(true); // ���� �˾� 
        yield return new WaitForSeconds(2f); // 2�� ��ٸ�
        wrongPopup.SetActive(false); // �˾� ����

        
        // �Է¸� �ʱ�ȭ
        answerInput.text = "";
       
        confirmButtonText.text = "����"; 
      
        isCoroutineRunning = false;
    }

    IEnumerator ShowCorrectPopupAndMoveScene()
    {
        correctPopup.SetActive(true); // �˾� ���̱�
        yield return new WaitForSeconds(2f); // 2�� ���
        correctPopup.SetActive(false); // ���Ѵٸ� ����
        wrongCount = 0; // ���� ����� �ʱ�ȭ
        SceneManager.LoadScene("CorrectScene"); // ���� �� �̵�
    }

    IEnumerator ShowGameOverPopupAndMoveScene()
    {
        isCoroutineRunning = true;
        wrongPopup.SetActive(true); // ���� �˾� �����ֱ�
        yield return new WaitForSeconds(2f);
        wrongPopup.SetActive(false);
        isCoroutineRunning = false;
        SceneManager.LoadScene("GameOverScene");
    }




}
