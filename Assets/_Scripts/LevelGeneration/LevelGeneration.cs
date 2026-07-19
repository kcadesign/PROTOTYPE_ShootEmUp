using UnityEngine;
using System.Collections;

public class LevelGeneration : MonoBehaviour
{
    public int levelLength = 3;
    public GameObject[] StartBlocks;
    public GameObject[] levelBlocks;
    public GameObject[] EndBlocks;

    private int _placedBlockIndex = 0;
    private int _newBlockIndex;

    private float _yPosition = 0;
    public float YPositionIncrement = 40f;

    private void Awake()
    {
        if (levelBlocks.Length <= 1)
        {
            Debug.LogWarning("Level Generation: Not enough level block variations to generate a level.");
        }
    }

    private void Start()
    {
        StartCoroutine(GenerateLevel());
    }

    private IEnumerator GenerateLevel()
    {
        ChooseAndPlaceStartBlock();

        yield return null;

        yield return StartCoroutine(ChooseAndPlaceMidBlocks());

        ChooseAndPlaceEndBlock();
    }

    private void ChooseAndPlaceStartBlock()
    {
        Instantiate(StartBlocks[Random.Range(0, StartBlocks.Length)], new Vector3(0, _yPosition, 0), Quaternion.identity, transform);
    }

    private IEnumerator ChooseAndPlaceMidBlocks()
    {
        for (int i = 0; i < levelLength; i++)
        {
            _newBlockIndex = Random.Range(0, levelBlocks.Length);

            if (_newBlockIndex == _placedBlockIndex)
            {
                _newBlockIndex = (_newBlockIndex + 1) % levelBlocks.Length;
            }

            GameObject block = levelBlocks[_newBlockIndex];

            IncementYPosition(YPositionIncrement);

            Instantiate(block, new Vector3(0, _yPosition, 0), Quaternion.identity, transform);

            _placedBlockIndex = _newBlockIndex;

            // Wait one frame before creating the next block
            yield return null;
        }
    }

    private void IncementYPosition(float amount)
    {
        _yPosition += amount;
    }

    private void ChooseAndPlaceEndBlock()
    {
        IncementYPosition(YPositionIncrement);
        Instantiate(EndBlocks[Random.Range(0, EndBlocks.Length)], new Vector3(0, _yPosition, 0), Quaternion.identity, transform);
    }
}
