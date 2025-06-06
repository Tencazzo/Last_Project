using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project3.Forms
{
    partial class ResultForm
    {
        private System.ComponentModel.IContainer? components = null;
        private Label? resultLabel;
        private Button? repeatGameButton;
        private Button? ratingButton;

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
            this.resultLabel = new Label();
            this.repeatGameButton = new Button();
            this.ratingButton = new Button();
            this.SuspendLayout();

            // Form
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(123, 104, 238);
            this.ClientSize = new Size(1022, 681);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ResultForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Результат игры - Пятая форма";

            // resultLabel
            this.resultLabel.AutoSize = true;
            this.resultLabel.Font = new Font("Microsoft Sans Serif", 48F, FontStyle.Bold);
            this.resultLabel.ForeColor = Color.White;
            this.resultLabel.Location = new Point(296, 200);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new Size(430, 91);
            this.resultLabel.TabIndex = 0;
            this.resultLabel.Text = "Вы выиграли!";
            this.resultLabel.TextAlign = ContentAlignment.MiddleCenter;

            // repeatGameButton
            this.repeatGameButton.BackColor = Color.White;
            this.repeatGameButton.Font = new Font("Microsoft Sans Serif", 16F);
            this.repeatGameButton.Location = new Point(266, 370);
            this.repeatGameButton.Name = "repeatGameButton";
            this.repeatGameButton.Size = new Size(220, 60);
            this.repeatGameButton.TabIndex = 1;
            this.repeatGameButton.Text = "Повторная игра";
            this.repeatGameButton.UseVisualStyleBackColor = false;

            // ratingButton
            this.ratingButton.BackColor = Color.White;
            this.ratingButton.Font = new Font("Microsoft Sans Serif", 16F);
            this.ratingButton.Location = new Point(536, 370);
            this.ratingButton.Name = "ratingButton";
            this.ratingButton.Size = new Size(220, 60);
            this.ratingButton.TabIndex = 2;
            this.ratingButton.Text = "Рейтинг";
            this.ratingButton.UseVisualStyleBackColor = false;

            this.Controls.Add(this.ratingButton);
            this.Controls.Add(this.repeatGameButton);
            this.Controls.Add(this.resultLabel);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
