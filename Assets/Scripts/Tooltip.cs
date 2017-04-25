using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {
    
    [SerializeField]
    private Text _info;
    private Item _item;
    private string _data;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void Activate(Item item)
    {
        _item = item;
        ConstructDataString();
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void ConstructDataString()
    {
        _data = _item.itemName;
        _info.text = _data;
    }
}
