using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class MainContainer : MonoBehaviour
{
    public TMP_InputField RowInput;
    public TMP_InputField ColInput;
    public Button button;

    Board board; // Uncomment this line if you have a Board script to reference
    //public Board board;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.onClick.AddListener(createGrid);
    }

    public void createGrid()
    {       
        string row = RowInput.text;
        if (int.Parse(row) > 20)
        {
            RowInput.text = "20";
        }
        if (int.Parse(row) < 4)
        {
            RowInput.text = "4";
        }

        string col = ColInput.text;
        if (int.Parse(col) > 20)
        {
            ColInput.text = "20";
        }
        if (int.Parse(col) < 4)
        {
            ColInput.text = "4";
        }
        Debug.Log(RowInput.text + " " + ColInput.text);
        board.CreateGrid(int.Parse(RowInput.text), int.Parse(ColInput.text));
    }

    void OnInputValueChanged(string text)
    {
        Debug.Log("Người dùng đang gõ: " + text);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
