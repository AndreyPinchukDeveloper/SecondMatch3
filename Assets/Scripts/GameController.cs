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
    public AudioSource audio; 
    Button[,] buttons;
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
        buttons[x, y].GetComponent<Image>().sprite = images[ball].sprite;
    }

    public void PlayCut()
    {
        audio.Play();
    }

    public void Click()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        int number = GetNumber(name);
        int x = number % ManageTreasure.Size;
        int y = number / ManageTreasure.Size;
        Debug.Log($"Clicked {name} {x}{y}");
        manageTreasure.Click(x, y);
    }

    private void InitializeButtons()
    {
        buttons = new Button[ManageTreasure.Size, ManageTreasure.Size];
        for (int i = 0; i < ManageTreasure.Size * ManageTreasure.Size; i++)
        {
            buttons[i % ManageTreasure.Size, i / ManageTreasure.Size] = 
                GameObject.Find($"Button ({i})").GetComponent<Button>();
        }
    }

    private void InitializeImages()
    {
        images = new Image[ManageTreasure.Balls];
        for (int i = 0; i < ManageTreasure.Balls; i++)
        {
            images[i] = GameObject.Find($"Image ({i})").GetComponent<Image>();
        }
    }

    private int GetNumber(string name)
    {
        Regex regex = new Regex("\\((\\d+)\\)");
        Match match = regex.Match(name);
        if (!match.Success)
        {
            throw new Exception("Unrecognized object name");
        }
        Group group = match.Groups[1];
        string number = group.Value;
        return Convert.ToInt32(number);
    }
}
