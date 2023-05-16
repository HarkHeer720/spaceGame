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
using System.Media;

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
        Rectangle player1 = new Rectangle(145, 670, 20, 20);
        Rectangle player2 = new Rectangle(445, 670, 20, 20);

        Rectangle player1Top = new Rectangle(150, 650, 10, 20);
        Rectangle player2Top = new Rectangle(450, 650, 10, 20);

        Rectangle player1LeftBoost = new Rectangle(145, 690, 5, 10);
        Rectangle player1RightBoost = new Rectangle(161, 690, 5, 10);

        Rectangle player2LeftBoost = new Rectangle(445, 690, 5, 10);
        Rectangle player2RightBoost = new Rectangle(461, 690, 5, 10);

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
        SolidBrush orangeBrush = new SolidBrush(Color.Orange);
        Pen whitePen = new Pen(Color.White, 4);

        //creating sound effetcs
        SoundPlayer rocketSound = new SoundPlayer(Properties.Resources.rocketSound2);

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
                player1Top.Y -= playerSpeed;
                player1LeftBoost.Y -= playerSpeed;
                player1RightBoost.Y -= playerSpeed;

                rocketSound.Play();
            }
            if (sDown == true && player1.Y < this.Height - player1.Height)
            {
                player1.Y += playerSpeed;
                player1Top.Y += playerSpeed;
                player1LeftBoost.Y += playerSpeed;
                player1RightBoost.Y += playerSpeed;

                rocketSound.Stop();
            }

            //move player 2
            if (upDown == true)
            {
                player2.Y -= playerSpeed;
                player2Top.Y -= playerSpeed;
                player2LeftBoost.Y -= playerSpeed;
                player2RightBoost.Y -= playerSpeed;

                rocketSound.Play();
            }
            if (downDown == true && player2.Y < this.Height - player2.Height)
            {
                player2.Y += playerSpeed;
                player2Top.Y += playerSpeed;
                player2LeftBoost.Y += playerSpeed;
                player2RightBoost.Y += playerSpeed;

                rocketSound.Stop();
            }

            //create asteroids on left side
            randValue = randGen.Next(1, 101);
            if (randValue <= 14)
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
            if (randValue <= 14)
            {
                randValue = randGen.Next(0, this.Height - 50);
                asteroidSpeedsRight.Add(randGen.Next(3, 10));

                Rectangle asteroid = new Rectangle(600, randValue, asteroidWidth, asteroidHeight);
                asteroidListRight.Add(asteroid);
            }

            //move right side asteroids
            for (int i = 0; i < asteroidListRight.Count(); i++)
            {
                int x = asteroidListRight[i].X - asteroidSpeedsRight[i];
                asteroidListRight[i] = new Rectangle(x, asteroidListRight[i].Y, asteroidWidth, asteroidHeight);
            }

            //check if player 1 hit an asteroid
            for (int i = 0; i < asteroidListRight.Count(); i++)
            {
                if (player1.IntersectsWith(asteroidListRight[i]) || player1Top.IntersectsWith(asteroidListRight[i]))
                {
                    asteroidListRight.RemoveAt(i);
                    asteroidSpeedsRight.RemoveAt(i);
                    player1.Y = 670;
                    player1Top.Y = 650;
                    player1LeftBoost.Y = 690;
                    player1RightBoost.Y = 690;
                }
            }
            for (int i = 0; i < asteroidListLeft.Count(); i++)
            {
                if (player1.IntersectsWith(asteroidListLeft[i]) || player1Top.IntersectsWith(asteroidListLeft[i]))
                {
                    asteroidListLeft.RemoveAt(i);
                    asteroidSpeedsLeft.RemoveAt(i);
                    player1.Y = 670;
                    player1Top.Y = 650;
                    player1LeftBoost.Y = 690;
                    player1RightBoost.Y = 690;
                }
            }

            //check if player 2 hit an asteroid
            for (int i = 0; i < asteroidListRight.Count(); i++)
            {
                if (player2.IntersectsWith(asteroidListRight[i]) || player2Top.IntersectsWith(asteroidListRight[i]))
                {
                    asteroidListRight.RemoveAt(i);
                    asteroidSpeedsRight.RemoveAt(i);
                    player2.Y = 670;
                    player2Top.Y = 650;
                    player2LeftBoost.Y = 690;
                    player2RightBoost.Y = 690;
                }
            }
            for (int i = 0; i < asteroidListLeft.Count(); i++)
            {
                if (player2.IntersectsWith(asteroidListLeft[i]) || player2Top.IntersectsWith(asteroidListLeft[i]))
                {
                    asteroidListLeft.RemoveAt(i);
                    asteroidSpeedsLeft.RemoveAt(i);
                    player2.Y = 670;
                    player2Top.Y = 650;
                    player2LeftBoost.Y = 690;
                    player2RightBoost.Y = 690;
                }
            }

            //checking if a player has scored
            if (player1.Y < 0)
            {
                player1Score++;

                player1.Y = 670;
                player1Top.Y = 650;
                player1LeftBoost.Y = 690;
                player1RightBoost.Y = 690;

                //sleep beacuse otherwise the score never shows 3 but the game ends
                Refresh();
                Thread.Sleep(5);
            }
            if (player2.Y < 0)
            {
                player2Score++;

                player2.Y = 670;
                player2Top.Y = 650;
                player2LeftBoost.Y = 690;
                player2RightBoost.Y = 690;

                //sleep beacuse otherwise the score never shows 3 but the game ends
                Refresh();
                Thread.Sleep(5);
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
            if (gameState == "startScreen")
            {

            }
            else if (gameState == "playing")
            {
                titleLabel.Text = "";
                subtitleLabel.Text = "";

                //draw the players
                e.Graphics.DrawRectangle(whitePen, player1);
                e.Graphics.DrawRectangle(whitePen, player2);
                e.Graphics.DrawRectangle(whitePen, player1Top);
                e.Graphics.DrawRectangle(whitePen, player2Top);

                //drawing boost
                if (wDown == true)
                {
                    e.Graphics.FillRectangle(orangeBrush, player1LeftBoost);
                    e.Graphics.FillRectangle(orangeBrush, player1RightBoost);
                }
                if (upDown == true)
                {
                    e.Graphics.FillRectangle(orangeBrush, player2LeftBoost);
                    e.Graphics.FillRectangle(orangeBrush, player2RightBoost);
                }


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
            else if (gameState == "endScreen")
            {
                gameTimer.Enabled = false;
                titleLabel.Text = "Space Race";
                subtitleLabel.Text += "\nPress space to play again or press escape to exit";
            }
        }
    }
}