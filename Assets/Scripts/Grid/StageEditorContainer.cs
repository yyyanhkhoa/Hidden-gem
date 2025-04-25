using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class StageEditorContainer : MonoBehaviour
{
    public TMP_InputField RowInput;
    public TMP_InputField ColInput;
    public TMP_InputField Mana;
    public Button BoardButton;
    public Button SaveButton;
    public Button exitButton;
    public GridManager board;
    public List<TMP_InputField> ListItems;
    //public Board board;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
    {     
        BoardButton.onClick.AddListener(setGrid);
        SaveButton.onClick.AddListener(SaveData);
        exitButton.onClick.AddListener(exitGame);
        board.CreateGrid(4, 4);
    }

    void exitGame()
    {
        Application.Quit();
    }
    public void setGrid()
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

        int row1, col1;

        if (int.TryParse(RowInput.text, out row1) && int.TryParse(ColInput.text, out col1))
        {
            board.rows = row1;
            board.cols = col1;
            board.CreateGrid(row1, col1);

            PlayerPrefs.SetInt("Rows", row1);
            PlayerPrefs.SetInt("Cols", col1);           
        }
    }


    public void SaveData()
    {
        int value;

        if (int.TryParse(Mana.text, out value))
            PlayerPrefs.SetInt("Mana", value);
        else
            PlayerPrefs.SetInt("Mana", 100); 


        for (int i = 0; i < ListItems.Count; i++)
        {
            if (int.TryParse(ListItems[i].text, out value))
                PlayerPrefs.SetInt("Item" + i, value);
            else
                PlayerPrefs.SetInt("Item" + i, 0); 
        }
        LoadGamePlayScreen();
    }
    void LoadGamePlayScreen()
    {        
        SceneManager.LoadScene(1);
    }
}
