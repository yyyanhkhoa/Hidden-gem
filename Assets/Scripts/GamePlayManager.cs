using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI.Table;
using System.Threading.Tasks;
using System;
using UnityEngine.UI;

public class GamePlayManager : MonoBehaviour
{
    int row;
    int col;
    int mana;
    int[] ListItems = new int[9];
    public TMPro.TMP_Text manaLeft;
    public Button exitButton;
    public GridManager board1;
    public GridManager board2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        exitButton.onClick.AddListener(exit);
        GridCell.OnCellDestroyed += OnCellDestroyed; // Đăng ký sự kiện khi ô bị hủy

        await getData();
        setBoard1();
        setBoard2();
    }

    private void OnCellDestroyed()
    {
        ReduceMana(1);
    }
    public async Task getData()
    {
        row = PlayerPrefs.GetInt("Rows");
        Debug.Log("Row: " + row);
        col = PlayerPrefs.GetInt("Cols");
        Debug.Log("Col: " + col);        
        mana = PlayerPrefs.GetInt("Mana");
        manaLeft.text = mana.ToString();
        Debug.Log("Mana: " + mana);
       
    }
    private async Task setBoard1()
    {
        await board1.CreateGrid(row, col);

        for (int i = 0; i < 9; i++)
        {
            ListItems[i] = PlayerPrefs.GetInt("Item" + i);
            Debug.Log("Item " + i + ": " + ListItems[i]);
            if (ListItems[i] > 0)
            {
                GameObject itemPrefab = board1.itemPrefabs[i];
                for (int j = 0; j < ListItems[i]; j++)
                    board1.PlaceSpecificItemRandomly(itemPrefab);
            }
        }
    }
    private async Task setBoard2()
    {
        await board2.CreateGrid(row, col, 0f, true);
        Color myColor;
        if (ColorUtility.TryParseHtmlString("#d59b63", out myColor))
        {
            board2.setColor(myColor);
        }
    }

    public void ReduceMana(int amount)
    {
        mana -= amount;
        if (mana < 0) mana = 0;
        manaLeft.text = mana.ToString();
        
    }
    public void exit()
    {

        SceneManager.LoadScene("Stage editor");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
