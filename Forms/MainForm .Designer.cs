using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project3.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer? components = null;
        private ComboBox? languageComboBox;
        private Button? newGameButton;
        private Button? logoutButton;
        private Panel? leaderboardPanel;
        private Label? leaderboardTitleLabel;
        private Label? leaderboardHeaderLabel;
        private ListBox? leaderboardListBox;

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
            languageComboBox = new ComboBox();
            newGameButton = new Button();
            logoutButton = new Button();
            leaderboardPanel = new Panel();
            leaderboardListBox = new ListBox();
            leaderboardHeaderLabel = new Label();
            leaderboardTitleLabel = new Label();
            leaderboardPanel.SuspendLayout();
            SuspendLayout();
            // 
            // languageComboBox
            // 
            languageComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            languageComboBox.Font = new Font("Microsoft Sans Serif", 12F);
            languageComboBox.Location = new Point(26, 31);
            languageComboBox.Margin = new Padding(4, 5, 4, 5);
            languageComboBox.Name = "languageComboBox";
            languageComboBox.Size = new Size(174, 37);
            languageComboBox.TabIndex = 0;
            // 
            // newGameButton
            // 
            newGameButton.BackColor = Color.White;
            newGameButton.Font = new Font("Microsoft Sans Serif", 16F);
            newGameButton.Location = new Point(148, 512);
            newGameButton.Margin = new Padding(4, 5, 4, 5);
            newGameButton.Name = "newGameButton";
            newGameButton.Size = new Size(300, 94);
            newGameButton.TabIndex = 1;
            newGameButton.Text = "Новая игра";
            newGameButton.UseVisualStyleBackColor = false;
            // 
            // logoutButton
            // 
            logoutButton.BackColor = Color.White;
            logoutButton.Font = new Font("Microsoft Sans Serif", 12F);
            logoutButton.Location = new Point(45, 960);
            logoutButton.Margin = new Padding(4, 5, 4, 5);
            logoutButton.Name = "logoutButton";
            logoutButton.Size = new Size(275, 62);
            logoutButton.TabIndex = 2;
            logoutButton.Text = "Выйти из аккаунта";
            logoutButton.UseVisualStyleBackColor = false;
            // 
            // leaderboardPanel
            // 
            leaderboardPanel.BackColor = Color.White;
            leaderboardPanel.BorderStyle = BorderStyle.FixedSingle;
            leaderboardPanel.Controls.Add(leaderboardListBox);
            leaderboardPanel.Controls.Add(leaderboardHeaderLabel);
            leaderboardPanel.Controls.Add(leaderboardTitleLabel);
            leaderboardPanel.Location = new Point(595, 117);
            leaderboardPanel.Margin = new Padding(4, 5, 4, 5);
            leaderboardPanel.Name = "leaderboardPanel";
            leaderboardPanel.Size = new Size(650, 905);
            leaderboardPanel.TabIndex = 3;
            // 
            // leaderboardListBox
            // 
            leaderboardListBox.Font = new Font("Microsoft Sans Serif", 12F);
            leaderboardListBox.FormattingEnabled = true;
            leaderboardListBox.ItemHeight = 29;
            leaderboardListBox.Location = new Point(25, 188);
            leaderboardListBox.Margin = new Padding(4, 5, 4, 5);
            leaderboardListBox.Name = "leaderboardListBox";
            leaderboardListBox.Size = new Size(599, 671);
            leaderboardListBox.TabIndex = 2;
            // 
            // leaderboardHeaderLabel
            // 
            leaderboardHeaderLabel.AutoSize = true;
            leaderboardHeaderLabel.Font = new Font("Microsoft Sans Serif", 12F);
            leaderboardHeaderLabel.Location = new Point(100, 109);
            leaderboardHeaderLabel.Margin = new Padding(4, 0, 4, 0);
            leaderboardHeaderLabel.Name = "leaderboardHeaderLabel";
            leaderboardHeaderLabel.Size = new Size(462, 29);
            leaderboardHeaderLabel.TabIndex = 1;
            leaderboardHeaderLabel.Text = "Имя пользователя - количество очков";
            leaderboardHeaderLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // leaderboardTitleLabel
            // 
            leaderboardTitleLabel.AutoSize = true;
            leaderboardTitleLabel.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold);
            leaderboardTitleLabel.Location = new Point(175, 31);
            leaderboardTitleLabel.Margin = new Padding(4, 0, 4, 0);
            leaderboardTitleLabel.Name = "leaderboardTitleLabel";
            leaderboardTitleLabel.Size = new Size(277, 37);
            leaderboardTitleLabel.TabIndex = 0;
            leaderboardTitleLabel.Text = "Таблица лидеров";
            leaderboardTitleLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(123, 104, 238);
            ClientSize = new Size(1278, 1064);
            Controls.Add(leaderboardPanel);
            Controls.Add(logoutButton);
            Controls.Add(newGameButton);
            Controls.Add(languageComboBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Рейтинг и запуск игры - Третья форма";
            leaderboardPanel.ResumeLayout(false);
            leaderboardPanel.PerformLayout();
            ResumeLayout(false);
        }
    }
}
