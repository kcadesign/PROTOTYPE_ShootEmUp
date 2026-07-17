using UnityEngine;

public class HandlePlayerLevel : MonoBehaviour
{
    [Header("Component References")]
    public PlayerStats PlayerStatsData;
    public PlayerHealth PlayerHealth;
    public Jump PlayerJump;
    public PlayerMovement PlayerMovement;
    public Grapple PlayerGrapple;

    private int _playerLevel;

    [Header("Level Up Amounts")]
    public float MoveSpeedIncrease = 2f;
    public float GrappleRangeIncrease = 1f;

    private void OnEnable()
    {
        int targetLevel = PlayerStatsData.GetPlayerLevel();

        if (_playerLevel != targetLevel)
        {
            while (_playerLevel < targetLevel)
            {
                _playerLevel++;
                ApplyRewardForLevel(_playerLevel);
            }
        }
        else if (_playerLevel == targetLevel)
        {
            Debug.Log($"Player level is {_playerLevel} on enable. No level up detected.");
        }
    }

    private void ApplyRewardForLevel(int level)
    {
        switch (level)
        {
            case 1:
                PlayerHealth.IncreaseMaxHealth();
                Debug.Log($"Player Level Up! Player Level: {level} - Max Health Increased");
                break;
            case 2:
                PlayerJump.IncreaseMaxAirJumps();
                Debug.Log($"Player Level Up! Player Level: {level} - Max Jumps Increased");
                break;
            case 3:
                PlayerGrapple.IncreaseGrappleRange(GrappleRangeIncrease);
                Debug.Log($"Player Level Up! Player Level: {level} - Grapple Range Increased");
                break;
            case 4:
                PlayerMovement.IncreaseMoveSpeed(MoveSpeedIncrease);
                Debug.Log($"Player Level Up! Player Level: {level} - Move Speed Increased");
                break;
            case 5:
                PlayerHealth.IncreaseMaxHealth();
                Debug.Log($"Player Level Up! Player Level: {level} - Max Health Increased");
                break;
            case 6:
                PlayerJump.IncreaseMaxAirJumps();
                Debug.Log($"Player Level Up! Player Level: {level} - Max Jumps Increased");
                break;
            case 7:
                PlayerGrapple.IncreaseGrappleRange(GrappleRangeIncrease);
                Debug.Log($"Player Level Up! Player Level: {level} - Grapple Range Increased");
                break;
            case 8:
                PlayerMovement.IncreaseMoveSpeed(MoveSpeedIncrease);
                Debug.Log($"Player Level Up! Player Level: {level} - Move Speed Increased");
                break;
            case 9:
                PlayerHealth.IncreaseMaxHealth();
                Debug.Log($"Player Level Up! Player Level: {level} - Max Health Increased");
                break;
            case 10:
                PlayerJump.IncreaseMaxAirJumps();
                Debug.Log($"Player Level Up! Player Level: {level} - Max Jumps Increased");
                break;
            default:
                break;
        }
    }
}
