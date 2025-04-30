using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    GameManager gameManager;
    Animator animator;

    [Space(10)]
    [SerializeField] float invulnerabilityTime;
    bool invulnerable = false;

    [Space(10)]
    [SerializeField] int ringsEjectedMax;
    [SerializeField] float ejectionForce;
    [SerializeField] GameObject ringPrefab;
    [SerializeField] AudioSource DamageAudioSource;

    private void Start()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
    }

    public void CauseDamage()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            StartCoroutine(DamageCoroutine());
        }
    }

    IEnumerator DamageCoroutine()
    {
        animator.SetBool("Damage", invulnerable);
        DamageAudioSource.Play();

        for (int i = 0; i < Mathf.Min(ringsEjectedMax, gameManager.Rings); i++)
        {
            GameObject ring = Instantiate(ringPrefab, transform.position, Quaternion.identity);
            Vector2 direction = UnityEngine.Random.insideUnitCircle;
            direction.y = Mathf.Abs(direction.y);
            ring.GetComponent<Rigidbody2D>().AddForce(direction * ejectionForce, ForceMode2D.Impulse);
        }
        gameManager.LoseAllRings();

        yield return new WaitForSeconds(invulnerabilityTime);

        invulnerable = false;
        animator.SetBool("Damage", invulnerable);
    }
}
