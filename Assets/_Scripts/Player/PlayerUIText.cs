using UnityEngine;
using TMPro;

public class PlayerUIText : MonoBehaviour
{
    public PlayerStats PlayerStatsData;

    public TextMeshProUGUI ComboText;
    public TextMeshProUGUI StaminaText;
    private string _staminaIcon = "* ";

    private void OnEnable()
    {
        Stamina.OnCurrentStaminaChanged += Stamina_OnCurrentStaminaChanged;
    }

    private void OnDisable()
    {
        Stamina.OnCurrentStaminaChanged -= Stamina_OnCurrentStaminaChanged;
    }

    private void Update()
    {
       if (PlayerStatsData.GetComboLevel() > 1)
        {
            ComboText.text = PlayerStatsData.GetComboLevel().ToString();
        }
        else
        {
            ComboText.text = "";
        }
    }

    private void Stamina_OnCurrentStaminaChanged(int amount)
    {
        AdjustStaminaText(amount);
    }

    private void AdjustStaminaText(int amount)
    {
        StaminaText.text = "";
        for (int i = 0; i < amount; i++)
        {
            StaminaText.text += _staminaIcon;
        }
    }
}
