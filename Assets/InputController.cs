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
