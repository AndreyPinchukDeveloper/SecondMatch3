using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    ManageTreasure manageTreasure;
    Button[,] button;
    Image[] images;
    public void Start()
    {
        manageTreasure = new ManageTreasure(ShowBox, PlayCut);
        InitializeButtons();
        InitializeImages();
        manageTreasure.Start();
    }

    public void ShowBox(int x, int y, int ball)
    {

    }

    public void PlayCut()
    {

    }

    public void Click()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        int number = GetNumber();
        int x = number % ManageTreasure.Size;
        int y = number % ManageTreasure.Size;
        Debug.Log($"Clicked {name} {x}{y}");
    }

    private void InitializeButtons()
    {
        button = new Button[ManageTreasure.Size, ManageTreasure.Size];
        for (int i = 0; i < ManageTreasure.Size * ManageTreasure.Size; i++)
        {
            button[i % ManageTreasure.Size, i / ManageTreasure.Size] = 
                GameObject.Find($"Button({i})").GetComponent<Button>();
        }
    }

    private void InitializeImages()
    {
        images = new Image[ManageTreasure.Balls];
        for (int i = 0; i < ManageTreasure.Balls; i++)
        {
            images[i] = GameObject.Find($"Image ({j})").GetComponent<Image>();
        }
    }

    private int GetNumber(string name)
    {
        Regex regex = new Regex("\\((\\d+)\\)");
        ManageTreasure match = regex.Match(name);
        if (!match.Success)
        {
            throw new Exception("Unrecognized object name");
        }
        IGrouping group = match.Groups[1];
        string number = group.Value;
        return Convert.ToInt32(number);
    }
}
