using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject cellPrefab;
    public int rows = 4;
    public int cols = 4;

    public float spacing = 5f;

    void Start()
    {
        CreateGrid(rows, cols);
    }

    public void CreateGrid(int x, int y)
    {
        // Xóa tất cả các cellPrefab đã tồn tại
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        RectTransform parentRect = GetComponent<RectTransform>();

        // Bước 1: Lấy kích thước panel
        float panelWidth = parentRect.rect.width;
        float panelHeight = parentRect.rect.height;

        // Bước 2: Tính toán kích thước cell vừa khít panel (trừ spacing)
        float totalSpacingX = spacing * (cols - 1);
        float totalSpacingY = spacing * (rows - 1);

        float cellWidth = (panelWidth - totalSpacingX) / cols;
        float cellHeight = (panelHeight - totalSpacingY) / rows;

        // Chọn kích thước vuông (tuỳ bạn)
        float cellSizeMin = Mathf.Min(cellWidth, cellHeight);
        Vector2 cellSize = new Vector2(cellSizeMin, cellSizeMin);

        // Bước 3: Tính lại tổng lưới (dùng cellSize mới)
        float totalWidth = cols * cellSize.x + totalSpacingX;
        float totalHeight = rows * cellSize.y + totalSpacingY;

        // Bước 4: Tạo các cell
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject grid = Instantiate(cellPrefab, this.transform);
                grid.name = $"element {row} {col}";

                RectTransform gridRect = grid.GetComponent<RectTransform>();
                gridRect.localScale = Vector3.one;
                gridRect.sizeDelta = cellSize;

                float xPos = -totalWidth / 2 + col * (cellSize.x + spacing) + cellSize.x / 2;
                float yPos = totalHeight / 2 - row * (cellSize.y + spacing) - cellSize.y / 2;

                gridRect.anchoredPosition = new Vector2(xPos, yPos);
            }
        }
    }
}
