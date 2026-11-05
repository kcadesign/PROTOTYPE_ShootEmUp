using UnityEngine;

public class Node : MonoBehaviour
{
    private BossController_Existence _bossController;

    private void Awake()
    {
        // Find the BossController_Existence in one of the parent above this object
        _bossController = GetComponentInParent<BossController_Existence>();
    }

    private void OnDisable()
    {
        // When this node is disabled, check if all nodes are inactive
        if (_bossController != null)
        {
            _bossController.CheckActiveNodes();
        }
    }
}
