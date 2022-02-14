using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

public delegate void ShowBox(int x, int y, int ball);
public delegate void PlayCut();

public class ManageTreasure
{
    public const int Size = 9;
    public const int Balls =7;
    const int AddBall = 3;
    Random random = new Random();

    ShowBox showBox;
    PlayCut playCut;

    int fromX, fromY;
    bool isBoolSelected;

    int[,] map;

    public ManageTreasure(ShowBox showBox, PlayCut playCut)
    {
        this.showBox = showBox;
        this.playCut = playCut;
        map = new int[Size, Size];
    }

    public void Start()
    {
        ClearMap();
        AddRandomBalls();
        isBoolSelected = false;
    }

    private void AddRandomBalls()
    {
        for (int i = 0; i < AddBall; i++)
        {
            AddRandomBall();
        }
    }

    private void AddRandomBall()
    {
        int x, y;
        int loop = Size * Size;
        do
        {
            x = random.Next(Size);
            y = random.Next(Size);
            if (--loop<=0)
            {
                return;
            }
        } 
        while (map[x,y]>0);

        int ball = 1 + random.Next(Balls - 1);
        SetMap(x, y, ball);
    }

    private void ClearMap()
    {
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                SetMap(x, y, 0);
            }
        }
    }

    private void SetMap(int x, int y, int ball)
    {
        map[x, y] = ball;
        showBox(x, y, ball);
    }

    public void Click(int x, int y)
    {
        if (IsGameOver())
        {
            Start();
        }
        else
        {
            if (map[x, y] > 0)
            {
                TakeBall(x, y);
            }
            else
            {
                MoveBall(x, y);
            }
        }     
    }

    private void MoveBall(int x, int y)
    {
        if (!isBoolSelected)
        {
            return;
        }
        if (!CanMove(x,y))
        {
            return;
        }
        SetMap(x, y, map[fromX, fromY]);
        SetMap(fromX, fromY, 0);
        isBoolSelected = false;
        if (!CutLines())
        {
            AddRandomBalls();
            CutLines();
        }
    }

    private bool[,] chosenLines;

    private bool CutLines()
    {
        int balls = 0;
        chosenLines = new bool[Size, Size];

        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                balls += CutLine(x, y, 1, 0);
                balls += CutLine(x, y, 1, 1);
                balls += CutLine(x, y, -1, 1);
                balls += CutLine(x, y, 0, 1);
            }
        }
        if (balls>0)
        {
            playCut();
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (chosenLines[x,y])
                    {
                        SetMap(x, y, 0);
                    }
                }
            }
            return true;
        }
        return false;
    }

    private int CutLine(int North, int West, int East, int South)
    {
        int ball = map[North, West];
        if (ball==0)
        {
            return 0;
        }
        int count = 0;
        for (int x = North,y=West; GetMap(x,y)==ball; x+=East, y+=South)
        {
            count++;
        }
        if (count<5)
        {
            return 0;
        }
        for (int x = North, y = West; GetMap(x, y) == ball; x += East, y += South)
        {
            chosenLines[x, y] = true;
        }
        return count;
    }

    private int GetMap(int x, int y)
    {
        if (!OnMap(x,y))
        {
            return 0;
        }
        return map[x, y];
    }

    private bool OnMap(int x, int y)
    {
        return x >= 0 && x < Size && 
               y >= 0 && y < Size;
    }

    private bool[,] used;

    private bool CanMove(int toX, int toY)
    {
        used = new bool[Size, Size];
        Walk(fromX, fromY, true);
        return used[toX, toY];

    }

    private void Walk(int x, int y, bool start = false)//cheking a clear way to point
    {
        if (!start)
        {
            if (!OnMap(x, y))//cheking limit of map
            {
                return;
            }
            if (map[x, y] > 0)//cheking obstacles(balls)
            {
                return;
            }
            if (used[x,y])//cheking if we are there
            {
                return;
            }
        }
        used[x, y] = true;
        Walk(x + 1, y);
        Walk(x - 1, y);
        Walk(x, y + 1);
        Walk(x, y - 1);
    }

    private void TakeBall(int x, int y)
    {
        fromX = x;
        fromY = y;
        isBoolSelected = true;
    }

    private bool IsGameOver()
    {
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                if (map[x,y]==0)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
