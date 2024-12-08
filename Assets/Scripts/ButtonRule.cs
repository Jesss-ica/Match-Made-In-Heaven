using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonRule : MonoBehaviour
{

    public Button[] button;

    GameObject selected;

    BaseInputModule inputModule;
    private EventSystem eventSystem;


    // Start is called before the first frame update
    void Awake()
    {
        if (eventSystem == null)
        {
            eventSystem = GameObject.FindObjectOfType<EventSystem>();
            eventSystem.SetSelectedGameObject(null);
            if (eventSystem == null)
            {
                Debug.Log("Event system is null");
            }

            eventSystem.SetSelectedGameObject(button[0].gameObject);
            eventSystem.firstSelectedGameObject = button[0].gameObject;
            selected = button[0].gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetFocusObject() != null)
        {
            eventSystem.SetSelectedGameObject(GetFocusObject());
        }
    }


    private GameObject GetFocusObject()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        if (raycastResults.Count > 0)
        {
            foreach (var go in raycastResults)
            {
                if (go.gameObject.GetComponent<Button>() != null)
                {
                    selected = go.gameObject;
                    return selected;
                }
            }
        }

        return null;
    }
}
