using ChceckersLogicComponents;
using PlayerSign = ChceckersLogicComponents.GameUtilities.ePlayerSign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace CheckersWinform
{
    public partial class CheckersBoard : Form
    {
        private const string k_BlackPlayerSign = "O";
        private const string k_WhitePlayerSign = "O";
        private CheckersGameLogic m_GameLogicComponent = null;
        private int m_CellSize = 0;
        private int m_BoardSize = 0;
        private BoardCell m_FirstCoords = new BoardCell().DefualtCell;
        private BoardCell m_SecondCoords = new BoardCell().DefualtCell;
        private bool m_IsFirstCoordsInsetred = false;
        private Player m_CurrentPlayerSign = null;
        public CheckersBoard(string i_FirstPlayerName, string i_SecondPlayerName = "Computer", bool i_IsSecondPlayerHuman = false)
        {
            m_GameLogicComponent = new CheckersGameLogic(i_FirstPlayerName, i_SecondPlayerName, i_IsSecondPlayerHuman);
            InitializeComponent();
            m_BoardSize = m_GameLogicComponent.BoardSize;
            m_CellSize = this.Width / m_BoardSize - 2;
        }

        private void CheckersBoard_Load(object sender, EventArgs e)
        {
            this.Size = getAppDefualtSize();
            int gridWidth = m_BoardSize * (m_CellSize + 10);
            int gridHeight = (m_BoardSize + 1) * m_CellSize;

            this.ClientSize = new Size(gridWidth, gridHeight);
            this.Location = getUserScreenCenterOfScreen();
            initializeButtons();
            PrintBoard();
        }
        private Point getUserScreenCenterOfScreen()
        {
            int centerScreenHeight = Screen.PrimaryScreen.Bounds.Height / 50;
            int centerOfScreenWidth = Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2;

            return new Point(centerOfScreenWidth, centerScreenHeight);
        }

        private Size getAppDefualtSize()
        {
            return new Size(2 * Screen.PrimaryScreen.Bounds.Width / 3, 9 * Screen.PrimaryScreen.Bounds.Height / 10);
        }

        private void initializeButtons()
        {
            for (int row = 0; row < m_BoardSize; row++)
            {
                for (int col = 0; col < m_BoardSize; col++)
                {
                    initializeAButton(row, col);
                }
            }
        }

        private void initializeAButton(int i_RowNumber, int i_ColNumber)
        {
            CheckersBoardCell currentCell = m_GameLogicComponent.GetCellValue(i_RowNumber, i_ColNumber);
            string buttonLocationAsIdentifier = $"{i_ColNumber},{i_RowNumber}";
            int distanceBetweenButtons = 9;
            Button btn = new Button();

            btn.Size = new Size(m_CellSize, m_CellSize);
            btn.Padding = new Padding(5);
            btn.Tag = buttonLocationAsIdentifier;
            btn.Location = new Point((m_CellSize + distanceBetweenButtons) * i_RowNumber + distanceBetweenButtons / 2, (m_CellSize + distanceBetweenButtons / 2) * i_ColNumber + distanceBetweenButtons / 2);
            btn.Click += Button_Clicked;
            this.Controls.Add(btn);
            btn.Font = new Font(btn.Font.FontFamily, btn.Font.Size + 10f, FontStyle.Bold);
            btn.TabStop = false;
            btn.Enabled = true;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            ///TODO:
            ///1. convert tag to to int for the indices - for the current and candidate cell.
            ///2. PlayMove or display error to screen as MessageBox
            Button clickedButton = sender as Button;
            int rowIndex = 0;
            int colIndex = 0;

            if(clickedButton != null )
            {
                convertBtnTagToIdices(ref rowIndex, ref colIndex, clickedButton.Tag.ToString());
                
                if(m_IsFirstCoordsInsetred == false)
                {
                    m_FirstCoords = new BoardCell(rowIndex, colIndex);
                    m_IsFirstCoordsInsetred = true;
                }
                else
                {
                    m_SecondCoords = new BoardCell(rowIndex, colIndex);
                    m_IsFirstCoordsInsetred = false;

                    try
                    {
                        m_GameLogicComponent.MakeAMove(m_FirstCoords, m_SecondCoords);
                        UpdateAndProceedGame();
                    }
                    catch (Exception exceptionToCatch)
                    {
                        MessageBox.Show(exceptionToCatch.Message);
                    }
                }

                if (m_GameLogicComponent.CurrentPlayerTurn.PlayerType == GameUtilities.ePlayersType.Computer)
                {
                    m_GameLogicComponent.MakeRandomMove();
                    UpdateAndProceedGame();
                }
            }
        }

        private void UpdateAndProceedGame()
        {
            PrintBoard();
           
            string message = "";
            string messageCaption = "";
            string winnerName = m_GameLogicComponent.CurrentPlayerTurn.Name;
            bool isAPopWindowNeedsToBeSent = false;
            
            if (m_GameLogicComponent.IsThereAWin())
            {
                message = string.Format($@"The Winner is {winnerName}!");
                isAPopWindowNeedsToBeSent = true;
            }

/*            if (isAPopWindowNeedsToBeSent)
            {
                askUserToPlayAnotherRound(message, messageCaption);
            }*/
        }

        private void convertBtnTagToIdices(ref int i_RowNumber, ref int i_ColNumber, string i_ButtonTag)
        {
            string[] tagIndices = i_ButtonTag.Split(',');

            i_RowNumber = int.Parse(tagIndices[0]);
            i_ColNumber = int.Parse(tagIndices[1]);
        }

        public void PrintBoard()
        {
            for (int rowIndex = 0; rowIndex < m_BoardSize; rowIndex++)
            {
                printRow(rowIndex);
            }
        }

        private void printRow(int i_Rowindex)
        {
            string btnTag = "";
            Button btn = null;

            for (int colIndex = 0; colIndex < m_BoardSize; colIndex++)
            {
                btnTag = $"{i_Rowindex},{colIndex}";
                btn = getButtenTag(btnTag);
                setBtnLabelAndBackColor(btn, m_GameLogicComponent.GetCellValue(i_Rowindex, colIndex).CellSign);
                
                if(i_Rowindex %2 == 0)
                {
                    btn.BackColor = (colIndex % 2 == 0) ? Color.White : Color.DarkGray;
                }
                else
                {
                    btn.BackColor = (colIndex % 2 != 0) ? Color.White : Color.DarkGray;
                }
            }
        }

        private void setBtnLabelAndBackColor(Button i_ButtonToChangeSettings, PlayerSign i_CellSign)
        {
            if (i_ButtonToChangeSettings != null)
            {
                switch (i_CellSign)
                {
                    case PlayerSign.empty:
                        i_ButtonToChangeSettings.Text = "";
                        break;
                    case PlayerSign.first:
                        i_ButtonToChangeSettings.Text = k_BlackPlayerSign;
                        i_ButtonToChangeSettings.ForeColor = Color.Black;
                        break;
                    case PlayerSign.second:
                        i_ButtonToChangeSettings.Text = k_WhitePlayerSign;
                        i_ButtonToChangeSettings.ForeColor = Color.White;
                        break;
                }
            }
        }

        private Button getButtenTag(string i_ButtonTag)
        {
            Button buttonToReturn = null;

            foreach (Control controlElement in this.Controls)
            {
                if (controlElement is Button btnConversion)
                {
                    if (btnConversion.Tag.ToString() == i_ButtonTag)
                    {
                        buttonToReturn = btnConversion;
                    }
                }
            }

            return buttonToReturn;
        }
    }
}

