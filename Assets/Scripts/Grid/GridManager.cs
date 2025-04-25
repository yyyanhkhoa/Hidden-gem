using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GridManager : MonoBehaviour
{
    [Header("Prefabs & Settings")]
    public GameObject cellPrefab;                       // Prefab chứa UI/Image + GridCell script
    public List<GameObject> itemPrefabs;                // Prefab các item có thể đặt lên grid

    public int rows = 4;
    public int cols = 4;

    private GridCell[,] grid;

    void Start()
    {
        _ = InitGridAsync();
    }

    private async Task InitGridAsync()
    {
      //  await CreateGrid(rows, cols);
        //GameObject prefab = itemPrefabs[8];
        //PlaceSpecificItemRandomly(prefab);
    }

    public async Task CreateGrid(int rows, int cols, float space = 5f, bool isButton = false)
    {
        await ClearGrid();
        this.rows = rows;
        this.cols = cols;   
        RectTransform parentRect = GetComponent<RectTransform>();
        float panelWidth = parentRect.rect.width;
        float panelHeight = parentRect.rect.height;

        //Tính toán kích thước và vị trí cho cell:
        float totalSpacingX = space * (cols - 1);
        float totalSpacingY = space * (rows - 1);
        //Tổng khoảng cách giữa các ô theo chiều ngang và dọc.
        float cellWidth = (panelWidth - totalSpacingX) / cols;
        float cellHeight = (panelHeight - totalSpacingY) / rows;
        //Tính chiều rộng và cao lý tưởng của mỗi ô sao cho vừa khít trong panel.
        float cellSizeMin = Mathf.Min(cellWidth, cellHeight);
        Vector2 cellSize = new Vector2(cellSizeMin, cellSizeMin);
        //Tính tổng kích thước thực tế sau khi chia lưới
        float totalWidth = cols * cellSize.x + totalSpacingX;
        float totalHeight = rows * cellSize.y + totalSpacingY;

        grid = new GridCell[rows, cols];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject cellGO = Instantiate(cellPrefab, transform);
                cellGO.name = $"Cell {row} {col}";

                RectTransform rect = cellGO.GetComponent<RectTransform>();
                rect.localScale = Vector3.one;
                rect.sizeDelta = cellSize;

                float xPos = -totalWidth / 2 + col * (cellSize.x + space) + cellSize.x / 2;
                float yPos = totalHeight / 2 - row * (cellSize.y + space) - cellSize.y / 2;

                rect.anchoredPosition = new Vector2(xPos, yPos);

                GridCell cell = cellGO.GetComponent<GridCell>();
                cell.isOccupied = false;
                if (isButton)
                {
                    cell.isButton = true;
                }
                grid[row, col] = cell;
            }
        }
    }

    public void setColor(Color cl)
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                grid[row, col].setColor(cl) ;
            }
        }
    }

    public async Task ClearGrid(GameObject girdObject = null)
    {
        if (girdObject != null)
        {
            Destroy(girdObject);          
        }
        else
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        await Task.Yield(); // Đợi 1 frame nếu cần thiết
    }

    public void PlaceSpecificItemRandomly(GameObject prefabToPlace)
    {
        ItemData data = prefabToPlace.GetComponent<ItemData>();
        bool placed = false;

        List<Vector2Int> possiblePositions = new List<Vector2Int>();

        // Tạo danh sách tất cả các vị trí hợp lệ theo chiều ngang
        for (int x = 0; x <= rows - data.sizeX; x++)
        {
            for (int y = 0; y <= cols - data.sizeY; y++)
            {
                possiblePositions.Add(new Vector2Int(x, y));
            }
        }

        // Xáo trộn danh sách
        ShuffleList(possiblePositions);

        // Thử đặt item theo danh sách ngẫu nhiên
        foreach (Vector2Int pos in possiblePositions)
        {
            if (CanPlaceItem(pos.x, pos.y, data.sizeX, data.sizeY))
            {
                PlaceItem(prefabToPlace, pos.x, pos.y, data.sizeX, data.sizeY);
                placed = true;
                return;
            }
        }

        // Nếu không đặt được theo chiều ngang, thử xoay dọc
        possiblePositions.Clear();
        for (int x = 0; x <= rows - data.sizeY; x++)
        {
            for (int y = 0; y <= cols - data.sizeX; y++)
            {
                possiblePositions.Add(new Vector2Int(x, y));
            }
        }

        ShuffleList(possiblePositions);

        foreach (Vector2Int pos in possiblePositions)
        {
            if (CanPlaceItem(pos.x, pos.y, data.sizeY, data.sizeX))
            {
                PlaceItem(prefabToPlace, pos.x, pos.y, data.sizeY, data.sizeX);
                placed = true;
                return;
            }
        }

        if (!placed)
        {
            Debug.LogWarning("Không thể đặt item vào lưới: " + prefabToPlace.name);
        }
    }
    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }


    public bool CanPlaceItem(int startX, int startY, int sizeX, int sizeY)
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (grid[startX + x, startY + y].isOccupied)
                    return false;
            }
        }
        return true;
    }

    public void PlaceItem(GameObject prefab, int startX, int startY, int sizeX, int sizeY, float space = 5f)
    {
        // Lấy cell đầu tiên (top-left)
        GridCell originCell = grid[startX, startY];
        RectTransform originRect = originCell.GetComponent<RectTransform>();
        Vector2 cellSize = originRect.sizeDelta;

        // Tính tổng kích thước item trên lưới
        float itemWidth = sizeY * cellSize.x + (sizeY - 2) * space;
        float itemHeight = sizeX * cellSize.y + (sizeX - 2) * space;
        Vector2 totalSize = new Vector2(itemWidth, itemHeight);

        // Tính vị trí trung tâm của item (trung tâm khối các ô mà nó chiếm)
        Vector2 centerPos = originRect.anchoredPosition;

        // Dịch sang phải và xuống để đến giữa vùng chiếm
        centerPos += new Vector2(
            ((sizeY - 1) * (cellSize.x + space)) / 2f,
            -((sizeX - 1) * (cellSize.y + space)) / 2f
        );

        // Tạo item
        GameObject itemGO = Instantiate(prefab, this.transform);
        RectTransform itemRect = itemGO.GetComponent<RectTransform>();
        itemRect.localScale = Vector3.one;
        itemRect.sizeDelta = totalSize;
        itemRect.anchoredPosition = centerPos;

        // Đánh dấu các ô đã bị chiếm
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                grid[startX + x, startY + y].isOccupied = true;
            }
        }
    }


}
