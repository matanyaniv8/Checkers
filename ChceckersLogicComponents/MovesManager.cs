using System;
using System.Collections.Generic;
using PlayerSign = ChceckersLogicComponents.GameUtilities.ePlayerSign;

namespace ChceckersLogicComponents
{
    internal class MovesManager
    {
        private readonly Board r_GameBoard;
        internal Board CheckersBoard { get { return r_GameBoard; } }

        internal MovesManager(Board i_GameBoard)
        {
            if (i_GameBoard != null)
            {
                r_GameBoard = i_GameBoard;
            }
            else
            {
                throw new ArgumentNullException("Please Insert a Checkers Game Board");
            }
        }

        // Checks if the Player's move is a right eat move.
        private KeyValuePair<BoardCell, bool> isMoveIsRightEatMove(Player i_PlayerThatMakesTheMove, BoardCell i_CurrentCell, BoardCell i_PotenitialCandidateCell)
        {
            BoardCell opponentLocation = i_CurrentCell;

            if (r_GameBoard.IsIndicesWithinRange(i_PotenitialCandidateCell)) // DOTO: there's a double check here and in the isMoveCellReadyToBeEaten method.
            {
                if (i_PlayerThatMakesTheMove.PlayerSign == PlayerSign.first)
                {
                    opponentLocation = new BoardCell(i_PotenitialCandidateCell.X + 1, i_PotenitialCandidateCell.Y - 1);
                }
                else
                {
                    opponentLocation = new BoardCell(i_PotenitialCandidateCell.X - 1, i_PotenitialCandidateCell.Y - 1);
                }
            }
            else
            {
                throw new Exception("Indices aren't within board size");
            }

            return new KeyValuePair<BoardCell, bool>(opponentLocation, r_GameBoard.IsIndicesWithinRange(opponentLocation));
        }

        // Check if the Player's move is a left eat move.
        private KeyValuePair<BoardCell, bool> isMoveIsLeftEatMove(Player i_PlayerThatMakesTheMove, BoardCell i_CurrentCell, BoardCell i_PotenitialCandidateCell)
        {
            BoardCell opponentLocation = i_CurrentCell;

            if (r_GameBoard.IsIndicesWithinRange(i_PotenitialCandidateCell))
            {
                if (i_PlayerThatMakesTheMove.PlayerSign == PlayerSign.first)
                {
                    opponentLocation = new BoardCell(i_PotenitialCandidateCell.X + 1, i_PotenitialCandidateCell.Y + 1);
                }
                else
                {
                    opponentLocation = new BoardCell(i_PotenitialCandidateCell.X - 1, i_PotenitialCandidateCell.Y + 1);
                }
            }
            else
            {
                throw new Exception("Indices aren't within board size");
            }

            return new KeyValuePair<BoardCell, bool>(opponentLocation, r_GameBoard.IsIndicesWithinRange(opponentLocation));
        }

        // Aggrigate the checking of an enemy point under a method to avoid code repetitive 
        private KeyValuePair<BoardCell, bool> isEnemyCellOk(BoardCell i_CurrentCell, BoardCell i_EnemyCell)
        {
            if (r_GameBoard.IsIndicesWithinRange(i_EnemyCell))
            {
                i_EnemyCell.CheckersReleventInfo = r_GameBoard.GetPlayerCellFromBoard(i_EnemyCell);
            }

            return new KeyValuePair<BoardCell, bool>(i_EnemyCell, isMoveCellReadyToBeEaten(i_CurrentCell, i_EnemyCell));
        }

        // Gets the Opponent cell on an eat move.
        private KeyValuePair<BoardCell, bool> getEnemyCellIfAvaiableOnAnEatMove(Player i_CurrenctPlayer, BoardCell i_CurrentCell, BoardCell i_CandidateCell)
        {
            KeyValuePair<BoardCell, bool> moveDest = isMoveIsLeftEatMove(i_CurrenctPlayer, i_CurrentCell, i_CandidateCell);

            moveDest = isEnemyCellOk(i_CurrentCell, moveDest.Key);

            if (moveDest.Value == false)
            {
                moveDest = isMoveIsRightEatMove(i_CurrenctPlayer, i_CurrentCell, i_CandidateCell);
                moveDest = isEnemyCellOk(i_CurrentCell, moveDest.Key);
            }

            return moveDest;
        }

        // Checks What is the move and tries to applied it.
        internal bool MakeMove(Player i_CurrenctPlayer, Player i_OpponentPlayer, BoardCell i_CurrentCell, BoardCell i_CandidateCell)
        {
            bool moveSucceeded = false;
            KeyValuePair<BoardCell, bool> destCellIfAvailable;

            if (IsPlayerMoveAnEatMove(i_CurrenctPlayer, i_CurrentCell, i_CandidateCell))
            {
                destCellIfAvailable = getEnemyCellIfAvaiableOnAnEatMove(i_CurrenctPlayer, i_CurrentCell, i_CandidateCell);

                if (destCellIfAvailable.Value == true)
                {
                    moveSucceeded = MakeEatMove(i_CurrenctPlayer.PlayerSign, i_CurrentCell, destCellIfAvailable.Key, i_CandidateCell);
                    i_OpponentPlayer.NumberOfTroopsRemaining = (moveSucceeded) ? i_OpponentPlayer.NumberOfTroopsRemaining-- : i_OpponentPlayer.NumberOfTroopsRemaining; // Updates the opponent on eat succeeded.
                }
            }
            else if (IsCurrentMoveIsMovingWithoutEating(i_CurrenctPlayer, i_CurrentCell, i_CandidateCell))
            {
                moveSucceeded = makeNonEatingMove(i_CurrenctPlayer, i_OpponentPlayer, i_CurrentCell, i_CandidateCell);
            }
            else
            {
                throw new Exception("Please insert legit move!");
            }

            // if move succeeded, checks if a players becomes a king.
            if (moveSucceeded &&
                (i_CandidateCell.X == 0 || i_CandidateCell.X == r_GameBoard.BoardSize - 1 && i_CandidateCell != i_CurrentCell))
            {
                r_GameBoard.GameBoard[(int)i_CandidateCell.X, (int)i_CandidateCell.Y].IsCellPlayerAKing = true;
            }

            return moveSucceeded;
        }

        // Make an eat move.
        internal bool MakeEatMove(PlayerSign i_CurrenctPlayer, BoardCell i_CurrentCell, BoardCell i_EnemyCell, BoardCell i_DestCell)
        {
            bool isEatMoveSucceeded = false;

            try
            {
                r_GameBoard.GameBoard[i_DestCell.X, i_DestCell.Y] = r_GameBoard.GameBoard[i_CurrentCell.X, i_CurrentCell.Y];
                r_GameBoard.GameBoard[i_EnemyCell.X, i_EnemyCell.Y] = new CheckersBoardCell();
                r_GameBoard.GameBoard[i_CurrentCell.X, i_CurrentCell.Y] = new CheckersBoardCell();
                isEatMoveSucceeded = true;
            }
            catch
            {
                isEatMoveSucceeded = false;
            }

            return isEatMoveSucceeded;
        }

        //Applied Non eating move on the board.
        private bool makeNonEatingMove(Player i_PlayerThatMakesTheMove, Player i_OpponentPlayer, BoardCell i_CurrentCell, BoardCell i_CandidateCell)
        {
            bool isMoveBeenMadeSuccessfully = false;
            CheckersBoardCell candidateLocationInfo = r_GameBoard.GetPlayerCellFromBoard(i_CandidateCell);
            CheckersBoardCell currentLocationInfo = r_GameBoard.GetPlayerCellFromBoard(i_CurrentCell);

            if (candidateLocationInfo.CellSign == PlayerSign.empty && currentLocationInfo.CellSign == i_PlayerThatMakesTheMove.PlayerSign)
            {
                try
                {
                    r_GameBoard.SetPlayerCellInGameBoard(i_CandidateCell, r_GameBoard.GetPlayerCellFromBoard(i_CurrentCell));
                    r_GameBoard.SetPlayerCellInGameBoard(i_CurrentCell, candidateLocationInfo);
                    isMoveBeenMadeSuccessfully = true;
                }
                catch (Exception e)
                {
                    isMoveBeenMadeSuccessfully = false;
                    throw e;
                }
            }
            else
            {
                throw new ArgumentException("Players Sign Doesnt Match Board Info!");
            }

            return isMoveBeenMadeSuccessfully;
        }

        // Check if the coords of the next move are valid and checks if there's an enemy in that coords in the board.
        private bool isMoveCellReadyToBeEaten(BoardCell i_CurrentCell, BoardCell i_CandidateCell)
        {
            bool isCellIsReadyToBeEaten = false;
            PlayerSign currenctCellSign = PlayerSign.empty;
            PlayerSign nextCellSign = PlayerSign.empty;

            if (r_GameBoard.IsIndicesWithinRange(i_CandidateCell))
            {
                currenctCellSign = r_GameBoard.GetPlayerCellFromBoard(i_CurrentCell).CellSign;
                nextCellSign = r_GameBoard.GetPlayerCellFromBoard(i_CandidateCell).CellSign;
                isCellIsReadyToBeEaten = currenctCellSign != nextCellSign && currenctCellSign != PlayerSign.empty && nextCellSign != PlayerSign.empty;
            }

            return isCellIsReadyToBeEaten;
        }

        private bool isCurrentMoveRightNonEatingMove(Player i_PlayerThatMakesAMove, BoardCell i_CurrentLocation, BoardCell i_CandidatePoint)
        {
            bool isRightMoveLegit = false;
            int horizontalDistance = i_CurrentLocation.X - i_CandidatePoint.X;
            int verticalDistance = (int)(i_CurrentLocation.Y - i_CandidatePoint.Y);
            int horizontalDistanceFactor = (i_PlayerThatMakesAMove.PlayerSign == PlayerSign.first) ? 1 : -1;
            int verticalDistanceFactor = (i_PlayerThatMakesAMove.PlayerSign == PlayerSign.first) ? -1 : 1;
            PlayerSign candiateLocatin;
            PlayerSign currentPlayerSign;

            if (r_GameBoard.IsIndicesWithinRange(i_CandidatePoint))
            {
                candiateLocatin = r_GameBoard.GetPlayerCellFromBoard(i_CandidatePoint).CellSign;
                currentPlayerSign = r_GameBoard.GetPlayerCellFromBoard(i_CurrentLocation).CellSign;
                isRightMoveLegit = horizontalDistance == horizontalDistanceFactor && verticalDistance == verticalDistanceFactor;
                isRightMoveLegit &= currentPlayerSign == i_PlayerThatMakesAMove.PlayerSign && candiateLocatin == PlayerSign.empty;
            }
            else
            {
                throw new Exception("Not valid coords!");
            }

            return isRightMoveLegit;
        }


        private bool isCurrentMoveLeftNonEatingMove(Player i_PlayerThatMakesAMove, BoardCell i_CurrentLocation, BoardCell i_CandidatePoint)
        {
            bool isLeftMoveLegit = false;
            int horizontalDistance = i_CurrentLocation.X - i_CandidatePoint.X;
            int verticalDistance = (int)(i_CurrentLocation.Y - i_CandidatePoint.Y);
            int horizontalDistanceFactor = (i_PlayerThatMakesAMove.PlayerSign == PlayerSign.first) ? 1 : -1;
            int verticalDistanceFactor = (i_PlayerThatMakesAMove.PlayerSign == PlayerSign.first) ? 1 : -1;
            PlayerSign candiateLocatin;
            PlayerSign currentPlayerSign;

            if (r_GameBoard.IsIndicesWithinRange(i_CandidatePoint))
            {
                candiateLocatin = r_GameBoard.GetPlayerCellFromBoard(i_CandidatePoint).CellSign;
                currentPlayerSign = r_GameBoard.GetPlayerCellFromBoard(i_CurrentLocation).CellSign;
                isLeftMoveLegit = horizontalDistance == horizontalDistanceFactor && verticalDistance == verticalDistanceFactor;
                isLeftMoveLegit &= currentPlayerSign == i_PlayerThatMakesAMove.PlayerSign && candiateLocatin == PlayerSign.empty;
            }
            else
            {
                throw new Exception("Not valid coords!");
            }

            return isLeftMoveLegit;
        }

        internal bool IsCurrentMoveIsMovingWithoutEating(Player i_PlayerThatMakesAMove, BoardCell i_CurrentLocation, BoardCell i_CandidatePoint)
        {
            bool isMoveLegit = isCurrentMoveLeftNonEatingMove(i_PlayerThatMakesAMove, i_CurrentLocation, i_CandidatePoint);

            isMoveLegit |= isCurrentMoveRightNonEatingMove(i_PlayerThatMakesAMove, i_CurrentLocation, i_CandidatePoint);

            return isMoveLegit;
        }

        internal bool IsPlayerMoveAnEatMove(Player i_PlayerThatMakesAMove, BoardCell i_CurrentLocation, BoardCell i_CandidatePoint)
        {
            PlayerSign currentCellSign = i_PlayerThatMakesAMove.PlayerSign;
            PlayerSign candidateCellSign = r_GameBoard.GetPlayerCellFromBoard(i_CandidatePoint).CellSign;
            bool isCellsAreDiffrents = currentCellSign != candidateCellSign && candidateCellSign == PlayerSign.empty;
            int horizontalDistance = (int)(i_CurrentLocation.X - i_CandidatePoint.X);
            int verticalDistance = (int)(i_CurrentLocation.Y - i_CandidatePoint.Y);
            bool isRightMove = horizontalDistance != verticalDistance;
            bool isMoveAnEatMove = false;
            int horizontalDistanceFactor = 2;
            int verticalDistanceFactor = horizontalDistanceFactor;

            if (isRightMove)
            {
                if (currentCellSign == PlayerSign.first)
                {
                    horizontalDistanceFactor = 2;
                    verticalDistanceFactor = -2;
                }
                else
                {
                    horizontalDistanceFactor = -2;
                    verticalDistanceFactor = 2;
                }
            }

            if (isCellsAreDiffrents)
            {
                isMoveAnEatMove = horizontalDistance == horizontalDistanceFactor && verticalDistance == verticalDistanceFactor;
            }

            // If currentTroop is a king, it can eat backwards.
            if (!isMoveAnEatMove)
            {
                verticalDistanceFactor = 2;
                horizontalDistanceFactor = 2;

                if (isCurrentTroopAKing(i_CurrentLocation))
                {
                    isMoveAnEatMove = (Math.Abs(horizontalDistance) == horizontalDistanceFactor
                        && Math.Abs(verticalDistance) == verticalDistanceFactor);
                }
            }

            isMoveAnEatMove = isMoveAnEatMove && getEnemyCellIfAvaiableOnAnEatMove(i_PlayerThatMakesAMove, i_CurrentLocation, i_CandidatePoint).Value;

            return isMoveAnEatMove;
        }

        private bool isCurrentTroopAKing(BoardCell i_CurrentMovingTroop)
        {
            bool isTroopAKing = false;

            try
            {
                isTroopAKing = r_GameBoard.GetPlayerCellFromBoard(i_CurrentMovingTroop).IsCellPlayerAKing;
            }
            catch
            {
                isTroopAKing = false;
            }

            return isTroopAKing;
        }
    }
}

        /*        private KeyValuePair<BoardCell, bool> isNextMoveIsAnEatMove(Player i_PlayerThatMakesTheMove, Player i_Opponent, BoardCell i_CurrentCell, BoardCell i_CandidateCell)
                {
                    bool isEatMove = false;
                    BoardCell cellToReturn = new BoardCell(0, 0);
                    BoardCell rightMove = cellToReturn;
                    BoardCell leftMove = cellToReturn;

                    if (!isCurrentTroopAKing(i_CurrentCell))
                    {
                        if (i_PlayerThatMakesTheMove.PlayerSign == PlayerSign.first)
                        {
                            rightMove = new BoardCell(i_CurrentCell.X - 1, i_CurrentCell.Y + 1, null, PlayerSign.second);
                            leftMove = new BoardCell(i_CurrentCell.X - 1, i_CurrentCell.Y - 1, null, PlayerSign.second);
                        }
                        else if (i_PlayerThatMakesTheMove.PlayerSign == PlayerSign.second)
                        {
                            rightMove = new BoardCell(i_CurrentCell.X + 1, i_CurrentCell.Y + 1, null, PlayerSign.first);
                            leftMove = new BoardCell(i_CurrentCell.X + 1, i_CurrentCell.Y - 1, null, PlayerSign.first);
                        }

                        if (isMoveCellReadyToBeEaten(i_CurrentCell, rightMove))
                        {
                            cellToReturn = rightMove;
                            isEatMove = true;
                        }
                        else if (isMoveCellReadyToBeEaten(i_CurrentCell, leftMove))
                        {
                            cellToReturn = leftMove;
                            isEatMove = true;
                        }
                        else
                        {
                            throw new Exception("Indices not in range of board size!");
                        }
                    }

                    return new KeyValuePair<BoardCell, bool>(cellToReturn, isEatMove);
                }*/

        /*        internal bool IsPlayerTryingToMoveForwardOrBackward(PlayerSign i_PlayerThatMakesTheMove, BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
        {
            bool isTryingToMoveForward = isMovingForwardAllowed(i_PlayerThatMakesTheMove, i_CurrentTroopLocation, i_NextTroopLocation);
            bool isTryingToMoveBackwards = isMovingBackwardAllowed(i_PlayerThatMakesTheMove, i_CurrentTroopLocation, i_NextTroopLocation);
            bool isNextCellLocationSignIsEmpty = r_GameBoard.GetPlayerCellFromBoard(i_NextTroopLocation).CellSign == PlayerSign.empty;
            bool isCurrentPlayerSlotIsNotEmpty = r_GameBoard.GetPlayerCellFromBoard(i_CurrentTroopLocation).CellSign != PlayerSign.empty;

            return (isTryingToMoveForward || isTryingToMoveBackwards) && isNextCellLocationSignIsEmpty && isCurrentPlayerSlotIsNotEmpty;
        }*/

        /*        private bool isMovingBackwardAllowed(PlayerSign i_PlayerThatMakesTheMoveSign, BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
                {
                    bool isTryingToMoveBackward = i_NextTroopLocation.Y == i_CurrentTroopLocation.Y && isCurrentTroopAKing(i_CurrentTroopLocation);
                    int bound4MovingBackwards = 3;

                    if (isTryingToMoveBackward == true)
                    {
                        if (i_PlayerThatMakesTheMoveSign == PlayerSign.first)
                        {
                            bound4MovingBackwards = r_GameBoard.BoardSize - 3;
                            isTryingToMoveBackward &= (i_CurrentTroopLocation.X + 1 == i_NextTroopLocation.X && i_CurrentTroopLocation.X < bound4MovingBackwards);
                        }
                        else if (i_PlayerThatMakesTheMoveSign == PlayerSign.second)
                        {
                            bound4MovingBackwards = 3;
                            isTryingToMoveBackward &= (i_CurrentTroopLocation.X - 1 == i_NextTroopLocation.X && i_CurrentTroopLocation.X >= bound4MovingBackwards);
                        }
                    }

                    return isTryingToMoveBackward;
                }*/

        /*        private bool isMovingForwardAllowed(PlayerSign i_PlayerThatMakesTheMoveSign, BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
                {
                    bool isTryingToMoveForward = i_CurrentTroopLocation.Y == i_NextTroopLocation.Y && isCurrentTroopAKing(i_CurrentTroopLocation);
                    bool isCurrentPlayerAKing = *//*r_RowsNumbersForADoubleMove.Contains(i_CurrentTroopLocation.X) && *//*isCurrentTroopAKing(i_CurrentTroopLocation);
                    int currentRowIndex = i_CurrentTroopLocation.X;

                    if (isTryingToMoveForward)
                    {
                        if (i_PlayerThatMakesTheMoveSign == PlayerSign.first)
                        {
                            //currentRowIndex = (isCurrentPlayerAKing) ? currentRowIndex - 1 : currentRowIndex ;
                            isTryingToMoveForward &= (*//*currentRowIndex == i_NextTroopLocation.X ||*//* currentRowIndex - 1 == i_NextTroopLocation.X);
                        }
                        else if (i_PlayerThatMakesTheMoveSign == PlayerSign.second)
                        {
                            //currentRowIndex = (isCurrentPlayerAKing) ? currentRowIndex + 1: currentRowIndex ;
                            isTryingToMoveForward &= *//*currentRowIndex == i_NextTroopLocation.X ||*//* currentRowIndex + 1 == i_NextTroopLocation.X;
                        }
                    }

                    return isTryingToMoveForward && isCurrentPlayerAKing;
                }*/
    
