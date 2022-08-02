using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace checkersLogic
{
    public class Player
    {
        public enum ePlayerType
        {
            Human,
            Computer
        }

        private readonly ePlayerType r_Type;
        private readonly eEntity r_EntityPlayer;
        private readonly string r_Name;
        private int m_CurrAmountOfPieces;
        private bool m_IfOutOfMoves;
        private int m_Score;

        public Player(ePlayerType i_Type, eEntity i_EntityPlayer, string i_Name, int i_CurrAmountOfPieces)
        {
            r_Type = i_Type;
            r_EntityPlayer = i_EntityPlayer;
            r_Name = i_Name;
            m_CurrAmountOfPieces = i_CurrAmountOfPieces;
            m_IfOutOfMoves = false;
            m_Score = 0;
        }

        public ePlayerType Type
        {
            get { return r_Type; }
        }

        public eEntity     EntityPlayer
        {
            get { return r_EntityPlayer; }
        }

        public string      Name
        {
            get { return r_Name; }
        }

        public int         CurrAmountOfPieces
        {
            get { return m_CurrAmountOfPieces; }
            set { m_CurrAmountOfPieces = value; }
        }

        public int         Score
        {
            get { return m_Score; }
            set
            {
                if (value >= 0)
                {
                    m_Score = value;
                }
            }
        }

        public bool        IfOutOfMoves
        {
            get { return m_IfOutOfMoves; }
            set { m_IfOutOfMoves = value; }
        }
    }
}
