﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PokdengManagerBOT : MonoBehaviour
{
    public static PokdengManagerBOT current;

    [Header("Card Proproties")]
    public bool isGameStatus;
    public int drawCard; //  draw = 1 เท่ากับ 1 ใบ
    public List<Sprite> card = new List<Sprite>();
    public List<int> scoreCard = new List<int>();
    public List<int> firstCard = new List<int>();
    public List<int> secondCard = new List<int>();
    public List<int> thirdCard = new List<int>();

    System.Random rnd = new System.Random();

    List<int> deck = new List<int>();

    [Header("Host Properties")]
    public HostCard host;

    [Header("Player Properties")]
    public List<Player> player = new List<Player>();

    int runloop = 0;


    void Start()
    {
        current = this;

        isGameStatus = true;
        drawCard = 0;

        //shuffle deck
        deck = Enumerable.Range(0, 52).OrderBy(c => rnd.Next()).ToList();

        for (int i = 0; i < 10; i++)
            firstCard.Add(deck.ElementAt(i));

        for (int i = 10; i < 20; i++)
            secondCard.Add(deck.ElementAt(i));

        for (int i = 20; i < 30; i++)
            thirdCard.Add(deck.ElementAt(i));


    }

    void Update()
    {
        GameStatus(isGameStatus);
    }

    public void GameStatus(bool gameStatus)
    {
        if (gameStatus != true)
            return;

        RandomCard(runloop);

    }

    public void RandomCard(int r1)
    {   
        if (drawCard == 1)
        {    
            //Player Data
            for(int i=r1; i<9; i=r1)
            {
                player.ElementAt(i).cardPlayer1.SetActive(true); 
                player.ElementAt(i).cardPlayer1.GetComponent<Image>().sprite = card[firstCard[i]];
            }
            
            //Host Data
            host.bgch1.SetActive(true);
            host.ch1.SetActive(true);
            host.ch1.GetComponent<Image>().sprite = card[firstCard[9]];
            
            //next draw card
            drawCard = 2;
        }

        else if(drawCard == 2)
        {
            //Player Data
            for (int i = 0; i < 9; i++)
            {
                player.ElementAt(i).cardPlayer2.SetActive(true);
                player.ElementAt(i).cardPlayer2.GetComponent<Image>().sprite = card[secondCard[i]];

                player.ElementAt(i).totalScore = scoreCard[firstCard[i]] + scoreCard[secondCard[i]];
            }

            //Host Card
            host.bgch2.SetActive(true);
            host.ch2.SetActive(true);
            host.ch2.GetComponent<Image>().sprite = card[secondCard[9]];
            host.totalScore = scoreCard[firstCard[9]] + scoreCard[secondCard[9]];

            //Next draw card
            drawCard = 3;
        
        }

        else if(drawCard == 3)
        {     
            //player data
            for (int i = 0; i < 9; i++)
            {
                if(player.ElementAt(i).totalScore < 4)
                {
                    player.ElementAt(i).cardPlayer3.SetActive(true);
                    player.ElementAt(i).cardPlayer3.GetComponent<Image>().sprite = card[thirdCard[i]];

                    player.ElementAt(i).totalScore = scoreCard[firstCard[i]] 
                        + scoreCard[secondCard[i]] 
                        + scoreCard[thirdCard[i]];
                }

            }

            //host data
            if (host.totalScore < 4)
            {
                host.bgch3.SetActive(true);
                host.ch3.SetActive(true);
                host.ch3.GetComponent<Image>().sprite = card[thirdCard[9]];
                host.totalScore = host.totalScore + scoreCard[thirdCard[9]];
            }

            drawCard = 0;
        }


    }




}