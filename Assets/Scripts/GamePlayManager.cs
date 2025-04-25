using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI.Table;
using System.Threading.Tasks;
using System;

public class GamePlayManager : MonoBehaviour
{
    int row;
    int col;
    int mana;
    int[] ListItems = new int[9];
    public GridManager board1;
    public GridManager board2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        await getData();
        await setBoard2();
    }



    public async Task getData()
    {
        row = PlayerPrefs.GetInt("Rows");
        Debug.Log("Row: " + row);
        col = PlayerPrefs.GetInt("Cols");
        Debug.Log("Col: " + col);
        mana = PlayerPrefs.GetInt("Mana");
        Debug.Log("Mana: " + mana);
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
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
