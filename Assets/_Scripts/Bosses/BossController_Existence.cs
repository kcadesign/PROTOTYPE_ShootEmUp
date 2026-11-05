using UnityEngine;

public class BossController_Existence : MonoBehaviour
{
    private Health _bossHealth;
    [Header("Node References")]
    public HandleNodes HandleNodes;
    public GameObject[] Phase1Nodes;
    public GameObject[] Phase2Nodes;
    public GameObject[] Phase3Nodes;
    private GameObject[] _currentNodes;

    [Header("Shell Reference")]
    public GameObject Shell;

    public enum BossPhase
    {
        Phase1,
        Phase2,
        Phase3
    }

    private BossPhase CurrentPhase = BossPhase.Phase1;
    private int _lastHealth = -1;


    private void Awake()
    {
        _bossHealth = TryGetComponent<Health>(out Health health) ? health : null;
        if (_bossHealth == null)
        {
            Debug.LogError("Health component not found on Boss_Existence.");
        }
    }

    private void Update()
    {
        if (_bossHealth != null)
        {
            DecideBossPhase();
        }
    }

    public void DecideBossPhase()
    {
        int currentHealth = _bossHealth.GetHealth();

        if (currentHealth == _lastHealth)
        {
            return;
        }

        _lastHealth = currentHealth;

        switch (currentHealth)
        {
            case 3:
                UpdateBossPhase(BossPhase.Phase1);
                break;

            case 2:
                UpdateBossPhase(BossPhase.Phase2);
                break;

            case 1:
                UpdateBossPhase(BossPhase.Phase3);
                break;
        }
    }

    private void UpdateBossPhase(BossPhase phase)
    {
        CurrentPhase = phase;
        switch (phase)
        {
            case BossPhase.Phase1:
                // Phase 1 nodes should be set active and all others inactive
                _currentNodes = Phase1Nodes;
                Phase1Nodes[0].transform.parent.gameObject.SetActive(true);
                Phase2Nodes[0].transform.parent.gameObject.SetActive(false);
                Phase3Nodes[0].transform.parent.gameObject.SetActive(false);
                SetShellActive(true);
                break;
            case BossPhase.Phase2:
                // Phase 2 nodes should be set active and all others inactive
                _currentNodes = Phase2Nodes;
                Phase1Nodes[0].transform.parent.gameObject.SetActive(false);
                Phase2Nodes[0].transform.parent.gameObject.SetActive(true);
                Phase3Nodes[0].transform.parent.gameObject.SetActive(false);
                SetShellActive(true);
                break;
            case BossPhase.Phase3:
                // Phase 3 nodes should be set active and all others inactive
                _currentNodes = Phase3Nodes;
                Phase1Nodes[0].transform.parent.gameObject.SetActive(false);
                Phase2Nodes[0].transform.parent.gameObject.SetActive(false);
                Phase3Nodes[0].transform.parent.gameObject.SetActive(true);
                SetShellActive(true);
                break;
            default:
                Debug.LogWarning("Invalid node phase");
                break;
        }
        Debug.Log($"Current Boss Phase: {CurrentPhase}");
    }


    public void CheckActiveNodes()
    {
        Debug.Log("Checking active nodes");
        // Assume all nodes are inactive
        bool allNodesInactive = true;

        foreach (GameObject node in _currentNodes)
        {
            if (node.activeSelf)
            {
                //Debug.Log($"Node is active: {node.name}");
                // Found an active node, so not all are inactive
                allNodesInactive = false;
                break;
            }
        }

        if (allNodesInactive)
        {
            Debug.Log("All nodes are inactive. Deactivating shell.");
            SetShellActive(false);
            //_currentNodes[0].transform.parent.gameObject.SetActive(false);
        }
    }

    private void SetShellActive(bool isActive)
    {
        if (Shell != null)
        {
            Shell.SetActive(isActive);
        }
        else
        {
            Debug.LogWarning("Shell GameObject is not assigned.");
        }
    }
}
