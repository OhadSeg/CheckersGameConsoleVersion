using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace checkersLogic
{
    public class Move
    {
        private readonly int m_currRow;
        private readonly int m_currCol;
        private readonly int m_destRow;
        private readonly int m_destCol;

        public Move(int i_currCol, int i_currRow, int i_destCol, int i_destRow)
        {
            m_currCol = i_currCol;
            m_currRow = i_currRow;
            m_destCol = i_destCol;
            m_destRow = i_destRow;
        }

        public int CurrRow
        {
            get { return m_currRow; }
        }

        public int CurrCol
        {
            get { return m_currCol; }
        }

        public int DestRow
        {
            get { return m_destRow; }
        }

        public int DestCol
        {
            get { return m_destCol; }
        }
    }
}
