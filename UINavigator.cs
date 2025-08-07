using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UINavigator : MonoBehaviour
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
        GameObject currentSelectable = eventSystem.currentSelectedGameObject; //pega o GameObject que está sendo selecionado atualmente
        Selectable selectable = currentSelectable.GetComponent<Selectable>(); //pega o selecionável dentro desse objeto

        if(selectable != null)
        {
            if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                Selectable previousSelectable = selectable.FindSelectableOnUp(); //verifica se há um selecionável acima
                if (previousSelectable != null)
                {
                    previousSelectable.Select();
                }
            }

            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                Selectable nextSelectable = selectable.FindSelectableOnDown(); //verifica se há um selecionável abaixo
                if (nextSelectable != null)
                {
                    nextSelectable.Select();
                }
                else
                {
                    firstInputToSelect.Select(); //caso não, retorna ao primeiro selecionável
                }
            }
        }
    }
}


