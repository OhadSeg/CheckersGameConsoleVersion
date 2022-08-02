using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace checkersLogic
{
    public class GameLogic
    {
        private const int k_NumOfcontestants = 2;
        private readonly Cell[,] m_Board;
        private readonly List<Move> m_PossibleStepsList;
        private readonly List<Move> m_PossibleJumpsList;
        private readonly List<Move> m_PossibleMultiplyJumpsList;
        private Player m_TurnToPlay;
        private Player m_NextToPlay;
        private bool m_IfDoubleJumpPossible;

        public GameLogic(int i_SizeOfBoard, Player i_Player1, Player i_Player2)
        {
            m_Board = new Cell[i_SizeOfBoard, i_SizeOfBoard];
            m_TurnToPlay = i_Player1;
            m_NextToPlay = i_Player2;
            m_PossibleStepsList = new List<Move>();
            m_PossibleJumpsList = new List<Move>();
            m_PossibleMultiplyJumpsList = new List<Move>();
            m_IfDoubleJumpPossible = false;
        }

        public Player      TurnToPlay
        {
            get { return m_TurnToPlay; }
            set { m_TurnToPlay = value; }
        }

        public Player      NextToPlay
        {
            get { return m_NextToPlay; }
            set { m_NextToPlay = value; }
        }

        public bool        IfDoubleJumpPossible
        {
            get { return m_IfDoubleJumpPossible; }
        }

        public Cell[,]     GetBoard()
        {
            Cell[,] returnBoard = new Cell[m_Board.GetLength(0), m_Board.GetLength(1)];

            for (int i = 0; i < m_Board.GetLength(0); i++)
            {
                for (int j = 0; j < m_Board.GetLength(1); j++)
                {
                    returnBoard[i, j] = m_Board[i, j];
                }
            }

            return returnBoard;
        }

        public void        SetBoard()
        {
            bool isValidSettingHelper = false;

            for (int i = 0; i < m_Board.GetLength(0); i++)
            {
                for (int j = 0; j < m_Board.GetLength(1); j++)
                {
                    m_Board[i, j] = new Cell(eEntity.Inaccessible);

                    if (!isValidSettingHelper)
                    {
                        m_Board[i, j].Entity = eEntity.Inaccessible;
                    }
                    else if (i == (m_Board.GetLength(0) / 2) - 1 || i == m_Board.GetLength(0) / 2)
                    {
                        m_Board[i, j].Entity = eEntity.Empty;
                    }
                    else if (i < (m_Board.GetLength(0) / 2) - 1)
                    {
                        m_Board[i, j].Entity = eEntity.Player2;
                    }
                    else
                    {
                        m_Board[i, j].Entity = eEntity.Player1;
                    }

                    if (j != m_Board.GetLength(0) - 1)
                    {
                        isValidSettingHelper = !isValidSettingHelper;
                    }
                }
            }
        }

        private void       deleteInBetween(Move i_Move)
        {
            int rowDistance = i_Move.DestRow - i_Move.CurrRow;
            int colDistance = i_Move.DestCol - i_Move.CurrCol;

            if (rowDistance == -2 && colDistance == 2)
            {
                m_Board[i_Move.CurrRow - 1, i_Move.CurrCol + 1].Entity = eEntity.Empty;
                m_Board[i_Move.CurrRow - 1, i_Move.CurrCol + 1].IsKing = false;
            }

            if (rowDistance == -2 && colDistance == -2)
            {
                m_Board[i_Move.CurrRow - 1, i_Move.CurrCol - 1].Entity = eEntity.Empty;
                m_Board[i_Move.CurrRow - 1, i_Move.CurrCol - 1].IsKing = false;
            }

            if (rowDistance == 2 && colDistance == 2)
            {
                m_Board[i_Move.CurrRow + 1, i_Move.CurrCol + 1].Entity = eEntity.Empty;
                m_Board[i_Move.CurrRow + 1, i_Move.CurrCol + 1].IsKing = false;
            }

            if (rowDistance == 2 && colDistance == -2)
            {
                m_Board[i_Move.CurrRow + 1, i_Move.CurrCol - 1].Entity = eEntity.Empty;
                m_Board[i_Move.CurrRow + 1, i_Move.CurrCol - 1].IsKing = false;
            }
        }


        private List<Move> buildAllPossStepsForCurrToPlay()
        {
            m_PossibleStepsList.Clear();

            for (int i = 0; i < m_Board.GetLength(0); i++)
            {
                for (int j = 0; j < m_Board.GetLength(1); j++)
                {
                    if (m_Board[i, j].Entity == m_TurnToPlay.EntityPlayer)
                    {
                        if (m_TurnToPlay.EntityPlayer == eEntity.Player1)
                        {
                            if (checkTypeOfStepsValidation(i - 1, j + 1))
                            {
                                Move stepMove = new Move(j, i, j + 1, i - 1);
                                m_PossibleStepsList.Add(stepMove);
                            }

                            if (checkTypeOfStepsValidation(i - 1, j - 1))
                            {
                                Move stepMove = new Move(j, i, j - 1, i - 1);
                                m_PossibleStepsList.Add(stepMove);
                            }

                            if (m_Board[i, j].IsKing)
                            {
                                if (checkTypeOfStepsValidation(i + 1, j - 1))
                                {
                                    Move stepMove = new Move(j, i, j - 1, i + 1);
                                    m_PossibleStepsList.Add(stepMove);
                                }

                                if (checkTypeOfStepsValidation(i + 1, j + 1))
                                {
                                    Move stepMove = new Move(j, i, j + 1, i + 1);
                                    m_PossibleStepsList.Add(stepMove);
                                }
                            }
                        }
                        else if (m_TurnToPlay.EntityPlayer == eEntity.Player2)
                        {
                            if (checkTypeOfStepsValidation(i + 1, j - 1))
                            {
                                Move stepMove = new Move(j, i, j - 1, i + 1);
                                m_PossibleStepsList.Add(stepMove);
                            }

                            if (checkTypeOfStepsValidation(i + 1, j + 1))
                            {
                                Move stepMove = new Move(j, i, j + 1, i + 1);
                                m_PossibleStepsList.Add(stepMove);
                            }

                            if (m_Board[i, j].IsKing)
                            {
                                if (checkTypeOfStepsValidation(i + 1, j - 1))
                                {
                                    Move stepMove = new Move(j, i, j - 1, i + 1);
                                    m_PossibleStepsList.Add(stepMove);
                                }

                                if (checkTypeOfStepsValidation(i + 1, j + 1))
                                {
                                    Move stepMove = new Move(j, i, j + 1, i + 1);
                                    m_PossibleStepsList.Add(stepMove);
                                }
                            }
                        }
                    }
                }
            }

            return m_PossibleStepsList;
        }

        private List<Move> buildAllPossJumpsForCurrToPlay()
        {
            m_PossibleJumpsList.Clear();

            for (int i = 0; i < m_Board.GetLength(0); i++)
            {
                for (int j = 0; j < m_Board.GetLength(1); j++)
                {
                    if (m_Board[i, j].Entity == m_TurnToPlay.EntityPlayer)
                    {
                        if (m_TurnToPlay.EntityPlayer == eEntity.Player1 || m_Board[i, j].IsKing)
                        {
                            if (checkTypeOfJumpingValidation(i - 2, j + 2, i - 1, j + 1))
                            {
                                Move eatMove = new Move(j, i, j + 2, i - 2);
                                m_PossibleJumpsList.Add(eatMove);
                            }

                            if (checkTypeOfJumpingValidation(i - 2, j - 2, i - 1, j - 1))
                            {
                                Move eatMove = new Move(j, i, j - 2, i - 2);
                                m_PossibleJumpsList.Add(eatMove);
                            }
                        }

                        if (m_TurnToPlay.EntityPlayer == eEntity.Player2 || m_Board[i, j].IsKing)
                        {
                            if (checkTypeOfJumpingValidation(i + 2, j + 2, i + 1, j + 1))
                            {
                                Move eatMove = new Move(j, i, j + 2, i + 2);
                                m_PossibleJumpsList.Add(eatMove);
                            }

                            if (checkTypeOfJumpingValidation(i + 2, j - 2, i + 1, j - 1))
                            {
                                Move eatMove = new Move(j, i, j - 2, i + 2);
                                m_PossibleJumpsList.Add(eatMove);
                            }
                        }
                    }
                }
            }

            return m_PossibleJumpsList;
        }

        private void       buildPossiblePieceJumpsList(Move i_Move)
        {

            List<Move> AllPossibleJumpsList = buildAllPossJumpsForCurrToPlay();

            foreach (Move jumpMove in AllPossibleJumpsList)
            {
                if (jumpMove.CurrRow == i_Move.DestRow && jumpMove.CurrCol == i_Move.DestCol)
                {
                    Move secondJump = new Move(jumpMove.CurrCol, jumpMove.CurrRow, jumpMove.DestCol, jumpMove.DestRow);
                    m_PossibleMultiplyJumpsList.Add(secondJump);
                    m_IfDoubleJumpPossible = true;
                }
            }
        }

        private bool       checkTypeOfJumpingValidation(int i_DestRow, int i_DestCol, int i_JumpAbovedRow, int i_JumpAbovedCol)
        {
            bool ifValidMove = false;

            if (i_DestRow >= 0 && i_DestRow < m_Board.GetLength(0) && i_DestCol >= 0 && i_DestCol < m_Board.GetLength(1) && m_Board[i_DestRow, i_DestCol].Entity == eEntity.Empty)
            {
                if (i_JumpAbovedRow >= 0 && i_JumpAbovedCol < m_Board.GetLength(1) && m_Board[i_JumpAbovedRow, i_JumpAbovedCol].Entity == m_NextToPlay.EntityPlayer)
                {
                    ifValidMove = true;
                }
            }

            return ifValidMove;
        }


        public bool        ApplayMoveOfHuman(Move i_Move)
        {
            bool isValidMove = false;

            buildAllPossJumpsForCurrToPlay();
            buildAllPossStepsForCurrToPlay();

            if (m_PossibleMultiplyJumpsList.Count() != 0)
            {
                if (m_PossibleMultiplyJumpsList.Any
                    (x => x.CurrRow == i_Move.CurrRow && x.CurrCol == i_Move.CurrCol && x.DestRow == i_Move.DestRow && x.DestCol == i_Move.DestCol))
                {
                    m_NextToPlay.CurrAmountOfPieces--;
                    isValidMove = true;
                    applayMoveOnBoard(i_Move);
                    deleteInBetween(i_Move);
                    m_PossibleMultiplyJumpsList.Clear();
                    buildPossiblePieceJumpsList(i_Move);
                    if (m_PossibleMultiplyJumpsList.Count() != 0)
                    {
                        m_IfDoubleJumpPossible = true;
                    }
                    else
                    {
                        m_IfDoubleJumpPossible = false;
                    }
                }
            }
            else if (m_PossibleJumpsList.Count() != 0)
            {
                if (m_PossibleJumpsList.Any
                        (x => x.CurrRow == i_Move.CurrRow && x.CurrCol == i_Move.CurrCol && x.DestRow == i_Move.DestRow && x.DestCol == i_Move.DestCol))
                {
                    m_NextToPlay.CurrAmountOfPieces--;
                    applayMoveOnBoard(i_Move);
                    deleteInBetween(i_Move);
                    isValidMove = true;
                    buildPossiblePieceJumpsList(i_Move);
                    if (m_PossibleMultiplyJumpsList.Count() != 0)
                    {
                        m_IfDoubleJumpPossible = true;
                    }
                }
            }
            else if (m_PossibleStepsList.Count() != 0)
            {
                if (m_PossibleStepsList.Any
                       (x => x.CurrRow == i_Move.CurrRow && x.CurrCol == i_Move.CurrCol && x.DestRow == i_Move.DestRow && x.DestCol == i_Move.DestCol))
                {
                    applayMoveOnBoard(i_Move);
                    isValidMove = true;
                }
            }
            else
            {
                m_TurnToPlay.IfOutOfMoves = true;
            }

            return isValidMove;
        }

        public void        ApplayMoveOfComputer()
        {
            Move move = null;
            int drawnNumber;



            Random rnd = new Random();

            buildAllPossJumpsForCurrToPlay();
            buildAllPossStepsForCurrToPlay();

            if (m_PossibleMultiplyJumpsList.Count() != 0)
            {
                m_NextToPlay.CurrAmountOfPieces--;
                drawnNumber = rnd.Next(0, m_PossibleMultiplyJumpsList.Count() - 1);
                move = m_PossibleMultiplyJumpsList[drawnNumber];
                applayMoveOnBoard(move);
                deleteInBetween(move);
                m_PossibleMultiplyJumpsList.Clear();
                buildPossiblePieceJumpsList(move);
                if (m_PossibleMultiplyJumpsList.Count() != 0)
                {
                    m_IfDoubleJumpPossible = true;
                }
                else
                {
                    m_IfDoubleJumpPossible = false;
                }
            }
            else if (m_PossibleJumpsList.Count() != 0)
            {
                m_NextToPlay.CurrAmountOfPieces--;
                drawnNumber = rnd.Next(0, m_PossibleJumpsList.Count() - 1);
                move = m_PossibleJumpsList[drawnNumber];
                applayMoveOnBoard(move);
                deleteInBetween(move);
                buildPossiblePieceJumpsList(move);
                if (m_PossibleMultiplyJumpsList.Count() != 0)
                {
                    m_IfDoubleJumpPossible = true;
                }
            }
            else if (m_PossibleStepsList.Count() != 0)
            {
                drawnNumber = rnd.Next(0, m_PossibleStepsList.Count() - 1);
                move = m_PossibleStepsList[drawnNumber];
                applayMoveOnBoard(move);
            }
            else
            {
                m_TurnToPlay.IfOutOfMoves = true;
            }
        }

        public int         GetSizeOfBoard()
        {
            return m_Board.GetLength(0);
        }

        public void        SwitchPlayerTurn()
        {
            Player temp = m_TurnToPlay;

            m_TurnToPlay = m_NextToPlay;

            m_NextToPlay = temp;
        }

        private void       applayMoveOnBoard(Move i_Move)
        {
            bool ifCurrPieceIsKing = m_Board[i_Move.CurrRow, i_Move.CurrCol].IsKing;

            m_Board[i_Move.CurrRow, i_Move.CurrCol].Entity = eEntity.Empty;
            m_Board[i_Move.CurrRow, i_Move.CurrCol].IsKing = false;
            m_Board[i_Move.DestRow, i_Move.DestCol].Entity = m_TurnToPlay.EntityPlayer;

            if ((i_Move.DestRow == 0 && m_TurnToPlay.EntityPlayer == eEntity.Player1) ||
                (i_Move.DestRow == m_Board.GetLength(0) - 1 && m_TurnToPlay.EntityPlayer == eEntity.Player2))
            {
                m_Board[i_Move.DestRow, i_Move.DestCol].IsKing = true;
            }
            else
            {
                m_Board[i_Move.DestRow, i_Move.DestCol].IsKing = false;
            }

            if (ifCurrPieceIsKing)
            {
                m_Board[i_Move.DestRow, i_Move.DestCol].IsKing = true;
            }
        }

        private bool       checkTypeOfStepsValidation(int i_DestRow, int i_DestCol)
        {
            bool ifValidMove = false;

            if (i_DestRow >= 0 && i_DestRow < m_Board.GetLength(0) && i_DestCol >= 0 && i_DestCol < m_Board.GetLength(1))
            {
                if (m_Board[i_DestRow, i_DestCol].Entity == eEntity.Empty)
                {
                    ifValidMove = true;
                }
            }

            return ifValidMove;
        }

        public Player      CheckIfThereIsAWinner()
        {
            Player winner = null;

            if (m_NextToPlay.CurrAmountOfPieces == 0)
            {
                winner = m_TurnToPlay;
            }

            return winner;
        }

        public void        CalculatePlayersScore(Player i_Winner)
        {
            int player1Score = 0, player2Score = 0;
            int winnerFinalScore;

            foreach (Cell cell in m_Board)
            {
                if (cell.Entity == eEntity.Player1)
                {
                    if (cell.IsKing)
                    {
                        player1Score += 4;
                    }
                    else
                    {
                        player1Score++;
                    }
                }

                if (cell.Entity == eEntity.Player2)
                {
                    if (cell.IsKing)
                    {
                        player2Score += 4;
                    }
                    else
                    {
                        player2Score++;
                    }
                }
            }

            winnerFinalScore = Math.Max(player1Score, player2Score) - Math.Min(player1Score, player2Score);
            i_Winner.Score += winnerFinalScore;
        }
    }
}
