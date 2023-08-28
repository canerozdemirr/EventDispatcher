# EventDispatcher for Unity

## Overview

An event dispatcher which applies the Observer Pattern, written in C# for Unity.

## Features

- Type-safe event subscription and dispatching.
- Easy to integrate into existing Unity projects.
- Singleton-based for convenient global access.
- Supports multiple subscribers for the same event type.
- Utility methods for easy management of event subscriptions.

## Table of Contents

- [Installation](#installation)
- [Getting Started](#getting-started)
  - [Subscribing to an Event](#subscribing-to-an-event)
  - [Publishing an Event](#publishing-an-event)
  - [Unsubscribing from an Event](#unsubscribing-from-an-event)
- [API Reference](#api-reference)
- [Contributing](#contributing)
- [License](#license)

## Installation

1. Clone this repository or download the ZIP file.
2. Move the `EventDispatcher.cs` file into your Unity project's `Scripts` folder.
3. You're all set!

## Getting Started

For the demonstration of the event bus, I will make a simple event which will help us logging messages in the console. Here is the simple event that we will use: 

```csharp
public struct ConsoleLogEvent
{
    public string LogMessage;

    public ConsoleLogEvent(string logMessage)
    {
        LogMessage = logMessage;
    }
}
```

I picked struct as the type of event because I like the performance cost deduction when having to define more events as the game grows. It will also allow me to not worry about the null checks because it is immutable. However, you are free to define it with a different type. The event dispatcher does not worry about the type because it supports generic types. The decision is up to you.

### Subscribing to an Event

To subscribe to an event of type `T`, use the `Subscribe<T>` method. Here's how to subscribe to a `ConsoleLogEvent` event:

```csharp
EventDispatcher.Instance.Subscribe<ConsoleLogEvent>(DisplayLogMessage);
```

Your handler method should look like this:

```csharp
private void DisplayLogMessage(ConsoleLogEvent logEvent)
    {
        Debug.Log(logEvent.LogMessage);
    }
```

Now, your handler method is registered to the event and will be called whenever the event is dispatched. (We will get to the dispatch method soon.)

### Unsubscribing from an Event

To unsubscribe from an event of type `T`, use the `Unsubscribe<T>` method. Here's how to unsubscribe from a `ConsoleLogEvent` event:

```csharp
EventDispatcher.Instance.Unsubscribe<ConsoleLogEvent>(DisplayLogMessage);
```

### Dispatching an Event

To dispatch an event of type `T`, use the `Dispatch<T>` method. Here's how to dispatch a `ConsoleLogEvent` event:

```csharp
EventDispatcher.Instance.Dispatch(new ConsoleLogEvent("Hello world"));
```

Now, we have an input controller that will dispatch our event whenever we left click our mouse: 

```csharp
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] 
    private string _logMessage;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            EventDispatcher.Instance.Dispatch(new ConsoleLogEvent(_logMessage));
        }
    }
}
```

We will create another class which will serve as the class that will be responsible for displaying console messages:

```csharp
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
```

This event dispatcher is a basic but powerful implementation of the Observer Pattern. The primary aim is to promote loose coupling between different parts of your application. Here's a brief explanation of how it works:

The Observer Pattern establishes a one-to-many relationship between objects, allowing for part-to-part communication without them being tightly bound to each other. In the context of this event dispatcher:

* Subject (Observable): The event, such as ConsoleLogEvent, serves as the subject that can be observed.
* Observers: These are the methods in your code that subscribe to a particular event.

Here are the steps in this pattern:

1- Subscription: Observers subscribe to a particular event type, indicating their interest in being notified when that event occurs.

2- Notification: When the subject changes state (i.e., the event is dispatched), all subscribed observers are automatically notified.

3- Handling: Upon notification, each observer executes its own logic based on the data it receives from the subject.

This approach is highly useful for avoiding tightly coupled systems, making your codebase easier to manage, modify, and extend.

Contributions are welcome! Feel free to submit a pull request or create an issue for bug reports or feature requests.

