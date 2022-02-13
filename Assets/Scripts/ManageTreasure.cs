using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ShowBox(int x, int y, int ball);
public delegate void PlayCut();

public class ManageTreasure
{
    public const int Size = 9;
    public const int Balls = 7;

    ShowBox showBox;
    PlayCut playCut;

    public ManageTreasure(ShowBox showBox, PlayCut playCut)
    {
        this.showBox = showBox;
        this.playCut = playCut;
    }

    public void Start()
    {
        playCut();
    }

    public void Click(int x, int y)
    {
        showBox(x, y, 1);
    }
}
