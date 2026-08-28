using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIHandler : MonoBehaviour
{

    public enum BarType { Health, Mana }

    [Header("UI Elements")]
    [SerializeField] private Image HealthBarUI;
    [SerializeField] private Image ManaBarUI;

    public void SetFill(float normalizedValue, BarType type)
    {
        switch (type)
        {
            case BarType.Health:
                if (HealthBarUI != null)
                {
                    HealthBarUI.fillAmount = normalizedValue;
                }
                break;
            case BarType.Mana:
                if (ManaBarUI != null)
                {
                    ManaBarUI.fillAmount = normalizedValue;
                }
                break;
            default:
                Debug.LogWarning("Invalid type specified for SetFill: " + type);
                break;
        }
    }
}
