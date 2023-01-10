using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent (typeof(Button))]
public class MenuButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] AudioClip menuButtonClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            AudioManager.Instance.PlaySound(menuButtonClick);
        }
    }
}