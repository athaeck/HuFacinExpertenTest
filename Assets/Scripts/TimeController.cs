using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private static TimeController _instance = null;
    public static TimeController Instance { get { return _instance; } }

    [SerializeField]
    private int _timeSinceStart = 0;

    void Awake()
    {
        _instance = this;
    }

    public void InCreaseTimeSinceStartet(int timeSince)
    {
        _timeSinceStart += timeSince;
    }

    public int GetTimeSinceStart()
    {
        return _timeSinceStart;
    }
}
