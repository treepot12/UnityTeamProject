using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Linq;


public class PuzzleUIManager : MonoBehaviour
{
    private int wrongCount = 0; // 틀린 횟수 저장
    int maxWrongCount = 3; // 최대 틀릴 수 있는 횟수

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
    private string[] correctAnswers = { "문자열에서 처음으로 한 번만 등장한 문자를 찾아냄", "o(n)", "a"};
    void Start()
    {
        isAnswerInputActive = false;
        answerInput.gameObject.SetActive(false);
        wrongCount = 0;
        if (confirmButton == null)
        {
            Debug.LogError(" confirmButton이 Inspector에서 연결되어 있지 않습니다!");
        }
        else
        {
            Debug.Log(" confirmButton 연결 성공: " + confirmButton.name);
        }
        hintButton.onClick.AddListener(OnHintClicked);
        closeHintButton.onClick.AddListener(OnCloseHintClicked);
        laterButton.onClick.AddListener(OnLaterClicked);
        confirmButton.onClick.AddListener(OnConfirmClicked);

        hintPanel.SetActive(false);
        if (wrongPopup == null)
        {
            Debug.LogError(" wrongPopup이 Inspector에서 연결되어 있지 않습니다!");
        }
        else
        {
            Debug.Log(" wrongPopup 연결 성공: " + wrongPopup.name);
            wrongPopup.SetActive(false); // 숨김
        }
        if (correctPopup == null)
        {
            Debug.LogError("correctPopup이 Inspector에서 연결되어 있지 않습니다!");
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
        //Debug.Log("나중에 풀기 클릭됨");
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
        Debug.Log("결정 버튼 눌림!");
        //if (isCoroutineRunning || correctPopup.activeSelf || wrongPopup.activeSelf) return;
        if (isCoroutineRunning) return;

        if (!isAnswerInputActive || !answerInput.gameObject.activeSelf)
        {
            
            answerInput.text = "";
            answerInput.gameObject.SetActive(true);
            
            isAnswerInputActive = true;
            confirmButtonText.text = "결정!!!";
            return;
        }

        string userAnswer = answerInput.text.Trim().ToLower();
       

        // 입력값이 비어있으면 무시 (오답 처리하지 않음)
        if (string.IsNullOrEmpty(userAnswer))
        {
            Debug.Log("아직 정답을 입력하지 않았습니다.");
            return;
        }

        bool isCorrect = correctAnswers.Any(ans => ans.ToLower() == userAnswer);

        if (isCorrect)
        {
            Debug.Log("정답입니다!!!");
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
        wrongPopup.SetActive(true); // 오답 팝업 
        yield return new WaitForSeconds(2f); // 2초 기다림
        wrongPopup.SetActive(false); // 팝업 끄기

        
        // 입력만 초기화
        answerInput.text = "";
       
        confirmButtonText.text = "결정"; 
      
        isCoroutineRunning = false;
    }

    IEnumerator ShowCorrectPopupAndMoveScene()
    {
        correctPopup.SetActive(true); // 팝업 보이기
        yield return new WaitForSeconds(2f); // 2초 대기
        correctPopup.SetActive(false); // 원한다면 끄기
        wrongCount = 0; // 다음 퍼즐용 초기화
        SceneManager.LoadScene("CorrectScene"); // 정답 씬 이동
    }

    IEnumerator ShowGameOverPopupAndMoveScene()
    {
        isCoroutineRunning = true;
        wrongPopup.SetActive(true); // 오답 팝업 보여주기
        yield return new WaitForSeconds(2f);
        wrongPopup.SetActive(false);
        isCoroutineRunning = false;
        SceneManager.LoadScene("GameOverScene");
    }




}
