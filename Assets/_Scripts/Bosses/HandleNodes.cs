using UnityEngine;

public class HandleNodes : MonoBehaviour
{
    public GameObject[] Nodes;
    public GameObject Shell;

    private void Awake()
    {
        if (Nodes.Length == 0)
        {
            Debug.LogWarning("No nodes added to array");
        }
        Debug.Log($"Nodes array length: {Nodes.Length}");
    }

    private void Update()
    {
        CheckActiveNodes();
    }

    private void CheckActiveNodes()
    {
        // Assume all nodes are inactive
        bool allNodesInactive = true;

        foreach (GameObject node in Nodes)
        {
            if (node.activeSelf)
            {
                // Found an active node, so not all are inactive
                allNodesInactive = false;
                break;
            }
        }

        if (allNodesInactive)
        {
            Shell.SetActive(false);
        }
    }
}
