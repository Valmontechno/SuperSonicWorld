using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    GameManager gameManager;

    [Space(10)]
    [SerializeField] TextMeshProUGUI ringCouter;
    [SerializeField] Animator ringCounterAnimator;

    [Space(10)]
    [SerializeField] Animator damageAnimator;

    void Start()
    {
        gameManager = GameManager.Instance;

        gameManager.RingChanghed += UpdateRingCounter;
        gameManager.DamageTaken += OnDamageTaken;
    }

    void OnDestroy()
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
