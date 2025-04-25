using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public class GridCell : MonoBehaviour
{
    public bool isOccupied = false;
    public bool isButton = false;
    // Gắn UI/Image nếu cần tham chiếu
    public Image backgroundImage;
    public Color color;
    public GameObject BreabreakEffect;
    // Định nghĩa một sự kiện khi có click
    public static event Action OnCellDestroyed; // Sự kiện sẽ được gọi khi ô bị hủy
    private void Start()
    {   
        Button btn = GetComponent<Button>();
        if (isButton)
        {
            if (btn != null)
            {
                // Thêm sự kiện click
                btn.onClick.AddListener(OnClickDestroy);
            }
        }
        else
        {
            btn.interactable = false;
        }
      
    }

    public void setColor(Color cl)
    {
        Image img = GetComponent<Image>();

        if (img != null)
        {
           img.color = cl;
        }
    }

    public void OnClickDestroy()
    {
        // Phát sự kiện
        OnCellDestroyed?.Invoke(); // Gọi sự kiện

        Destroy(gameObject, 0.5f);
    }
}
