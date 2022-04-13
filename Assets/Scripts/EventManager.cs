using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public event Action onkeyStacked;
    public void KeyStacked()
    {
        if(onkeyStacked != null)
        {
            onkeyStacked();
        }
    }

    public event Action onKeyMoved;
    public void KeyMoved()
    {
        if(onKeyMoved != null)
        {
            onKeyMoved();
        }
    }

    public event Action onObstacleTriggered;
    public void ObstacleTriggered()
    {
        if(onObstacleTriggered != null)
        {
            onObstacleTriggered();
        }
    }

    public event Action onPrinterTriggered;
    public void PrinterTriggered()
    {
        if(onPrinterTriggered != null)
        {
            onPrinterTriggered();
        }
    }

    public event Action onGateTriggered;
    public void GateTriggered()
    {
        if(onGateTriggered != null)
        {
            onGateTriggered();
        }
    }

    public event Action onTouchToStart;
    public void TouchToStart()
    {
        if(onTouchToStart != null)
        {
            onTouchToStart();
        }
    }
}
