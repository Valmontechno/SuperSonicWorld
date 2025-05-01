using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : Collectible
{
    [SerializeField] GameObject ringSound;

    protected override void Collect()
    {
        GameManager.Instance.AddRings(1);
        Instantiate(ringSound, transform.position, Quaternion.identity);
    }
}
