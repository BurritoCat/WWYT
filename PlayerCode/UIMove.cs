using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIMove : MonoBehaviour
{
    [Tooltip("What canvas object has the buttons?")]public GameObject ButtonHolder;
    [SerializeField,Tooltip("What button does the UI currently have selected?")] private GameObject currentButtonSelected;

    void Awake()
    {
        currentButtonSelected = null;
    }

    public void MoveOption(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Vector2 keyPress = (context.ReadValue<Vector2>());

            if(keyPress.x != 0.0f)
            {
                float x = keyPress.x;

                if (currentButtonSelected != null)
                {
                    currentButtonSelected.transform.GetComponent<Image>().color = new Color32(217, 227, 228, 255);
                }

                if (x > 0)
                {
                    //Select new option
                    currentButtonSelected = ButtonHolder.transform.GetChild(2).gameObject;
                    currentButtonSelected.transform.GetComponent<Image>().color = new Color32(197, 207, 208, 255);
                }
                else
                {
                    //Select new option
                    currentButtonSelected = ButtonHolder.transform.GetChild(1).gameObject;
                    currentButtonSelected.transform.GetComponent<Image>().color = new Color32(197, 207, 208, 255);
                }
            }
        }
    }

    public void Select(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentButtonSelected == null)
                return;

            GameObject temp = currentButtonSelected;
            temp.transform.GetComponent<Buttons>().trigger();
            currentButtonSelected = null;
        }
    }
}
