using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GridCell : MonoBehaviour
{
    public bool isOccupied = false;
    public bool canDestroy = false;
    // Gắn UI/Image nếu cần tham chiếu
    public Image backgroundImage;
    public Color color;
    public GameObject BreabreakEffect;

    private void Start()
    {
        if (canDestroy)
        {
            Button btn = GetComponent<Button>();

            if (btn != null)
            {
                // Thêm sự kiện click
                btn.onClick.AddListener(OnClickDestroy);
            }
        }
    }

    private void setColor(Color cl)
    {
        backgroundImage = this.GetComponent<Image>();
        if (backgroundImage != null)
        {
            backgroundImage.color = color;
        }
    }
    void OnClickDestroy()
    {
        if (BreabreakEffect != null)
        {
            Debug.Log("Destroy cell");
            // Hiệu ứng tại vị trí cell
            GameObject effect = Instantiate(BreabreakEffect, transform.position, Quaternion.identity);
            effect.transform.SetParent(transform.parent); // tránh bị xóa cùng cell
            Destroy(effect, 1f); // huỷ effect sau 2s
        }
        Destroy(gameObject, 1f);
    }

}
