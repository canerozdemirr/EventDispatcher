using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleLogger : MonoBehaviour
{
    private void OnEnable()
    {
        EventDispatcher.Instance.Subscribe<ConsoleLogEvent>(DisplayLogMessage);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.Unsubscribe<ConsoleLogEvent>(DisplayLogMessage);
    }

    private void DisplayLogMessage(ConsoleLogEvent logEvent)
    {
        Debug.Log(logEvent.LogMessage);
    }
}
