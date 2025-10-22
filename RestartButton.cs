using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public static RestartButton instance;

    [SerializeField] private float gameOverDelay = 2f; // 게임 오버 후 메인 메뉴로 돌아가기 전 대기 시간

    private void Awake()
    {
        // 싱글톤 패턴 적용
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 게임 시작 시 초기화 작업
        Debug.Log("게임 매니저 초기화 완료");
    }

    // 게임 시작하기 (메인 메뉴에서 게임 화면으로)
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // 실제 게임 씬 이름으로 변경해야 함
    }

    // 메인 메뉴로 돌아가기
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // 게임 오버 처리
    public void GameOver()
    {
        Debug.Log("게임 오버!");
        // 게임 오버 UI 표시 등의 작업

        // 지연 시간 후 메인 메뉴로 돌아가기
        Invoke("ReturnToMainMenu", gameOverDelay);
    }

    // 게임 일시정지
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    // 게임 재개
    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    // 게임 종료
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}