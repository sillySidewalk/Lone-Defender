using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


/*
 * This is given text by something else, and displays it
 */
public class hover_text_ui : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject hover_obj; // this includes the text and the background
    [SerializeField] TextMeshProUGUI text_mesh;
    public void add_text(string s)
    {
        text_mesh.text = s;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover_obj.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover_obj.gameObject.SetActive(false);
    }
}
