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
        List<Rectangle> asteroidSpecialRight = new List<Rectangle>();
        List<Rectangle> asteroidSpecialLeft = new List<Rectangle>();
        List<int> asteroidSpeedsRightSpecial = new List<int>();
        List<int> asteroidSpeedsLeftSpecial = new List<int>();

        //declaring players
        Rectangle player1 = new Rectangle(145, 670, 20, 20);
        Rectangle player2 = new Rectangle(445, 670, 20, 20);

        Rectangle player1Middle = new Rectangle(150, 650, 10, 20);
        Rectangle player2Middle = new Rectangle(450, 650, 10, 20);

        Rectangle player1LeftBoost = new Rectangle(145, 690, 5, 10);
        Rectangle player1RightBoost = new Rectangle(161, 690, 5, 10);

        Rectangle player2LeftBoost = new Rectangle(445, 690, 5, 10);
        Rectangle player2RightBoost = new Rectangle(461, 690, 5, 10);

        //declaring a randomizer
        Random randGen = new Random();

        //declaring variables
        int player1Speed = 6;
        int player2Speed = 6;
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
        SolidBrush purpleBrush = new SolidBrush(Color.Purple);
        SolidBrush greenBrush = new SolidBrush(Color.Green);
        Pen whitePen = new Pen(Color.White, 4);

        //creating sound effetcs
        SoundPlayer scoreSound = new SoundPlayer(Properties.Resources.scoreSound);
        SoundPlayer explosionSound = new SoundPlayer(Properties.Resources.explosionSound);

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

            randValue = randGen.Next(0, this.Height - 70);
            asteroidSpeedsRight.Add(randGen.Next(3, 10));

            Rectangle asteroid = new Rectangle(600, randValue, asteroidWidth, asteroidHeight);
            asteroidListRight.Add(asteroid);

            randValue = randGen.Next(0, this.Height - 70);

            Rectangle special = new Rectangle(600, randValue, asteroidWidth, asteroidHeight);
            asteroidSpecialRight.Add(special);

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
                player1.Y -= player1Speed;
                player1Middle.Y -= player1Speed;
                player1LeftBoost.Y -= player1Speed;
                player1RightBoost.Y -= player1Speed;
            }
            if (sDown == true && player1.Y < this.Height - player1.Height)
            {
                player1.Y += player1Speed;
                player1Middle.Y += player1Speed;
                player1LeftBoost.Y += player1Speed;
                player1RightBoost.Y += player1Speed;
            }

            //move player 2
            if (upDown == true)
            {
                player2.Y -= player2Speed;
                player2Middle.Y -= player2Speed;
                player2LeftBoost.Y -= player2Speed;
                player2RightBoost.Y -= player2Speed;
            }
            if (downDown == true && player2.Y < this.Height - player2.Height)
            {
                player2.Y += player2Speed;
                player2Middle.Y += player2Speed;
                player2LeftBoost.Y += player2Speed;
                player2RightBoost.Y += player2Speed;
            }

            //create asteroids on left side
            randValue = randGen.Next(1, 101);
            if (randValue <= 12)
            {
                randValue = randGen.Next(0, this.Height - 70);
                asteroidSpeedsLeft.Add(randGen.Next(3, 10));

                Rectangle ball = new Rectangle(0, randValue, asteroidWidth, asteroidHeight);
                asteroidListLeft.Add(ball);
            }

            //create left side special asteroids
            else if (randValue > 95)
            {
                randValue = randGen.Next(0, this.Height - 70);

                Rectangle asteroid = new Rectangle(0, randValue, asteroidWidth, asteroidHeight);
                asteroidSpecialLeft.Add(asteroid);
            }

            //move left side asteroids
            for (int i = 0; i < asteroidListLeft.Count(); i++)
            {
                int x = asteroidListLeft[i].X + asteroidSpeedsLeft[i];
                asteroidListLeft[i] = new Rectangle(x, asteroidListLeft[i].Y, asteroidWidth, asteroidHeight);

                if (asteroidListLeft[i].X > 600)
                {
                    asteroidListLeft.RemoveAt(i);
                    asteroidSpeedsLeft.RemoveAt(i);
                }
            }

            //move left side special asteroids
            for (int i = 0; i < asteroidSpecialLeft.Count(); i++)
            {
                asteroidSpeedsLeftSpecial.Add(randGen.Next(3, 10));

                int x = asteroidSpecialLeft[i].X + asteroidSpeedsLeftSpecial[i];
                asteroidSpecialLeft[i] = new Rectangle(x, asteroidSpecialLeft[i].Y, asteroidWidth, asteroidHeight);
            }

            //create asteroids on right side
            randValue = randGen.Next(1, 101);
            if (randValue <= 12)
            {
                randValue = randGen.Next(0, this.Height - 70);
                asteroidSpeedsRight.Add(randGen.Next(3, 10));

                Rectangle asteroid = new Rectangle(600, randValue, asteroidWidth, asteroidHeight);
                asteroidListRight.Add(asteroid);
            }

            //create right side special asteroids
            else if (randValue > 95)
            {
                randValue = randGen.Next(0, this.Height - 70);

                Rectangle asteroid = new Rectangle(600, randValue, asteroidWidth, asteroidHeight);
                asteroidSpecialRight.Add(asteroid);
            }

            //move right side asteroids
            for (int i = 0; i < asteroidListRight.Count(); i++)
            {
                int x = asteroidListRight[i].X - asteroidSpeedsRight[i];
                asteroidListRight[i] = new Rectangle(x, asteroidListRight[i].Y, asteroidWidth, asteroidHeight);

                if (asteroidListRight[i].X < 0)
                {
                    asteroidListRight.RemoveAt(i);
                    asteroidSpeedsRight.RemoveAt(i);
                }
            }

            //move right side special asteroids
            for (int i = 0; i < asteroidSpecialRight.Count(); i++)
            {
                asteroidSpeedsRightSpecial.Add(randGen.Next(3, 10));

                int x = asteroidSpecialRight[i].X - asteroidSpeedsRightSpecial[i];
                asteroidSpecialRight[i] = new Rectangle(x, asteroidSpecialRight[i].Y, asteroidWidth, asteroidHeight);
            }

            //check if player 1 hit an asteroid
            for (int i = 0; i < asteroidListRight.Count(); i++)
            {
                if (player1.IntersectsWith(asteroidListRight[i]) || player1Middle.IntersectsWith(asteroidListRight[i]))
                {
                    explosionSound.Play();

                    asteroidListRight.RemoveAt(i);
                    asteroidSpeedsRight.RemoveAt(i);
                    player1.Y = 670;
                    player1Middle.Y = 650;
                    player1LeftBoost.Y = 690;
                    player1RightBoost.Y = 690;
                    player1Speed = 6;
                }
            }
            for (int i = 0; i < asteroidListLeft.Count(); i++)
            {
                if (player1.IntersectsWith(asteroidListLeft[i]) || player1Middle.IntersectsWith(asteroidListLeft[i]))
                {
                    explosionSound.Play();

                    asteroidListLeft.RemoveAt(i);
                    asteroidSpeedsLeft.RemoveAt(i);
                    player1.Y = 670;
                    player1Middle.Y = 650;
                    player1LeftBoost.Y = 690;
                    player1RightBoost.Y = 690;
                    player1Speed = 6;
                }
            }

            //checking if player 1 hit a special asteroid and giving the appropriate powerup or debuff
            for (int i = 0; i < asteroidSpecialRight.Count(); i++)
            {
                if (player1.IntersectsWith(asteroidSpecialRight[i]) || player1Middle.IntersectsWith(asteroidSpecialRight[i]))
                {
                    if (player1Speed > 12)
                    {

                    }
                    else
                    {
                        player1Speed *= 2;
                    }
                    asteroidSpecialRight.RemoveAt(i);
                    asteroidSpeedsRightSpecial.RemoveAt(i);
                }
            }
            for (int i = 0; i < asteroidSpecialLeft.Count(); i++)
            {
                if (player1.IntersectsWith(asteroidSpecialLeft[i]) || player1Middle.IntersectsWith(asteroidSpecialLeft[i]))
                {
                    if (player1Speed < 2)
                    {

                    }
                    else
                    {
                        player1Speed /= 2;
                    }
                    asteroidSpecialLeft.RemoveAt(i);
                    asteroidSpeedsLeftSpecial.RemoveAt(i);
                }
            }

            //check if player 2 hit an asteroid
            for (int i = 0; i < asteroidListRight.Count(); i++)
            {
                if (player2.IntersectsWith(asteroidListRight[i]) || player2Middle.IntersectsWith(asteroidListRight[i]))
                {
                    explosionSound.Play();

                    asteroidListRight.RemoveAt(i);
                    asteroidSpeedsRight.RemoveAt(i);
                    player2.Y = 670;
                    player2Middle.Y = 650;
                    player2LeftBoost.Y = 690;
                    player2RightBoost.Y = 690;
                    player2Speed = 6;
                }
            }
            for (int i = 0; i < asteroidListLeft.Count(); i++)
            {
                if (player2.IntersectsWith(asteroidListLeft[i]) || player2Middle.IntersectsWith(asteroidListLeft[i]))
                {
                    explosionSound.Play();

                    asteroidListLeft.RemoveAt(i);
                    asteroidSpeedsLeft.RemoveAt(i);
                    player2.Y = 670;
                    player2Middle.Y = 650;
                    player2LeftBoost.Y = 690;
                    player2RightBoost.Y = 690;
                    player2Speed = 6;
                }
            }

            //checking if player 2 hit a special asteroid and giving the appropriate powerup or debuff
            for (int i = 0; i < asteroidSpecialRight.Count(); i++)
            {
                if (player2.IntersectsWith(asteroidSpecialRight[i]) || player2Middle.IntersectsWith(asteroidSpecialRight[i]))
                {
                    if (player2Speed > 12)
                    {

                    }
                    else
                    {
                        player2Speed *= 2;
                    }
                    asteroidSpecialRight.RemoveAt(i);
                    asteroidSpeedsRightSpecial.RemoveAt(i);
                }
            }
            for (int i = 0; i < asteroidSpecialLeft.Count(); i++)
            {
                if (player2.IntersectsWith(asteroidSpecialLeft[i]) || player2Middle.IntersectsWith(asteroidSpecialLeft[i]))
                {
                    if (player2Speed < 2)
                    {

                    }
                    else
                    {
                        player2Speed /= 2;
                    }
                    asteroidSpecialLeft.RemoveAt(i);
                    asteroidSpeedsLeftSpecial.RemoveAt(i);
                }
            }
            //checking if a player has scored
            if (player1.Y < 0)
            {
                scoreSound.Play();

                player1Score++;

                player1.Y = 670;
                player1Middle.Y = 650;
                player1LeftBoost.Y = 690;
                player1RightBoost.Y = 690;

                player1Speed = 6;

                //sleep beacuse otherwise the score never shows 3 but the game ends
                Refresh();
                Thread.Sleep(5);
            }
            if (player2.Y < 0)
            {
                scoreSound.Play();

                player2Score++;

                player2.Y = 670;
                player2Middle.Y = 650;
                player2LeftBoost.Y = 690;
                player2RightBoost.Y = 690;

                player2Speed = 6;

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
                e.Graphics.DrawRectangle(whitePen, player1Middle);
                e.Graphics.DrawRectangle(whitePen, player2Middle);

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

                //draw left side special asteroids
                for (int i = 0; i < asteroidSpecialLeft.Count(); i++)
                {
                    e.Graphics.FillRectangle(greenBrush, asteroidSpecialLeft[i]);
                }

                //draw right asteroids
                for (int i = 0; i < asteroidListRight.Count(); i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, asteroidListRight[i]);
                }

                //draw right side special asteroids
                for (int i = 0; i < asteroidSpecialRight.Count(); i++)
                {
                    e.Graphics.FillRectangle(purpleBrush, asteroidSpecialRight[i]);
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