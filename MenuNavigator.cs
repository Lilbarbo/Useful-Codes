using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{

    [SerializeField] private Selectable firstInputToSelect;
    [SerializeField] private EventSystem eventSystem;
    void Start()
    {
        eventSystem = EventSystem.current;  

        if(firstInputToSelect != null)
        {
            firstInputToSelect.Select();
        }
    }

    private void Update()
    {
        GameObject currentSelectable = eventSystem.currentSelectedGameObject;
        Selectable selectable = currentSelectable.GetComponent<Selectable>();

        if(selectable != null)
        {
            if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                Selectable previousSelectable = selectable.FindSelectableOnUp();
                if (previousSelectable != null)
                {
                    previousSelectable.Select();
                }
            }

            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                Selectable nextSelectable = selectable.FindSelectableOnDown();
                if (nextSelectable != null)
                {
                    nextSelectable.Select();
                }
                else
                {
                    firstInputToSelect.Select();
                }
            }
        }
    }
}
