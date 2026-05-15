using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BattleMenuController : MonoBehaviour
{
    public Button[] buttons;

    private int index = 0;

    private void Start()
    {
        Highlight(0);
    }

    private void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        // Horizontal movement (Attack ↔ Magic, Item ↔ Talk)
        if (keyboard.rightArrowKey.wasPressedThisFrame)
            Move(1);

        if (keyboard.leftArrowKey.wasPressedThisFrame)
            Move(-1);

        // Vertical movement (row switching)
        if (keyboard.downArrowKey.wasPressedThisFrame)
            Move(2);

        if (keyboard.upArrowKey.wasPressedThisFrame)
            Move(-2);

        // Confirm selection
        if (keyboard.enterKey.wasPressedThisFrame ||
            keyboard.numpadEnterKey.wasPressedThisFrame)
        {
            buttons[index].onClick.Invoke();
        }
    }

    void Move(int dir)
    {
        index += dir;

        if (index < 0)
            index = buttons.Length - 1;

        if (index >= buttons.Length)
            index = 0;

        Highlight(index);
    }

    void Highlight(int i)
    {
        EventSystem.current.SetSelectedGameObject(buttons[i].gameObject);
    }
}