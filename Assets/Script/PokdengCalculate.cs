﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokdengCalculate : MonoBehaviour
{
    public static PokdengCalculate current;

    [Header("Score of Card")]
    public int totalScore;
    public List<int> scoreCard = new List<int>();
    public int[] score;
    private int amountCard;

    void Start()
    {
        current = this;
    }

    public void GetScore(int cntCard)
    {
        if (cntCard == 2)
        {/*
            score[0] = scoreCard[HostCard.current.numCard1];
            score[1] = scoreCard[HostCard.current.numCard2];

            totalScore = score[0] + score[1];
            if (totalScore >= 10 && totalScore <= 19)
                totalScore = totalScore - 10;
            if (totalScore >= 20 && totalScore <= 30)
                totalScore = totalScore - 20;

           // if (totalScore == 8 || totalScore == 9)
               // PokdengUIManager.current.BTPass();
            */
        }

        else if (cntCard == 3)
        {/*
            score[0] = scoreCard[HostCard.current.numCard1];
            score[1] = scoreCard[HostCard.current.numCard2];
            score[2] = scoreCard[HostCard.current.numCard3];

            totalScore = score[0] + score[1] + score[2];
            if (totalScore >= 10 && totalScore <= 19)
                totalScore = totalScore - 10;
            if (totalScore >= 20 && totalScore <= 30)
                totalScore = totalScore - 20;

           */
        }

    }
}
