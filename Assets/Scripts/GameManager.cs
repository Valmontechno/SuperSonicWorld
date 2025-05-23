using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public int Rings {  get; set; }

    public Action<int> RingChanghed;
    public Action DamageTaken;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void AddRings(int rings)
    {
        Rings += rings;
        RingChanghed.Invoke(Rings);
    }

    [Obsolete]
    public void LoseRings(int rings)
    {
        Rings -= rings;
        RingChanghed?.Invoke(Rings);
    }

    public void LoseAllRings()
    {
        Rings = 0;
        RingChanghed.Invoke(Rings);
        DamageTaken?.Invoke();
    }
}
