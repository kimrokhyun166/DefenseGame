using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI 연결")]
    [SerializeField] private Text moneyText; // 돈 표시용 UI
    [SerializeField] private Money money;    // Money 스크립트 참조

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (money != null)
            UpdateMoneyUI(money.GetCurrentMoney());
    }

    //돈 UI 업데이트
    public void UpdateMoneyUI(int amount)
    {
        if (moneyText != null)
            moneyText.text = $"Gold: {amount}";
    }

    // 외부에서 Money 참조용
    public Money GetMoney() => money;
}