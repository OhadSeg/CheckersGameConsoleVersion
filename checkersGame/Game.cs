using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using checkersLogic;

namespace checkersGame
{
    public class Game
    {
        // should be with prefix r_
        private readonly GameUI m_GameUI;
        private GameLogic m_GameLogic;
        private bool m_IsGameOn;
        private Player m_RoundWinner;

        public Game()
        {
            m_GameUI = new GameUI();
            m_GameLogic = null;
            m_IsGameOn = true;
            m_RoundWinner = null;
        }

        public void  StartGame()
        {
            initGame();

            while (m_IsGameOn)
            {
                initRound();

                gameFlow();

                m_GameLogic.CalculatePlayersScore(m_RoundWinner);

                m_GameUI.PrintGameResultMessege(m_RoundWinner);
                m_GameUI.PrintScoreMessege(m_GameLogic.TurnToPlay, m_GameLogic.NextToPlay);

                m_IsGameOn = m_GameUI.IfKeepPlaying();
                m_RoundWinner = null;
            }
        }

        private void initGame()
        {
            string player1Name = m_GameUI.GetPlayerName();
            string player2Name = null;
            int boardSize = m_GameUI.GetBoardSize();
            Player.ePlayerType typeOfPlayer2 = m_GameUI.GetGameMode();
            if (typeOfPlayer2 == Player.ePlayerType.Human)
            {
                player2Name = m_GameUI.GetPlayerName();
            }
            else
            {
                player2Name = "computer";
            }

            Player player1 = new Player(Player.ePlayerType.Human, checkersLogic.eEntity.Player1, player1Name, 0);
            Player player2 = new Player(typeOfPlayer2, checkersLogic.eEntity.Player2, player2Name, 0);

            m_GameLogic = new GameLogic(boardSize, player1, player2);
        }

        private void initRound()
        {
            int boardSize = m_GameLogic.GetSizeOfBoard();

            int playerAmountOfPieces = m_GameUI.GetPlayerAmountOfPieces(boardSize);

            if (m_GameLogic.TurnToPlay.EntityPlayer != eEntity.Player1)
            {
                m_GameLogic.SwitchPlayerTurn();
            }

            m_GameLogic.TurnToPlay.CurrAmountOfPieces = playerAmountOfPieces;
            m_GameLogic.NextToPlay.CurrAmountOfPieces = playerAmountOfPieces;

            m_GameLogic.SetBoard();

            m_RoundWinner = null;

            m_GameUI.PressedQ = false;
        }

        private void gameFlow()
        {
            bool ifMoveMade = false;

            m_GameUI.PrintBoardDisplay(m_GameLogic.GetBoard());

            while (m_RoundWinner == null)
            {
                while (!ifMoveMade)
                {
                    m_GameUI.TurnMessege(m_GameLogic.TurnToPlay);

                    if (m_GameLogic.TurnToPlay.Type == Player.ePlayerType.Computer)
                    {
                        System.Threading.Thread.Sleep(3000);
                        m_GameLogic.ApplayMoveOfComputer();
                        ifMoveMade = true;
                    }
                    else
                    {
                        Move move = m_GameUI.GetMoveFromUser(m_GameLogic.GetSizeOfBoard(), m_GameLogic.TurnToPlay);

                        if (m_GameUI.PressedQ)
                        {
                            m_RoundWinner = m_GameLogic.NextToPlay;

                            break;
                        }

                        ifMoveMade = m_GameLogic.ApplayMoveOfHuman(move);

                        if (!ifMoveMade)
                        {
                            m_GameUI.PrintInvalidMoveMessage(m_GameLogic.TurnToPlay);
                        }
                    }
                }

                if (!m_GameUI.PressedQ)
                {
                    m_RoundWinner = m_GameLogic.CheckIfThereIsAWinner();
                }

                if (m_GameLogic.IfDoubleJumpPossible == false)
                {
                    m_GameLogic.SwitchPlayerTurn();
                }

                ifMoveMade = false;

                m_GameUI.PrintBoardDisplay(m_GameLogic.GetBoard());
            }
        }
    }
}