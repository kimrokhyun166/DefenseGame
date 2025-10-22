using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public static RestartButton instance;

    [SerializeField] private float gameOverDelay = 2f; // ���� ���� �� ���� �޴��� ���ư��� �� ��� �ð�

    private void Awake()
    {
        // �̱��� ���� ����
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
        // ���� ���� �� �ʱ�ȭ �۾�
        Debug.Log("���� �Ŵ��� �ʱ�ȭ �Ϸ�");
    }

    // ���� �����ϱ� (���� �޴����� ���� ȭ������)
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // ���� ���� �� �̸����� �����ؾ� ��
    }

    // ���� �޴��� ���ư���
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // ���� ���� ó��
    public void GameOver()
    {
        Debug.Log("���� ����!");
        // ���� ���� UI ǥ�� ���� �۾�

        // ���� �ð� �� ���� �޴��� ���ư���
        Invoke("ReturnToMainMenu", gameOverDelay);
    }

    // ���� �Ͻ�����
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    // ���� �簳
    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    // ���� ����
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}