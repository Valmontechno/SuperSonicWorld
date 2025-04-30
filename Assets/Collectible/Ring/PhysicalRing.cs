using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicalRing : MonoBehaviour
{
    [SerializeField] Collider2D triggerCollider;
    [SerializeField] float enableDelay;
    [SerializeField] float warnDelay;
    [SerializeField] float destroyDelay;

    private void Start()
    {
        StartCoroutine(ActivateComponent());
    }

    IEnumerator ActivateComponent()
    {
        yield return new WaitForSeconds(enableDelay);
        triggerCollider.enabled = true;

        yield return new WaitForSeconds(warnDelay);
        GetComponentInChildren<Animator>().SetTrigger("Warn");

        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
