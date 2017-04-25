using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public abstract class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    public enum ItemType
    {
        Usable, Equipable, Stackable, Normal
    }

    public enum ItemState
    {
        Equiped, InInventory, InGame
    }

    public string itemName;

    protected ItemType _itemType;
    protected ItemState _itemState;

    protected InventoryManager _inventoryManager;
    protected Transform _originalSlot;

    void Awake()
    {
        setItemType();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    protected abstract void setItemType();

    public ItemType getItemType()
    {
        return _itemType;
    }

    public void setInventoryManager(InventoryManager newInventoryManager)
    {
        _inventoryManager = newInventoryManager;
    }

    public void setItemState(ItemState newItemState)
    {
        switch (newItemState)
        {
            case ItemState.InGame:
                float offset = 1.0f;

                GetComponent<Collider2D>().enabled = true;
                transform.SetParent(null);
                Vector2 playerPosition = _inventoryManager.player.transform.position;
                transform.position = new Vector2(playerPosition.x + offset, playerPosition.y);
                transform.localScale = Vector3.one;

                break;

            case ItemState.Equiped:
            case ItemState.InInventory:
                transform.SetAsFirstSibling();
                transform.localScale = Vector3.one;
                GetComponent<Collider2D>().enabled = false;

                RectTransform tempRectTransform = GetComponent<RectTransform>();

                tempRectTransform.offsetMin = Vector2.zero;
                tempRectTransform.offsetMax = Vector2.zero;
                tempRectTransform.anchorMin = new Vector2(0, 0);
                tempRectTransform.anchorMax = new Vector2(1, 1);
                tempRectTransform.pivot = new Vector2(0.5f, 0.5f);

                _originalSlot = transform.parent;
                break;
        }

        _itemState = newItemState;
    }

    public ItemState getItemState()
    {
        return _itemState;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalSlot = transform.parent;

        if (_itemState == ItemState.InInventory)
        {
            transform.SetParent(transform.parent.parent.parent.parent);
        }
        else if (_itemState == ItemState.Equiped)
        {
            transform.SetParent(transform.parent.parent);
        }

        transform.position = eventData.position;

        _inventoryManager.tooltip.Deactivate();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_originalSlot);
        transform.position = _originalSlot.position;
        transform.SetAsFirstSibling();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.parent.Equals(_originalSlot))
        {
            _inventoryManager.tooltip.Activate(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _inventoryManager.tooltip.Deactivate();
    }
}
