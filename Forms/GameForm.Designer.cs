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
            gamePanel = new Panel();
            completeButton = new Button();
            player1Label = new Label();
            player2Label = new Label();
            player1ColorPanel = new Panel();
            player2ColorPanel = new Panel();
            turnLabel = new Label();
            waitingLabel = new Label();
            SuspendLayout();
            // 
            // gamePanel
            // 
            gamePanel.BackColor = Color.FromArgb(0, 0, 139);
            gamePanel.BorderStyle = BorderStyle.Fixed3D;
            gamePanel.Location = new Point(125, 156);
            gamePanel.Margin = new Padding(4, 5, 4, 5);
            gamePanel.Name = "gamePanel";
            gamePanel.Size = new Size(1124, 1013);
            gamePanel.TabIndex = 0;
            // 
            // completeButton
            // 
            completeButton.BackColor = Color.White;
            completeButton.Font = new Font("Microsoft Sans Serif", 14F);
            completeButton.Location = new Point(1438, 1172);
            completeButton.Margin = new Padding(4, 5, 4, 5);
            completeButton.Name = "completeButton";
            completeButton.Size = new Size(225, 78);
            completeButton.TabIndex = 1;
            completeButton.Text = "Завершить";
            completeButton.UseVisualStyleBackColor = false;
            // 
            // player1Label
            // 
            player1Label.AutoSize = true;
            player1Label.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            player1Label.ForeColor = Color.White;
            player1Label.Location = new Point(250, 1203);
            player1Label.Margin = new Padding(4, 0, 4, 0);
            player1Label.Name = "player1Label";
            player1Label.Size = new Size(115, 29);
            player1Label.TabIndex = 2;
            player1Label.Text = "Игрок 1:";
            // 
            // player2Label
            // 
            player2Label.AutoSize = true;
            player2Label.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            player2Label.ForeColor = Color.White;
            player2Label.Location = new Point(562, 1203);
            player2Label.Margin = new Padding(4, 0, 4, 0);
            player2Label.Name = "player2Label";
            player2Label.Size = new Size(115, 29);
            player2Label.TabIndex = 4;
            player2Label.Text = "Игрок 2:";
            // 
            // player1ColorPanel
            // 
            player1ColorPanel.BackColor = Color.Orange;
            player1ColorPanel.BorderStyle = BorderStyle.FixedSingle;
            player1ColorPanel.Location = new Point(412, 1203);
            player1ColorPanel.Margin = new Padding(4, 5, 4, 5);
            player1ColorPanel.Name = "player1ColorPanel";
            player1ColorPanel.Size = new Size(37, 46);
            player1ColorPanel.TabIndex = 3;
            // 
            // player2ColorPanel
            // 
            player2ColorPanel.BackColor = Color.Yellow;
            player2ColorPanel.BorderStyle = BorderStyle.FixedSingle;
            player2ColorPanel.Location = new Point(725, 1203);
            player2ColorPanel.Margin = new Padding(4, 5, 4, 5);
            player2ColorPanel.Name = "player2ColorPanel";
            player2ColorPanel.Size = new Size(37, 46);
            player2ColorPanel.TabIndex = 5;
            // 
            // turnLabel
            // 
            turnLabel.AutoSize = true;
            turnLabel.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold);
            turnLabel.ForeColor = Color.White;
            turnLabel.Location = new Point(688, 78);
            turnLabel.Margin = new Padding(4, 0, 4, 0);
            turnLabel.Name = "turnLabel";
            turnLabel.Size = new Size(196, 32);
            turnLabel.TabIndex = 6;
            turnLabel.Text = "Ход игрока 1";
            turnLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // waitingLabel
            // 
            waitingLabel.BackColor = Color.Transparent;
            waitingLabel.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold);
            waitingLabel.ForeColor = Color.White;
            waitingLabel.Location = new Point(1125, 1016);
            waitingLabel.Margin = new Padding(4, 0, 4, 0);
            waitingLabel.Name = "waitingLabel";
            waitingLabel.Size = new Size(500, 62);
            waitingLabel.TabIndex = 7;
            waitingLabel.Text = "Ожидание второго игрока...";
            waitingLabel.TextAlign = ContentAlignment.MiddleCenter;
            waitingLabel.Visible = false;
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.RoyalBlue;
            ClientSize = new Size(1750, 1328);
            Controls.Add(waitingLabel);
            Controls.Add(turnLabel);
            Controls.Add(player2ColorPanel);
            Controls.Add(player2Label);
            Controls.Add(player1ColorPanel);
            Controls.Add(player1Label);
            Controls.Add(completeButton);
            Controls.Add(gamePanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "GameForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Сама игра";
            ResumeLayout(false);
            PerformLayout();
        }

        private void InitializeGameButtons()
        {
            if (gamePanel == null) return;

            gameButtons = new Button[6, 7];
            int buttonSize = 90; // Уменьшаем размер кнопок
            int spacing = 10;    // Уменьшаем промежутки
                                 // Увеличиваем отступы от краев панели
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
