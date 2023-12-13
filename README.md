# CheckersGame
#### A self learninig project using C# and winform to a Checkers Game implemented with Design Patterns.
Overview

This is a simple implementation of the classic game of checkers in C#. The game features a winForm interface where two players can take turns making moves on the checkers board.
Additionally It's a one game player as well, just play against the computer.

How to Play

Setup:
The game is played on an 8x8 checkers board.
Each player starts with 12 pieces placed on the dark squares of the three rows closest to them.
Players take turns making diagonal moves, and the goal is to capture all of the opponent's pieces or block them so they cannot make a move.
Movement:
Pieces move diagonally forward.
If a piece reaches the opponent's back row, it is "kinged" and can move both forward and backward.
Capture:
A piece captures an opponent's piece by jumping over it diagonally.
If a capture is possible after a move, the player must make the capture.
Winning:
The game ends when one player captures all of the opponent's pieces or blocks them so they cannot make a move.
How to Run

Requirements:
Make sure you have the latest version of .NET installed on your machine.
Clone the Repository:
bash
Copy code
git clone https://github.com/yourusername/checkers-csharp.git
cd checkers-csharp
Run the Game:
arduino
Copy code
dotnet run
Follow On-Screen Instructions:
The game will prompt you to enter your moves in a format like "A3 to B4."
Enjoy the Game:
Play with a friend and have fun!
Files

Program.cs: Contains the main game logic and user interface.
Board.cs: Manages the game board and piece placement.
Player.cs: Represents a player with their pieces.
Piece.cs: Defines the properties and behavior of a checkers piece.
Customization

Feel free to customize and extend the game as you wish! You can add features such as:

Additional game modes (e.g., single-player against an AI).
Improved graphics or a graphical user interface.
Save and load game functionality.
Keep track of the game history.
Implement a scoring system.
License

This project is licensed under the MIT License - see the LICENSE file for details.

Acknowledgments

This project is inspired by the classic game of checkers.
Special thanks to the C# and .NET community for their valuable contributions.
Have fun playing checkers in C#! If you have any questions or suggestions, feel free to open an issue or contribute to the project.
