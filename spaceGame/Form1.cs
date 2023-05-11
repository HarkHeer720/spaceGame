using System;
using System.Collections.Generic;
//im the biggest bird im the biggest bird
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace spaceGame
{
    public partial class Form1 : Form
    {
        //creating lists
        List<Rectangle> asteroidListRight = new List<Rectangle>();
        List<Rectangle> asteroidListLeft = new List<Rectangle>();
        List<int> asteroidSpeedsLeft = new List<int>();
        List<int> asteroidSpeedsRight = new List<int>();

        //declaring players
        Rectangle player1 = new Rectangle(50, 50, 50, 50);
        Rectangle player2 = new Rectangle(30, 30, 10, 10);

        //declaring a randomizer
        Random randGen = new Random();

        //declaring variables
        int playerSpeed = 6;
        int asteroidWidth = 15;
        int asteroidHeight = 5;
        string gameState = "startScreen";
        int player1Score = 0;
        int player2Score = 0;
        int randValue;

        //declaring button presses
        bool wDown = false;
        bool sDown = false;
        bool upDown = false;
        bool downDown = false;

        //declaring brushes and pens
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        public Form1()
        {
            InitializeComponent();
        }
        public void gameInitializer()
        {
            asteroidListRight.Clear();
            asteroidListLeft.Clear();
            asteroidSpeedsRight.Clear();
            asteroidSpeedsLeft.Clear();

            randValue = randGen.Next(0, this.Height - 50);
            asteroidSpeedsRight.Add(randGen.Next(3, 10));

            Rectangle asteroid = new Rectangle(600, randValue, asteroidWidth, asteroidHeight);
            asteroidListRight.Add(asteroid);

            player1Score = 0;
            player2Score = 0;

            gameState = "playing";
            gameTimer.Enabled = true;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Up:
                    upDown = true;
                    break;
                case Keys.Down:
                    downDown = true;
                    break;
                case Keys.Escape:
                    if (gameState == "startScreen" || gameState == "endScreen")
                    {
                        Application.Exit();
                    }
                    else if (gameState == "playing")
                    {

                    }
                    break;
                case Keys.Space:
                    if (gameState == "startScreen" || gameState == "endScreen")
                    {
                        gameInitializer();
                    }
                    else if (gameState == "playing")
                    {

                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Up:
                    upDown = false;
                    break;
                case Keys.Down:
                    downDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move player 1
            if (wDown == true)
            {
                player1.Y -= playerSpeed;
            }
            if (sDown == true)
            {
                player1.Y += playerSpeed;
            }

            //move player 2
            if (upDown == true)
            {
                player2.Y -= playerSpeed;
            }
            if (downDown == true)
            {
                player2.Y += playerSpeed;
            }

            //create asteroids on left side
            randValue = randGen.Next(1, 101);
            if (randValue <= 10)
            {
                randValue = randGen.Next(0, this.Height - 50);
                asteroidSpeedsLeft.Add(randGen.Next(3, 10));

                Rectangle ball = new Rectangle(0, randValue, asteroidWidth, asteroidHeight);
                asteroidListLeft.Add(ball);
            }

            //move left side asteroids
            for (int i = 0; i < asteroidListLeft.Count(); i++)
            {
                int x = asteroidListLeft[i].X + asteroidSpeedsLeft[i];
                asteroidListLeft[i] = new Rectangle(x, asteroidListLeft[i].Y, asteroidWidth, asteroidHeight);
            }

            //create asteroids on right side
            randValue = randGen.Next(1, 101);
            if (randValue <= 10)
            {
                randValue = randGen.Next(600, this.Height - 50);
                asteroidSpeedsRight.Add(randGen.Next(3, 10));

                Rectangle asteroid = new Rectangle(600, randValue, asteroidWidth, asteroidHeight);
                asteroidListRight.Add(asteroid);
            }

            //move right side asteroids
            for (int i = 0; i < asteroidListRight.Count(); i++)
            {
                int x = asteroidListRight[i].Y - asteroidSpeedsRight[i];
                asteroidListRight[i] = new Rectangle(x, asteroidListRight[i].Y, asteroidWidth, asteroidHeight);
            }

            //checking if a player has scored
            if (player1.Y < 0)
            {
                player1Score ++;

                player1.Y = 500;

                //sleep beacuse otherwise the score never shows 3 but the game ends
                Refresh();
                Thread.Sleep(5);
            }
            if (player2.Y < 0)
            {
                player2Score++;
            }

            //checking if a player has won
            if (player1Score >= 3)
            {
                subtitleLabel.Text = "Player 1 wins";
                gameState = "endScreen";
            }
            if (player2Score == 3)
            {
                subtitleLabel.Text = "Player 2 wins";
                gameState = "endScreen";
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //redrawing the screen depending on the game state
            if (gameState == "startScreen")
            {
                gameTimer.Enabled = false;
            }
            if (gameState == "playing")
            {
                //clearing the labels
                titleLabel.Text = "";
                subtitleLabel.Text = "";

                //draw the players
                e.Graphics.FillRectangle(whiteBrush, player1);
                e.Graphics.FillRectangle(whiteBrush, player2);

                //draw left asteroids
                for (int i = 0; i < asteroidListLeft.Count(); i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, asteroidListLeft[i]);
                }

                //draw right asteroids
                for (int i = 0; i < asteroidListRight.Count(); i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, asteroidListRight[i]);
                }

                Player1ScoreLabel.Text = $"{player1Score}";
                Player2ScoreLabel.Text = $"{player2Score}";
            }
            if (gameState == "endScreen")
            {
                gameTimer.Enabled = false;
                titleLabel.Text = "Space Race";
                subtitleLabel.Text += "\nPress space to play again or press escape to exit";
            }
        }
    }
}
