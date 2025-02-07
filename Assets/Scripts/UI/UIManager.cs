using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Simple UI for visualizing player health.
/// </summary>
public class UIManager : MonoBehaviour
{
    private VisualElement _root;
    private Label _healthLabel;
    public Health target;
    void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _healthLabel = _root.Q<Label>("healthLabel");
    }
    void Update()
    {
        _healthLabel.text = $"HEALTH: {target.GetHealth()}/{target.GetMaxHealth()}";
    }
}
