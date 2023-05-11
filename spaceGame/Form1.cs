using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace spaceGame
{
    public partial class Form1 : Form
    {
        //creating lists
        List<Rectangle> asteroidListRight = new List<Rectangle>();
        List<Rectangle> asteroidListLeft = new List<Rectangle>();
        List<int> asteroidSpeeds = new List<int>();

        //declaring players
        Rectangle player1 = new Rectangle(50, 50, 50, 50);
        Rectangle player2 = new Rectangle(30, 30, 10, 10);

        //declaring a randomizer
        Random randGen = new Random();
        int randValue;

        //declaring variables
        int playerSpeed = 6;
        int asteroidWidth = 15;
        int asteroidHeight = 5;

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

            randValue = randGen.Next(0, this.Height - 50);
            asteroidSpeeds.Add(randGen.Next(5, 15));

            Rectangle ball = new Rectangle(0, randValue, asteroidWidth, asteroidHeight);
            asteroidListLeft.Add(ball);
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
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
                    Application.Exit();
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

            //move left side asteroids
            for (int i = 0; i < asteroidListLeft.Count(); i++)
            {
                int x = asteroidListLeft[i].X + asteroidSpeeds[i];
                asteroidListLeft[i] = new Rectangle(x, asteroidListLeft[i].Y, asteroidWidth, asteroidHeight);
            }

            //create asteroids on left side
            randValue = randGen.Next(1, 101);
            if (randValue <= 15)
            {
                randValue = randGen.Next(0, this.Height - 50);
                asteroidSpeeds.Add(randGen.Next(3, 10));

                Rectangle ball = new Rectangle(0, randValue, asteroidWidth, asteroidHeight);
                asteroidListLeft.Add(ball);
            }

            //move right side asteroids
            for (int i = 0; i < asteroidListRight.Count(); i++)
            {
                int x = asteroidListRight[i].Y - asteroidSpeeds[i];
                asteroidListRight[i] = new Rectangle(x, asteroidListRight[i].Y, asteroidWidth, asteroidHeight);
            }

            //create asteroids on right side
            randValue = randGen.Next(1, 101);
            if (randValue <= 15)
            {
                randValue = randGen.Next(600, this.Height - 50);
                asteroidSpeeds.Add(randGen.Next(3, 10));

                Rectangle ball = new Rectangle(0, randValue, asteroidWidth, asteroidHeight);
                asteroidListRight.Add(ball);
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //draw the players
            e.Graphics.FillRectangle(whiteBrush, player1);

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
        }
    }
}
