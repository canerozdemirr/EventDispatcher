
using System;
using Interfaces;

public class ConsoleLogEvent : IEvent
{
    public string LogMessage;

    public ConsoleLogEvent(string logMessage)
    {
        LogMessage = logMessage;
    }
}
