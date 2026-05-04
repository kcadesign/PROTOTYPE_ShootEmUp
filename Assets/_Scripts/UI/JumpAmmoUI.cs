using UnityEngine;
using UnityEngine.UIElements;

public class JumpAmmoUI : MonoBehaviour
{
    [SerializeField] private UIDocument _uIDocument;

    private VisualElement _ammoContainer;
    private VisualElement _ammoBlockContainer;

    private int _currentAmmo;
    private int _maxAmmo;
    private int _maxAmmoLimit;

    private void Awake()
    {
        _ammoContainer = _uIDocument.rootVisualElement.Q<VisualElement>("AmmoContainer");
        _ammoBlockContainer = _uIDocument.rootVisualElement.Q<VisualElement>("AmmoBlockContainer");
    }

    private void OnEnable()
    {
        Jump.OnMaxAirJumpsChanged += Jump_OnMaxAirJumpsChanged;
        Jump.OnCurrentAirJumpAmountChanged += Jump_OnCurrentAirJumpAmountChanged;
    }

    private void OnDisable()
    {
        Jump.OnMaxAirJumpsChanged -= Jump_OnMaxAirJumpsChanged;
        Jump.OnCurrentAirJumpAmountChanged -= Jump_OnCurrentAirJumpAmountChanged;
    }

    private void Start()
    {
        _maxAmmoLimit = _ammoContainer.childCount;
    }

    private void Jump_OnMaxAirJumpsChanged(int maxAirJumps)
    {
        UpdateMaxAmmo(maxAirJumps);
    }

    private void Jump_OnCurrentAirJumpAmountChanged(int currentAirJumpAmount)
    {
        UpdateCurrentAmmo(currentAirJumpAmount);
    }

    private void UpdateMaxAmmo(int maxAmmo)
    {
        for (int i = 0; i < _ammoContainer.childCount; i++)
        {
            // set the number of children that display:flex to be the same as the max health, set the rest as none
            if (i < maxAmmo)
            {
                _ammoContainer[i].style.display = DisplayStyle.Flex;
            }
            else
            {
                _ammoContainer[i].style.display = DisplayStyle.None;
            }
        }
    }

    private void UpdateCurrentAmmo(int currentAmmo)
    {
        for (int i = 0; i < _ammoContainer.childCount; i++)
        {
            // set the number of children that display:flex to be the same as the max health, set the rest as none
            if (i < currentAmmo)
            {
                // get the child of _healthContainer[i] and set it to visible
                _ammoContainer[i][0].visible = true;
            }
            else
            {
                // get the child of _healthContainer[i] and set it to not visible
                _ammoContainer[i][0].visible = false;
            }
        }
    }

}
