using UnityEngine;
using UnityEngine.UI;

public class MilitaryCamp : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private Button summonButton;
    [SerializeField] private Button closeButton;

    [Header("소환 관련 설정")]
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int summonCost = 50;

    private Money moneySystem;  // Money 스크립트 참조
    private bool isUIOpen = false;

    private void Start()
    {
        if (uiPanel != null)
            uiPanel.SetActive(false);

        // 버튼 이벤트 등록
        if (summonButton != null)
            summonButton.onClick.AddListener(SummonUnit);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseUI);

        // GameManager를 통해 Money 스크립트 가져오기
        moneySystem = GameManager.Instance.GetMoney();
    }

    private void Update()
    {
        // 돈이 부족하면 자동으로 UI 비활성화하는 등 기능 추가 가능
    }

    private void OnMouseDown()
    {
        if (isUIOpen)
            CloseUI();
        else
            OpenUI();
    }

    private void OpenUI()
    {
        if (uiPanel != null)
            uiPanel.SetActive(true);
        isUIOpen = true;
    }

    public void CloseUI()
    {
        if (uiPanel != null)
            uiPanel.SetActive(false);
        isUIOpen = false;
    }

    private void SummonUnit()
    {
        if (unitPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("[MilitaryCamp] UnitPrefab 또는 SpawnPoint가 설정되지 않음!");
            return;
        }

        if (moneySystem == null)
        {
            Debug.LogError("[MilitaryCamp] Money 시스템이 연결되지 않음!");
            return;
        }

        // 💰 돈이 50 이상이면 소환
        if (moneySystem.SpendMoney(summonCost))
        {
            Instantiate(unitPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log($"[MilitaryCamp] 유닛 소환 완료! (-{summonCost} Gold)");
            CloseUI();
        }
    }
}
