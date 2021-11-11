using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    bool isTiming;
    float duration;
    float startTime;
    float currentTime;

    // Starts the timer with initial values
    public void StartTimer(float duration)
    {
        currentTime = 0;
        this.duration = duration;
        startTime = Time.realtimeSinceStartup;

        isTiming = true;
    }

    public void AddTime(float time)
    {
        duration += time;
    }

    public void StopTimer()
    {
        isTiming = false;
    }

    // Gets the current ticked time
    public float Tick()
    {
        if (isTiming)
        {
            currentTime = Time.realtimeSinceStartup - startTime;
            float countDown = duration - currentTime;

            return countDown;
        }
        else
        {
            return currentTime;
        }
    }

    public bool GetIsTiming()
    {
        return isTiming;
    }
}
