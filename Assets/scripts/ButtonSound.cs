using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonOver);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
    }
}
