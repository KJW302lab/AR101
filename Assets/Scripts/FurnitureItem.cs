using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class FurnitureItem : MonoBehaviour
{
    [SerializeField] private ARPlacementInteractable placement;
    [SerializeField] private GameObject              furniturePrefab;

    private Image  _bg;
    private Toggle _toggle;

    private void Awake()
    {
        _bg = GetComponent<Image>();
        _toggle = GetComponent<Toggle>();

        _toggle.onValueChanged.AddListener(OnValueChanged);

        OnValueChanged(_toggle.isOn);
    }

    private void OnValueChanged(bool isOn)
    {
        if (isOn)
        {
            _bg.color = Color.blue;
            placement.placementPrefab = furniturePrefab;
        }
        else
        {
            _bg.color = Color.white;
        }
    }
}
