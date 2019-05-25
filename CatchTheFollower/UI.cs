using System;
using System.Windows.Forms;
using System.Drawing;

namespace CatchTheFollower
{
    public class UI : Board
    {
        private Board board;
        private Image pauseImage;
        private Image resumeImage;
        private Image restartImage;
        private Image easyImage;
        private Image mediumImage;
        private Image hardImage;
        private Image nightmareImage;
        public Panel PauseButton;
        public Panel RestartButton;
        public Panel DifficultyButton;
        public UI(Board board)
        {
            this.board = board;
            pauseImage = Image.FromFile(@"..\..\Images\pause.png");
            resumeImage = Image.FromFile(@"..\..\Images\Resume.png");
            restartImage = Image.FromFile(@"..\..\Images\restart.png");
            easyImage = Image.FromFile(@"..\..\Images\easy.png");
            mediumImage = Image.FromFile(@"..\..\Images\medium.png");
            hardImage = Image.FromFile(@"..\..\Images\hard.png");
            nightmareImage = Image.FromFile(@"..\..\Images\nightmare.png");
        }
        public void MakeButtons()
        {
            PauseButton = new Panel
            {
                BackgroundImage = pauseImage,
                Size = new Size(100, 50),
                Location = new Point(840, 10),
                Visible = true
            };
            PauseButton.Click += PauseButton_Click;
            board.Controls.Add(PauseButton);

            RestartButton = new Panel
            {
                BackgroundImage = restartImage,
                Size = new Size(100, 50),
                Location = new Point(840, 70),
                Visible = true
            };
            RestartButton.Click += RestartButton_Click;
            board.Controls.Add(RestartButton);

            DifficultyButton = new Panel
            {
                BackgroundImage = mediumImage,
                Size = new Size(100, 50),
                Location = new Point(840, 130),
                Visible = true
            };
            DifficultyButton.Click += DifficultyButton_Click;
            board.Controls.Add(DifficultyButton);
        }
        private void DifficultyButton_Click(object sender, EventArgs e)
        {
            board.Difficulty++;
            if (board.Difficulty > 3)
            {
                board.Difficulty = 0;
            }
            if (board.Difficulty == 0)
            {
                DifficultyButton.BackgroundImage = easyImage;
            }
            if (board.Difficulty == 1)
            {
                DifficultyButton.BackgroundImage = mediumImage;
            }
            if (board.Difficulty == 2)
            {
                DifficultyButton.BackgroundImage = hardImage;
            }
            if (board.Difficulty == 3)
            {
                DifficultyButton.BackgroundImage = nightmareImage;
            }
            board.Paused = false;
            board.LoadItems();
        }
        private void RestartButton_Click(object sender, EventArgs e)
        {
            board.Paused = false;
            board.LoadItems();
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            board.Paused = !board.Paused;
            if (!board.Paused)
            {
                PauseButton.BackgroundImage = pauseImage;
            }
            if (board.Paused)
            {
                PauseButton.BackgroundImage = resumeImage;
            }
        }
    }
}
