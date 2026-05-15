using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BattleMenuController : MonoBehaviour
{
    public Button[] buttons;
    private int index = 0;

    void Start() => Highlight(0);

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.rightArrowKey.wasPressedThisFrame) Move(1);
        if (keyboard.leftArrowKey.wasPressedThisFrame) Move(-1);
        if (keyboard.downArrowKey.wasPressedThisFrame) Move(2);
        if (keyboard.upArrowKey.wasPressedThisFrame) Move(-2);

        if (keyboard.enterKey.wasPressedThisFrame)
            buttons[index].onClick.Invoke();
    }

    void Move(int dir)
    {
        index += dir;

        if (index < 0) index = buttons.Length - 1;
        if (index >= buttons.Length) index = 0;

        Highlight(index);
    }

    void Highlight(int i)
    {
        EventSystem.current.SetSelectedGameObject(buttons[i].gameObject);
    }
}