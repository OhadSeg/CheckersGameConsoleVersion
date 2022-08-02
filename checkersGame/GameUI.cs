using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using checkersLogic;

namespace checkersGame
{
    public enum eBoardSizeOptions
    {
        SixOnSix = 6,
        EightOnEight = 8,
        TenOnTen = 10,
        InvalidSize = -1
    }

    public class GameUI
    {

        // should be with prefix r_
        private readonly StringBuilder m_BoardDisplay;
        // should be with prefix r_
        private readonly char[] m_GamePiecesSigns;
        private bool m_PressedQ;

        internal GameUI()
        {
            m_BoardDisplay = new StringBuilder();
            const int k_AmountOfPieces = 4;
            m_PressedQ = false;

            m_GamePiecesSigns = new char[k_AmountOfPieces];
            m_GamePiecesSigns[0] = 'X';
            m_GamePiecesSigns[1] = 'K';
            m_GamePiecesSigns[2] = 'O';
            m_GamePiecesSigns[3] = 'U';
        }

        public bool PressedQ
        {
            get { return m_PressedQ; }
            set { m_PressedQ = value; }
        }

        public string             GetPlayerName()
        {
            string playerName = null;
            bool isValidName = false;

            Console.WriteLine("Please enter player name: ");

            while (!isValidName)
            {
                playerName = Console.ReadLine();

                if (!playerName.Any(char.IsWhiteSpace) && playerName.Length <= 20 && playerName != string.Empty)
                {
                    isValidName = true;
                }
                else
                {
                    Console.WriteLine("wrong input' try again: ");
                }
            }

            return playerName;
        }

        public int                GetBoardSize()
        {
            eBoardSizeOptions boardSizeSelection = eBoardSizeOptions.InvalidSize;
            string input = null;
            int parsedBoardSize;

            Console.WriteLine("Please enter desired board size (press 6/8/10): ");

            while (boardSizeSelection == eBoardSizeOptions.InvalidSize)
            {
                input = Console.ReadLine();

                if (int.TryParse(input, out parsedBoardSize))
                {
                    if (parsedBoardSize == (int)eBoardSizeOptions.SixOnSix || parsedBoardSize == (int)eBoardSizeOptions.EightOnEight || parsedBoardSize == (int)eBoardSizeOptions.TenOnTen)
                    {
                        boardSizeSelection = (eBoardSizeOptions)parsedBoardSize;
                    }
                }

                if (boardSizeSelection == eBoardSizeOptions.InvalidSize)
                {
                    Console.WriteLine("wrong input' try again: ");
                }
            }

            return (int)boardSizeSelection;
        }

        public Player.ePlayerType GetGameMode()
        {
            bool validInput = false;
            int parsedNumber;
            string input = null;
            Player.ePlayerType playerTypeInput = Player.ePlayerType.Computer;

            while (!validInput)
            {
                Console.WriteLine("press '1' to play against the computer or '2' to play against another player: ");
                input = Console.ReadLine();

                if (int.TryParse(input, out parsedNumber) && (parsedNumber == 1 || parsedNumber == 2))
                {
                    if (parsedNumber == 2)
                    {
                        playerTypeInput = Player.ePlayerType.Human;
                    }

                    validInput = true;
                }
            }

            return playerTypeInput;
        }

        public void               TurnMessege(Player i_CurrTurn)
        {
            Console.WriteLine("{0}'s Turn: ", i_CurrTurn.Name);
        }

        public Move               GetMoveFromUser(int i_SizeOfBoard, Player i_CurrTurn)
        {
            // should be moveInput
            Move MoveInput = null;
            string input = null;
            bool validInput = false;
 
            while (!validInput)
            {
                input = Console.ReadLine();

                if (ifPressedQ(input))
                {
                    m_PressedQ = true;
                    break;
                }

                validInput = checkIfValidMove(input, i_SizeOfBoard);

                if (!validInput)
                {
                    PrintInvalidMoveMessage(i_CurrTurn);
                }
            }

            if (validInput)
            {
                MoveInput = new Move(input[0] - 'A', input[1] - 'a', input[3] - 'A', input[4] - 'a');
            }

            return MoveInput;
        }

        public void               PrintInvalidMoveMessage(Player i_Player)
        {
            Console.WriteLine("{0}, you've entered invalid move, please Choose again: ", i_Player.Name);
        }

        private bool              checkIfValidMove(string i_Input, int i_SizeOfBoard)
        {
            // should be validInputLength
            // no need for const
            const int ValidInputLength = 5;
            int asciiOfA = (int)'A';
            int asciiOfa = (int)'a';
            bool validMove = true;

            if (i_Input.Length != ValidInputLength || i_Input[ValidInputLength / 2] != '>' || string.IsNullOrEmpty(i_Input))
            {
                validMove = false;
            }
            else
            {
                validMove = checkIfInputInScope(i_Input[0], asciiOfA, asciiOfA + i_SizeOfBoard)
                     && checkIfInputInScope(i_Input[1], asciiOfa, asciiOfa + i_SizeOfBoard)
                     && checkIfInputInScope(i_Input[3], asciiOfA, asciiOfA + i_SizeOfBoard)
                     && checkIfInputInScope(i_Input[4], asciiOfa, asciiOfa + i_SizeOfBoard);
            }

            return validMove;
        }

        private bool              checkIfInputInScope(char i_CharInput, int i_LowerBound, int i_UpperBound)
        {
            return i_CharInput >= i_LowerBound && i_CharInput <= i_UpperBound;
        }

        public bool               KeepPlaying()
        {
            string input = null;
            bool choice = false;
            bool validInput = false;

            Console.WriteLine("Do you want to keep playing? Y/N: ");

            while (!validInput)
            {
                input = Console.ReadLine();

                if (input.Equals("Y"))
                {
                    choice = true;
                    validInput = true;
                }
                else if (input.Equals("N"))
                {
                    choice = false;
                    validInput = true;
                }
            }

            return choice;
        }

        public void               PrintBoardDisplay(Cell[,] i_GameBoard)
        {
            Ex02.ConsoleUtils.Screen.Clear();
            int sizeOfBoard = i_GameBoard.GetLength(0);

            int colAscii = 65;
            int rowAscii = 97;

            for (int i = 0; i <= sizeOfBoard; i++)
            {
                for (int j = 0; j <= sizeOfBoard; j++)
                {
                    if (i == 0 && j < sizeOfBoard)
                    {
                        if (j == 0)
                        {
                            m_BoardDisplay.AppendFormat("    {0}", Convert.ToChar(colAscii));
                        }
                        else
                        {
                            m_BoardDisplay.AppendFormat("     {0}", Convert.ToChar(colAscii));
                        }

                        colAscii++;
                    }

                    if (i != 0 && j < sizeOfBoard)
                    {
                        m_BoardDisplay.AppendFormat("  {0}  |", GetPieceSymbol(i_GameBoard[i - 1, j]));
                    }
                }

                m_BoardDisplay.Append("\n");

                if (sizeOfBoard == 8)
                {
                    m_BoardDisplay.Append("  ================================================\n");
                }
                else if (sizeOfBoard == 6)
                {
                    m_BoardDisplay.Append("  ====================================\n");
                }
                else if (sizeOfBoard == 10)
                {
                    m_BoardDisplay.Append("  ============================================================\n");
                }

                if (i < sizeOfBoard)
                {
                    m_BoardDisplay.AppendFormat("{0}|", Convert.ToChar(rowAscii));
                    rowAscii++;
                }
            }

            Console.WriteLine(m_BoardDisplay);
            m_BoardDisplay.Clear();
        }

        // private methods starts with lowercase
        private char              GetPieceSymbol(Cell i_SpecificCell)
        {
            char symbol;

            switch (i_SpecificCell.Entity)
            {
                case checkersLogic.eEntity.Player1:

                    if (i_SpecificCell.IsKing)
                    {
                        symbol = m_GamePiecesSigns[1];
                    }
                    else
                    {
                        symbol = m_GamePiecesSigns[0];
                    }

                    break;

                case checkersLogic.eEntity.Player2:

                    if (i_SpecificCell.IsKing)
                    {
                        symbol = m_GamePiecesSigns[3];
                    }
                    else
                    {
                        symbol = m_GamePiecesSigns[2];
                    }

                    break;
                default:
                    symbol = ' ';
                    break;
            }

            return symbol;
        }

        public int                GetPlayerAmountOfPieces(int i_BoardSize)
        {
            int count = (int)((i_BoardSize - 2) * 0.5 * 0.5 * i_BoardSize);

            return count;
        }

        public void               PrintScoreMessege(Player i_Player1, Player i_Player2)
        {
            Console.WriteLine("{0}'s score: {1}", i_Player1.Name, i_Player1.Score);
            Console.WriteLine("{0}'s score: {1}", i_Player2.Name, i_Player2.Score);
        }

        public void               PrintGameResultMessege(Player i_Winner)
        {
            if (i_Winner != null)
            {
                Console.WriteLine("The winner is: {0}", i_Winner.Name);
            }
            else
            {
                Console.WriteLine("The game ended in a draw");
            }
        }

        public bool               IfKeepPlaying()
        {
            bool keepPlaying = false;
            string input = IfKeepPlayingMessege();

            if (input == "Y")
            {
                keepPlaying = true;
            }

            return keepPlaying;
        }

        private string            IfKeepPlayingMessege()
        {
            string input = null;
            bool validInput = false;

            Console.WriteLine("Do you want to keep playing? (Y/N):");
            while (!validInput)
            {
                input = Console.ReadLine();

                if (input == "N" || input == "Y")
                {
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Wrong input' try again");
                }
            }

            return input;
        }

        private bool              ifPressedQ(string i_Input)
        {
            return i_Input.Equals("Q");
        }
    }
}
