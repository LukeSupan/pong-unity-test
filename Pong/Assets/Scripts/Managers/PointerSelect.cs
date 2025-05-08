using UnityEngine;
using UnityEngine.EventSystems;

// Basically, hover becomes selects, this makes controller to mouse switching signficantly more seamless
public class PointerSelect : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
