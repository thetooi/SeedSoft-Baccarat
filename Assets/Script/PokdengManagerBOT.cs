﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   
using System.Linq;      // เพิ่มการทำงานให้ list สามารถอ้างอิงเชิงลึกได้

public class PokdengManagerBOT : MonoBehaviour
{
    public static PokdengManagerBOT current;       // ใช้ current อ้างอิงไปยังไฟล์อื่น

    [Header("Card Proproties")]
    public bool isGameStatus;                      // สถานะเกม
    public int drawCard;                           // รอบการ์ดที่แจก
    public List<Sprite> card = new List<Sprite>(); // รูปการ์ดทั้งหมด
    public List<int> typeCard = new List<int>();   // type of card => Club, Diamond, Heart, Spade
    public List<string> sorf = new List<string>(); // เก็บไพ่เรียงทั้งหมดในรูปแบบ string เช่น 2 3 4, 4 5 6 
    public List<float> scoreCard = new List<float>();  // คะแนนการ์ดทั้งหมด อ้างอิงลำดับจาก list card
    public List<int> firstCard = new List<int>();  // เก็บคะแนน และรูปการ์ดที่สุ่มได้ ใบที่ 1
    public List<int> secondCard = new List<int>(); // เก็บคะแนน และรูปการ์ดที่สุ่มได้ ใบที่ 2
    public List<int> thirdCard = new List<int>();  // เก็บคะแนน และรูปการ์ดที่สุ่มได้ ใบที่ 3

    public List<Sprite> X2X3Card = new List<Sprite>(); //รูป x2, x3

    System.Random rnd = new System.Random();       // ตัวรันลำดับ random ใน list

    List<int> deck = new List<int>();              // จัด deck

    [Header("Host Properties")]
    public HostCard host;                          // อ้างอิง scirpt host

    [Header("Player Properties")]
    public List<Player> player = new List<Player>();  // อ้างอิง script player

    void Start()
    {//Start
        current = this;        // ให้ current สามารถอ้างอิงถึงสคิปนี้ได้

        isGameStatus = true;   // เมื่อเริ่มเกม ให้สถานะเกมเท่ากับ true 
        drawCard = 0;          // กำหนดให้ drawcard = 0, ยังไม่เริ่มแจกการ์ด

        //shuffle deck
        deck = Enumerable.Range(0, 52).OrderBy(c => rnd.Next()).ToList();  // สลับตำแหน่ง 0, 52 ให้ตำแหน่งไม่เรียงกัน

        for (int i = 0; i < 10; i++)
            firstCard.Add(deck.ElementAt(i));   // เก็บค่าไพ่ที่ได้ลำดับที่ 1-9 ไว้ใน ตัวแปรการ์ดใบที่ 1

        for (int i = 10; i < 20; i++)
            secondCard.Add(deck.ElementAt(i));  // เก็บค่าไพ่ที่ได้ลำดับที่ 10-19 ไว้ใน ตัวแปรการ์ดใบที่ 2

        for (int i = 20; i < 30; i++)
            thirdCard.Add(deck.ElementAt(i));   // เก็บค่าไพ่ที่ได้ลำดับที่ 10-19 ไว้ใน ตัวแปรการ์ดใบที่ 3

    }// end start

    void FixedUpdate()
    {//FixedUpdate
        GameStatus(isGameStatus); //ส่ง isGameStatus เข้าไปเช็คในฟังก์ชัน

    }//end fixedUpdate

    public void GameStatus(bool gameStatus)
    {//GameStatus
        if (gameStatus != true)  //ถ้าค่าที่รับเข้ามาเป็น false ให้ออก , เกมจบแล้วให้ออก
            return;

        StartCoroutine(RandomCard()); //เริ่มเกม
    }//end GameStatus

    IEnumerator RandomCard()
    {//Randomcard
        if (drawCard == 1) // card = 1
        {//if
            //Player Data
            for (int i = 0; i < 9; i++)
            {//draw card of 0-8
                player.ElementAt(i).cardPlayer[0].SetActive(true);  //active image card
                player.ElementAt(i).cardPlayer[0].GetComponent<Image>().sprite = card[firstCard[i]];  //active image card
                player.ElementAt(i).bgcardPlayer[0].SetActive(true);  //active bg card
                player.ElementAt(i).totalScore = (int)scoreCard[firstCard[i]];

                player.ElementAt(i).typeCard[0] = typeCard[firstCard[i]]; //set number of type card1
                player.ElementAt(i).scoreCard[0] = scoreCard[firstCard[i]]; //set number of score card

                yield return new WaitForSeconds(0.2f); //delay draw card
                
            }//end draw card of 0-8

            //Host Data
            host.bgcardHost[0].SetActive(true); //active bg card
            host.cardHost[0].SetActive(true);   //active card
            host.cardHost[0].GetComponent<Image>().sprite = card[firstCard[9]]; //active image card
            host.typeCard[0] = typeCard[firstCard[9]]; //set number of type card1
            host.scoreCard[0] = scoreCard[firstCard[9]]; //set number of score card

            //next draw card
            drawCard = 2;
        }//end if

        else if (drawCard == 2) //card = 2
        {//if
            //Player Data
            for (int i = 0; i < 9; i++)
            {//loop
                player.ElementAt(i).cardPlayer[1].SetActive(true);
                player.ElementAt(i).cardPlayer[1].GetComponent<Image>().sprite = card[secondCard[i]];
                player.ElementAt(i).bgcardPlayer[1].SetActive(true);
                player.ElementAt(i).totalScore = (int)scoreCard[firstCard[i]] + (int)scoreCard[secondCard[i]]; // + score card 1, 2
                
                player.ElementAt(i).typeCard[1] = typeCard[secondCard[i]];
                player.ElementAt(i).scoreCard[1] = scoreCard[secondCard[i]];

                yield return new WaitForSeconds(0.2f);
            }//end loop

            //Host Card
            host.bgcardHost[1].SetActive(true);
            host.cardHost[1].SetActive(true);
            host.cardHost[1].GetComponent<Image>().sprite = card[secondCard[9]];
            host.totalScore = (int)scoreCard[firstCard[9]] + (int)scoreCard[secondCard[9]];  // + score card 1, 2
            host.typeCard[1] = typeCard[secondCard[9]]; //set number of type card2
            host.scoreCard[1] = scoreCard[secondCard[9]];

            //หลังจากแจกการ์ดถ้าผู้เล่นหรือเจ้าได้ 8 และ 9 การ์ดจะเปิดเอง
            StartCoroutine(Calculate(drawCard));
            drawCard = 0;
   
        }//end if

        else if (drawCard == 3) //card = 3
        {//if
            //player data
            for (int i = 0; i < 9; i++)
            {
                if (player.ElementAt(i).totalScore < 4) // score less than 4 auto draw card
                {//if else
                    player.ElementAt(i).cardPlayer[2].SetActive(true);
                    player.ElementAt(i).cardPlayer[2].GetComponent<Image>().sprite = card[thirdCard[i]];
                    player.ElementAt(i).bgcardPlayer[2].SetActive(true);
                    player.ElementAt(i).totalScore = (int)scoreCard[firstCard[i]]
                                                   + (int)scoreCard[secondCard[i]] // + score card 1, 2 , 3
                                                   + (int)scoreCard[thirdCard[i]];
                
                    player.ElementAt(i).typeCard[2] = typeCard[thirdCard[i]];
                    player.ElementAt(i).scoreCard[2] = scoreCard[thirdCard[i]];

                    //convert int to sting for check sorf card ("2 3 4", "4 5 6")
                    player.ElementAt(i).checkSort = "" + scoreCard[firstCard[i]].ToString() + " " + scoreCard[secondCard[i]].ToString() + 
                                                    " " + scoreCard[thirdCard[i]].ToString();

                    yield return new WaitForSeconds(0.2f);
                }//end if

                if (player.ElementAt(i).requestCard == true) //when player want to draw card
                {//if else
                    player.ElementAt(i).cardPlayer[2].SetActive(true);
                    player.ElementAt(i).cardPlayer[2].GetComponent<Image>().sprite = card[thirdCard[i]];
                    player.ElementAt(i).bgcardPlayer[2].SetActive(true);
                    player.ElementAt(i).totalScore = (int)scoreCard[firstCard[i]]
                                                   + (int)scoreCard[secondCard[i]]
                                                   + (int)scoreCard[thirdCard[i]];

                    player.ElementAt(i).typeCard[2] = typeCard[thirdCard[i]];
                    player.ElementAt(i).scoreCard[2] = scoreCard[thirdCard[i]];

                    
                    player.ElementAt(i).checkSort = "" + scoreCard[firstCard[i]].ToString() +
                                                    " " + scoreCard[secondCard[i]].ToString() +
                                                    " " + scoreCard[thirdCard[i]].ToString();   
                                                    
                    yield return new WaitForSeconds(0.2f);
                }//end if


            }

            //host data
            if (host.totalScore < 5) //score less than 4 auto draw card
            {//if else
                host.bgcardHost[2].SetActive(true);
                host.cardHost[2].SetActive(true);
                host.cardHost[2].GetComponent<Image>().sprite = card[thirdCard[9]];
                host.totalScore = host.totalScore + (int)scoreCard[thirdCard[9]];
                host.typeCard[2] = typeCard[thirdCard[9]];
                host.scoreCard[2] = scoreCard[thirdCard[9]];
                host.checkSort = "" + scoreCard[firstCard[9]].ToString() + " " + scoreCard[secondCard[9]].ToString() +  
                                 " " + scoreCard[thirdCard[9]].ToString();
            }//end if

            //การ์ดทั้งหมดจะถูกเปิดออก
            StartCoroutine(Calculate(drawCard));
            drawCard = 0; //stop draw card

        }//end if


    }//end RandomCard

    IEnumerator Calculate(int cntCard)
    {//Calculate
        yield return new WaitForSeconds(2); //Delay draw card 2 sec
        if (cntCard == 2)
        {//if cnt = 2
            
            //auto open card AI, when ai get pok 8,9    
            for (int i = 0; i < 9; i++)
            {//loop
                player.ElementAt(i).getStar = 1;
                if (player.ElementAt(i) != player.ElementAt(4)) //player 4 คือ ผู้เล่น
                {//if
                    
                    // เช็ค 2 เด้ง
                    if (player.ElementAt(i).typeCard[0] == player.ElementAt(i).typeCard[1] ||
                        player.ElementAt(i).scoreCard[0] == player.ElementAt(i).scoreCard[1])
                    {  //ถ้าดอกใบที่ และแต้ม ใบที่ 1 2 เหมือนกัน
                         //active x2 
                        player.ElementAt(i).X2X3.GetComponent<Image>().enabled = true;
                    }

                    if (player.ElementAt(i).totalScore == 8 || player.ElementAt(i).totalScore == 9) //pok 8,9
                    {//if
                        player.ElementAt(i).ActiveAniamtion(cntCard);  //call ActiveAnimation funtion in Player.cs
                        player.ElementAt(i).score.enabled = true;      //active score text 
                        player.ElementAt(i).bgscore.enabled = true;
                        player.ElementAt(i).X2X3.SetActive(true);
                        player.ElementAt(i).getStar = 8;                        
                    }//end if
                    
                }//end if
            }//end loop

            yield return new WaitForSeconds(2);   //Delay draw card 2 sec                                               
            if (host.totalScore == 8 || host.totalScore == 9) //auto open all player, when host get pok 8,9
            {//if else
                host.ActiveAniamtion(cntCard); //open host card
                host.score.enabled = true;  //active score text
                for (int i = 0; i < 9; i++)
                {//loop
                    player.ElementAt(i).ActiveAniamtion(cntCard);  //call ActiveAnimation funtion in Player.cs (open all of player card)
                    player.ElementAt(i).score.enabled = true;      //active score text
                }//end loop
            }//end if
   
            if (host.typeCard[0] == host.typeCard[1])
                host.X2X3.SetActive(true);
        }//end cnt = 2
        else if (cntCard == 3)
        {//if cnt = 3
            yield return new WaitForSeconds(2);
            
            //Player data
            for (int i = 0; i < 9; i++)
            {//loop
                player.ElementAt(i).ActiveAniamtion(cntCard);  //active animation
                player.ElementAt(i).score.enabled = true;      //active score text
                player.ElementAt(i).bgscore.enabled = true;
                if (player.ElementAt(i).cardPlayer[2].active)    //if active card3 equal true
                {
                    player.ElementAt(i).X2X3.SetActive(false); //X2X3 gameObject will be false (waiting status card3)
                    player.ElementAt(i).getStar = 1;
                }

                // เช็ค 3 เด้ง ไพ่ดอกเหมือนกัน
                if (player.ElementAt(i).typeCard[0] == player.ElementAt(i).typeCard[1] &&
                    player.ElementAt(i).typeCard[0] == player.ElementAt(i).typeCard[2] &&
                    player.ElementAt(i).typeCard[1] == player.ElementAt(i).typeCard[2]){ //if else
                    player.ElementAt(i).X2X3.GetComponent<Image>().enabled = true;
                    player.ElementAt(i).X2X3.SetActive(true);
                    player.ElementAt(i).X2X3.GetComponent<Image>().sprite = X2X3Card[1];
                }//end if

                // เช็ค 3 เด้ง ไพ่เซียน
                if(player.ElementAt(i).scoreCard[0] > 0 && player.ElementAt(i).scoreCard[0] <= .9 &&
                    player.ElementAt(i).scoreCard[1] > 0 && player.ElementAt(i).scoreCard[1] <= .9 &&
                    player.ElementAt(i).scoreCard[2] > 0 && player.ElementAt(i).scoreCard[2] <= .9){
                    player.ElementAt(i).X2X3.GetComponent<Image>().sprite = X2X3Card[1];
                    player.ElementAt(i).X2X3.SetActive(true);
                    player.ElementAt(i).getStar = 6;
                }

                // เช็ค 3 เด้ง (ไพ่เรียง)
                for (int j = 0; j < 66; j++)
                {//loop
                    if (i != j) //ถ้าจำนวนของ player ไม่เท่ากับจำนวน ไพ่เรียง
                    {//if
                        if (player.ElementAt(i).checkSort == sorf[j]) //ถ้าไพ่ทั้ง 3 เป็นไพ่เรียง
                        {//if
                            player.ElementAt(i).X2X3.SetActive(true);
                            player.ElementAt(i).X2X3.GetComponent<Image>().sprite = X2X3Card[1];
                            player.ElementAt(i).getStar = 4;

                            // เช็ค 5 เด้ง (ไพ่เรียง + ดอกเหมือนกัน) => สเตทฟรัช
                            if (player.ElementAt(i).typeCard[0] == player.ElementAt(i).typeCard[1] &&
                                player.ElementAt(i).typeCard[0] == player.ElementAt(i).typeCard[2] &&
                                player.ElementAt(i).typeCard[1] == player.ElementAt(i).typeCard[2]){ //if
                                player.ElementAt(i).X2X3.GetComponent<Image>().enabled = true;
                                player.ElementAt(i).X2X3.GetComponent<Image>().sprite = X2X3Card[2]; //change image x2 x3 to x5
                                player.ElementAt(i).getStar = 5;
                            }//end if
                        }//end if
                    }//end if                
                }//end loop
      
                // เช็ค 5 เด้ง (ไพ่ตอง)
                if (player.ElementAt(i).scoreCard[0] == player.ElementAt(i).scoreCard[1] &&
                    player.ElementAt(i).scoreCard[0] == player.ElementAt(i).scoreCard[2] &&
                    player.ElementAt(i).scoreCard[1] == player.ElementAt(i).scoreCard[2]){ //if else
                    player.ElementAt(i).X2X3.GetComponent<Image>().enabled = true;
                    player.ElementAt(i).X2X3.SetActive(true);
                    player.ElementAt(i).X2X3.GetComponent<Image>().sprite = X2X3Card[2]; //change image x2 x3 to x5
                }//end if


            }//end loop

            //Host data
            host.ActiveAniamtion(cntCard); //call ActiveAnimation in host.cs
            host.score.enabled = true;   //active score text 
            if (host.cardHost[2].active) //ถ้า การ์ดใบที่ 3 เปิด ให้ทำงาน if นี้
                host.X2X3.SetActive(false); //unactive x2x3 gameobject
            if ((host.typeCard[0] == host.typeCard[1] && host.typeCard[0] == host.typeCard[2] && host.typeCard[1] == host.typeCard[2]) ||
                host.scoreCard[0] > 0 && host.scoreCard[0] <= .9 && host.scoreCard[1] > 0 && host.scoreCard[1] <= .9 && host.scoreCard[2] > 0 &&
                host.scoreCard[2] <= .9)
            {//if ถ้าดอกที่ 1,2,3 เหมือนกัน และ ไพ่เซียน
                host.X2X3.SetActive(true);  //active x3
                host.X2X3.GetComponent<Image>().sprite = X2X3Card[1]; //change image x2 to x3
            }//end if

            //เช็ค 3 เด้ง (ไพ่เรียง)
            for (int j = 0; j < 66; j++)
            {//loop
                if (host.checkSort == sorf[j]) //ถ้าไพ่ทั้ง 3 เป็นไพ่เรียง
                {//if
                    host.X2X3.SetActive(true);
                    host.X2X3.GetComponent<Image>().sprite = X2X3Card[1];

                    // เช็ค 5 เด้ง (ไพ่เรียง + ดอกเหมือนกัน) => สเตทฟรัช
                    if (host.typeCard[0] == host.typeCard[1] &&
                        host.typeCard[0] == host.typeCard[2] &&
                        host.typeCard[1] == host.typeCard[2]){ //if
                        host.X2X3.SetActive(true);
                        host.X2X3.GetComponent<Image>().sprite = X2X3Card[2]; //change image x2 x3 to x5
                    }//end if
                }//end if              
            }//end loop

            //เช็ค 5 เด้ง (ไพ่ตอง)
            if (host.scoreCard[0] == host.scoreCard[1] &&
                host.scoreCard[0] == host.scoreCard[2] &&
                host.scoreCard[1] == host.scoreCard[2]) { //if else
                host.X2X3.SetActive(true);
                host.X2X3.GetComponent<Image>().sprite = X2X3Card[2]; //change image x2 x3 to x5
            }//end if
        } //end cnt = 3
    }//end Calculate

}
