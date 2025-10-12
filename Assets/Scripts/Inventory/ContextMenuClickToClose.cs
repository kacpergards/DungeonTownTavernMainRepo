using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class ContextMenuClickToClose : MonoBehaviour, IPointerClickHandler
{
    [Header("Events")]
    public UnityEngine.Events.UnityEvent OnClickedOutside;
    
    private bool clickedInside = false;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickedInside = false;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            if (!clickedInside)
            {
                OnClickedOutside.Invoke();
                InventoryItemContextMenu menu = GetComponent<InventoryItemContextMenu>();
                menu.DestroySelf();
            }
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        clickedInside = true;
    }
}