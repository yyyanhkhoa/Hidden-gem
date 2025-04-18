using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    public GameObject cellPrefab;      // Prefab ô (Image/Button)
    public int rows = 4;
    public int cols = 4;

    public float spacing = 5f;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject gird = Instantiate(cellPrefab) ;
                gird.name = $"element {row} {col}";
                gird.transform.position = new Vector3(row, col,0);
            }
        }

       
    }
}
