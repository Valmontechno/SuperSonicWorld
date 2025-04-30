using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI ringCouter;
    [SerializeField] private Animator ringCounterAnimator;

    [Space(10)]
    [SerializeField] private Animator damageAnimator;

    private void Start()
    {
        gameManager = GameManager.Instance;

        gameManager.RingChanghed += UpdateRingCounter;
        gameManager.DamageTaken += OnDamageTaken;
    }

    private void OnDestroy()
    {
        gameManager.RingChanghed -= UpdateRingCounter;
        gameManager.DamageTaken -= OnDamageTaken;
    }

    void UpdateRingCounter(int rings)
    {
        ringCouter.text = "Rings: " + rings.ToString();
        ringCounterAnimator.SetBool("Alert", rings <= 0);
    }

    void OnDamageTaken()
    {
        damageAnimator.SetTrigger("Damage");
    }
}
