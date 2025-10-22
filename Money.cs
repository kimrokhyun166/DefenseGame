using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] private int currentMoney = 0;      // 현재 돈
    [SerializeField] private int incomeAmount = 10;     // 주기당 벌리는 금액
    [SerializeField] private float incomeInterval = 5f; // 돈 벌리는 주기(초)

    private void Start()
    {
        InvokeRepeating(nameof(AddIncome), 1f, incomeInterval);
    }

    private void AddIncome()
    {
        currentMoney += incomeAmount;
        GameManager.Instance.UpdateMoneyUI(currentMoney);
        Debug.Log($" 돈 +{incomeAmount} (현재: {currentMoney})");
    }

    // 외부에서 돈 확인용
    public int GetCurrentMoney() => currentMoney;

    // 외부에서 돈 차감용
    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {       
            currentMoney -= amount;
            GameManager.Instance.UpdateMoneyUI(currentMoney);
            return true;
        }
        else
        {
            Debug.Log(" 돈이 부족합니다!");
            return false;
        }
    }
}
