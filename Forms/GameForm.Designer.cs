using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project3.Forms
{
    partial class GameForm
    {
        private System.ComponentModel.IContainer? components = null;
        private Panel? gamePanel;
        private Button? completeButton;
        private Button[,]? gameButtons;
        private Label? player1Label;
        private Label? player2Label;
        private Panel? player1ColorPanel;
        private Panel? player2ColorPanel;
        private Label? turnLabel;
        private Label? waitingLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.gamePanel = new Panel();
            this.completeButton = new Button();
            this.player1Label = new Label();
            this.player2Label = new Label();
            this.player1ColorPanel = new Panel();
            this.player2ColorPanel = new Panel();
            this.turnLabel = new Label();
            this.waitingLabel = new Label();

            // Form
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.RoyalBlue;
            this.ClientSize = new Size(1400, 850);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "GameForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = string.Empty;
            this.gamePanel.BackColor = Color.FromArgb(0, 0, 139);
            this.gamePanel.BorderStyle = BorderStyle.Fixed3D;
            this.gamePanel.Location = new Point(100, 100);
            this.gamePanel.Name = "gamePanel";
            this.gamePanel.Size = new Size(900, 650);
            this.gamePanel.TabIndex = 0;
            /*
            // waitingLabel
            this.waitingLabel.AutoSize = false;
            this.waitingLabel.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold);
            this.waitingLabel.ForeColor = Color.White;
            this.waitingLabel.BackColor = Color.Transparent;
            this.waitingLabel.Size = new Size(400, 40);
            this.waitingLabel.Location = new Point(
                (this.gamePanel.Width - 400) / 2,
                (this.gamePanel.Height - 40) / 2);
            this.waitingLabel.Name = "waitingLabel";
            this.waitingLabel.TabIndex = 7;
            this.waitingLabel.Text = string.Empty;
            this.waitingLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.waitingLabel.Visible = false;
            */
            // completeButton
            this.completeButton.BackColor = Color.White;
            this.completeButton.Font = new Font("Microsoft Sans Serif", 14F);
            this.completeButton.Location = new Point(1150, 750);
            this.completeButton.Name = "completeButton";
            this.completeButton.Size = new Size(180, 50);
            this.completeButton.TabIndex = 1;
            this.completeButton.Text = "Завершить";
            this.completeButton.UseVisualStyleBackColor = false;

            // player1Label
            this.player1Label.AutoSize = true;
            this.player1Label.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            this.player1Label.ForeColor = Color.White;
            this.player1Label.Location = new Point(200, 770);
            this.player1Label.Name = "player1Label";
            this.player1Label.Size = new Size(120, 25);
            this.player1Label.TabIndex = 2;
            this.player1Label.Text = "Игрок 1:";

            // player1ColorPanel
            this.player1ColorPanel.BackColor = Color.Orange;
            this.player1ColorPanel.BorderStyle = BorderStyle.FixedSingle;
            this.player1ColorPanel.Location = new Point(330, 770);
            this.player1ColorPanel.Name = "player1ColorPanel";
            this.player1ColorPanel.Size = new Size(30, 30);
            this.player1ColorPanel.TabIndex = 3;

            // player2Label
            this.player2Label.AutoSize = true;
            this.player2Label.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            this.player2Label.ForeColor = Color.White;
            this.player2Label.Location = new Point(450, 770);
            this.player2Label.Name = "player2Label";
            this.player2Label.Size = new Size(120, 25);
            this.player2Label.TabIndex = 4;
            this.player2Label.Text = "Игрок 2:";

            // player2ColorPanel
            this.player2ColorPanel.BackColor = Color.Yellow;
            this.player2ColorPanel.BorderStyle = BorderStyle.FixedSingle;
            this.player2ColorPanel.Location = new Point(580, 770);
            this.player2ColorPanel.Name = "player2ColorPanel";
            this.player2ColorPanel.Size = new Size(30, 30);
            this.player2ColorPanel.TabIndex = 5;

            // turnLabel
            this.turnLabel.AutoSize = true;
            this.turnLabel.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold);
            this.turnLabel.ForeColor = Color.White;
            this.turnLabel.Location = new Point(550, 50);
            this.turnLabel.Name = "turnLabel";
            this.turnLabel.Size = new Size(300, 29);
            this.turnLabel.TabIndex = 6;
            this.turnLabel.Text = string.Empty;
            this.turnLabel.TextAlign = ContentAlignment.MiddleCenter;

            this.Controls.Add(this.waitingLabel);
            this.Controls.Add(this.turnLabel);
            this.Controls.Add(this.player2ColorPanel);
            this.Controls.Add(this.player2Label);
            this.Controls.Add(this.player1ColorPanel);
            this.Controls.Add(this.player1Label);
            this.Controls.Add(this.completeButton);
            this.Controls.Add(this.gamePanel);

            InitializeGameButtons();
        }

        private void InitializeGameButtons()
        {
            if (gamePanel == null) return;

            gameButtons = new Button[6, 7];
            int buttonSize = 90;
            int spacing = 10;
            int startX = (gamePanel.Width - (7 * buttonSize + 6 * spacing)) / 2;
            int startY = (gamePanel.Height - (6 * buttonSize + 5 * spacing)) / 2;

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    gameButtons[row, col] = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(
                            startX + col * (buttonSize + spacing),
                            startY + row * (buttonSize + spacing)),
                        BackColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Tag = col,
                        Enabled = false
                    };

                    gameButtons[row, col].FlatAppearance.BorderSize = 0;
                    gameButtons[row, col].Click += OnGameButtonClick;

                    gamePanel.Controls.Add(gameButtons[row, col]);
                }
            }
        }
    }
}