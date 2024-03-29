﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace TicTacToe
{
    ///<summary>
    ///<para>moves bot do</para>
    ///</summary>
    static class Agent
    {
        //Values
        ///<summary>
        ///<para>If the bot is X or O</para>
        ///</summary>
        public static string XorO = "Null";
        ///<summary>
        ///<para>Exploration Percentage</para>
        ///</summary>
        public static int ExploreP = 20;
        //the action agent does
        ///<summary>
        ///<para>move to allow desicion to be made or default to random move</para>
        ///</summary>
        public static void AImove()
        {
            Random rnd = new Random();
            try
            {
                if ((rnd.Next(0, 100) + 1) > ExploreP)
                {
                    NodeSystem.Noding.Node stored = new NodeSystem.Noding.Node();
                    bool agentx;

                    if (XorO == "X")
                    {
                        agentx = true;
                    }
                    else if (XorO == "O")
                    {
                        agentx = false;
                    }
                    else
                    {
                        return;
                    }

                    Thread BestMV = new Thread(() =>
                    {
                        stored = NodeSystem.BestMove(agentx);
                    });
                    BestMV.Start();
                    BestMV.Join();

                    if (stored != new NodeSystem.Noding.Node())
                    {
                        Valuenter(NodeSystem.MoveDiff(NodeSystem.CurrentMoves().ToArray(), NodeSystem.ReturnMoves(stored)));
                        Link.Logs.Input("Bot : Reinforced Move");
                    }
                    else
                    {
                        RandomMove();
                    }
                }
                else
                {
                    NewMove();
                }
            }
            catch (Exception)
            {
                RandomMove();
            }

            Grid.Updategrid();

            if (Link.Botters.Checked)
            {
                SwapTurn();
            }
            Link.Logs.UpdateLog();
        }
        //
        ///<summary>
        ///<para>swaps turn when used for bot vs bot</para>
        ///</summary>
        private static void SwapTurn()
        {
            if (XorO == "X")
            {
                XorO = "O";
            }
            else if (XorO == "O")
            {
                XorO = "X";
            }
        }
        ///<summary>
        ///<para>Enters a move to the grid</para>
        ///</summary>
        private static void Valuenter(int i)
        {
            Grid.grid[i] = XorO;
            Link.Logs.Input($"Bot : {XorO} Placed On {Link.Logs.Translate(i)}\n");
        }
        ///<summary>
        ///<para>makes random moves</para>
        ///</summary>
        public static void RandomMove()
        {
            Link.Logs.Input("Bot : Random Move");
            Random random = new Random();
            int[] Amove = NodeSystem.CurrentMoves().ToArray();
            Valuenter(Amove[random.Next(Amove.Length)]);
        }
        public static void NewMove()
        {
            Link.Logs.Input("Bot : New Move");
            if (Link.Main.turntally == 9)
            {
                RandomMove();
                return;
            }
            else
            {
                Valuenter(NodeSystem.findnewmove(Link.Main.turntally,NodeSystem.Noding.ReturnCurrentNode()));
            }
        }
    }
}
