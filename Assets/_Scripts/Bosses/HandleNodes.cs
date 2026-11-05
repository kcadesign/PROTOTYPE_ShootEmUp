using Unity.VisualScripting;
using UnityEngine;

public class HandleNodes : MonoBehaviour
{
    //public BossController_Existence BossController;

    //public GameObject[] Phase1Nodes;
    //public GameObject[] Phase2Nodes;
    //public GameObject[] Phase3Nodes;
    //private GameObject[] _currentNodes;

    //public GameObject Shell;

    ////private void Start()
    ////{
    ////    CheckBossPhase();
    ////}

    //public void CheckActiveNodes()
    //{
    //    Debug.Log("Checking active nodes");
    //    // Assume all nodes are inactive
    //    bool allNodesInactive = true;

    //    foreach (GameObject node in _currentNodes)
    //    {
    //        if (node.activeSelf)
    //        {
    //            Debug.Log($"Node is active: {node.name}");
    //            // Found an active node, so not all are inactive
    //            allNodesInactive = false;
    //            break;
    //        }
    //    }

    //    if (allNodesInactive)
    //    {
    //        Debug.Log("All nodes are inactive. Deactivating shell and parent object.");
    //        Shell.SetActive(false);
    //        _currentNodes[0].transform.parent.gameObject.SetActive(false);
    //    }

    //    CheckBossPhase();
    //    Debug.Log($"Current phase: {BossController.GetBossPhase()} | Active nodes: {_currentNodes[0].transform.parent.gameObject.name}");
    //}

    //private void CheckBossPhase()
    //{
    //    switch (BossController.GetBossPhase())
    //    {
    //        case BossController_Existence.BossPhase.Phase1:
    //            _currentNodes = Phase1Nodes;
    //            Phase1Nodes[0].transform.parent.gameObject.SetActive(true);
    //            Phase2Nodes[0].transform.parent.gameObject.SetActive(false);
    //            Phase3Nodes[0].transform.parent.gameObject.SetActive(false);
    //            break;
    //        case BossController_Existence.BossPhase.Phase2:
    //            _currentNodes = Phase2Nodes;
    //            Phase1Nodes[0].transform.parent.gameObject.SetActive(false);
    //            Phase2Nodes[0].transform.parent.gameObject.SetActive(true);
    //            Phase3Nodes[0].transform.parent.gameObject.SetActive(false);
    //            break;
    //        case BossController_Existence.BossPhase.Phase3:
    //            _currentNodes = Phase3Nodes;
    //            Phase1Nodes[0].transform.parent.gameObject.SetActive(false);
    //            Phase2Nodes[0].transform.parent.gameObject.SetActive(false);
    //            Phase3Nodes[0].transform.parent.gameObject.SetActive(true);
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
