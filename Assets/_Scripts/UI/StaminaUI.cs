using UnityEngine;
using UnityEngine.UIElements;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] private UIDocument _uIDocument;

    private VisualElement _staminaContainer;
    private VisualElement _staminaBlockContainer;

    private int _currentStamina;
    private int _maxStamina;
    private int _maxStaminaLimit;

    private void Awake()
    {
        _staminaContainer = _uIDocument.rootVisualElement.Q<VisualElement>("AmmoContainer");
        _staminaBlockContainer = _uIDocument.rootVisualElement.Q<VisualElement>("AmmoBlockContainer");
    }

    private void OnEnable()
    {
        Stamina.OnCurrentStaminaChanged += Stamina_OnCurrentStaminaChanged;
        Stamina.OnMaxStaminaChanged += Stamina_OnMaxStaminaChanged;
    }

    private void OnDisable()
    {
        Stamina.OnCurrentStaminaChanged -= Stamina_OnCurrentStaminaChanged;
        Stamina.OnMaxStaminaChanged -= Stamina_OnMaxStaminaChanged;
    }

    private void Stamina_OnCurrentStaminaChanged(int amount)
    {
        UpdateCurrentStamina(amount);
    }

    private void Stamina_OnMaxStaminaChanged(int amount)
    {
        UpdateMaxStamina(amount);
    }

    private void Start()
    {
        _maxStaminaLimit = _staminaContainer.childCount;
    }

    private void UpdateMaxStamina(int maxStamina)
    {
        for (int i = 0; i < _staminaContainer.childCount; i++)
        {
            // set the number of children that display:flex to be the same as the max health, set the rest as none
            if (i < maxStamina)
            {
                _staminaContainer[i].style.display = DisplayStyle.Flex;
            }
            else
            {
                _staminaContainer[i].style.display = DisplayStyle.None;
            }
        }
    }

    private void UpdateCurrentStamina(int currentStamina)
    {
        for (int i = 0; i < _staminaContainer.childCount; i++)
        {
            // set the number of children that display:flex to be the same as the max health, set the rest as none
            if (i < currentStamina)
            {
                // get the child of _healthContainer[i] and set it to visible
                _staminaContainer[i][0].visible = true;
            }
            else
            {
                // get the child of _healthContainer[i] and set it to not visible
                _staminaContainer[i][0].visible = false;
            }
        }
    }

}
