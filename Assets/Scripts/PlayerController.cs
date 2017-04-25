using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour {

    public float displayTime = 1.0f;
    public GameObject messageCanvas;
    public Text infoMessage;

    #region InventoryScreenLogic Variables
    [Serializable]
    public struct InventoryCanvas
    {
        public GameObject equipScreen;
        public Toggle equipToggleButton;
        public GameObject inventoryScreen;
        public Toggle inventoryToggleButton;
    }

    public InventoryCanvas inventoryCanvas;
    #endregion

    [SerializeField]
    private InventoryManager _inventoryManager;

    void Awake()
    {
        DisableInventoryScreens();
        DisableMessage();

        _inventoryManager.UpdateInventorySize();
    }

    void Update()
    {
        CheckForScreenToggle();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            _inventoryManager.AddItem(other.gameObject.GetComponent<Item>());
        }
    }

    #region InventoryScreenLogic
    public void CheckForScreenToggle()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            inventoryCanvas.inventoryToggleButton.isOn = !inventoryCanvas.inventoryToggleButton.isOn;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            inventoryCanvas.equipToggleButton.isOn = !inventoryCanvas.equipToggleButton.isOn;
        }
    }

    public void DisableInventoryScreens()
    {
        inventoryCanvas.inventoryScreen.SetActive(false);
        inventoryCanvas.inventoryToggleButton.isOn = false;
        inventoryCanvas.equipScreen.SetActive(false);
        inventoryCanvas.equipToggleButton.isOn = false;
    }

    public void ToggleScreen(GameObject screenGameObject)
    {
        screenGameObject.SetActive(!screenGameObject.activeSelf);
    }
    #endregion

    private void DisableMessage()
    {
        messageCanvas.SetActive(false);
    }

    public void DisplayMessage(string newMessage)
    {
        messageCanvas.SetActive(true);
        infoMessage.text = newMessage;

        Invoke("DisableMessage", displayTime);
    }
}
