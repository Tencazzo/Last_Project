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
            resultLabel = new Label();
            repeatGameButton = new Button();
            ratingButton = new Button();
            SuspendLayout();
            // 
            // resultLabel
            // 
            resultLabel.AutoSize = true;
            resultLabel.Font = new Font("Microsoft Sans Serif", 48F, FontStyle.Bold);
            resultLabel.ForeColor = Color.White;
            resultLabel.Location = new Point(370, 312);
            resultLabel.Margin = new Padding(4, 0, 4, 0);
            resultLabel.Name = "resultLabel";
            resultLabel.Size = new Size(669, 108);
            resultLabel.TabIndex = 0;
            resultLabel.Text = "Вы выиграли!";
            resultLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // repeatGameButton
            // 
            repeatGameButton.BackColor = Color.White;
            repeatGameButton.Font = new Font("Microsoft Sans Serif", 16F);
            repeatGameButton.Location = new Point(332, 578);
            repeatGameButton.Margin = new Padding(4, 5, 4, 5);
            repeatGameButton.Name = "repeatGameButton";
            repeatGameButton.Size = new Size(275, 94);
            repeatGameButton.TabIndex = 1;
            repeatGameButton.Text = "Повторная игра";
            repeatGameButton.UseVisualStyleBackColor = false;
            // 
            // ratingButton
            // 
            ratingButton.BackColor = Color.White;
            ratingButton.Font = new Font("Microsoft Sans Serif", 16F);
            ratingButton.Location = new Point(670, 578);
            ratingButton.Margin = new Padding(4, 5, 4, 5);
            ratingButton.Name = "ratingButton";
            ratingButton.Size = new Size(275, 94);
            ratingButton.TabIndex = 2;
            ratingButton.Text = "Рейтинг";
            ratingButton.UseVisualStyleBackColor = false;
            // 
            // ResultForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.RoyalBlue;
            ClientSize = new Size(1278, 1064);
            Controls.Add(ratingButton);
            Controls.Add(repeatGameButton);
            Controls.Add(resultLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "ResultForm";
            StartPosition = FormStartPosition.CenterScreen;
            this.Text = string.Empty;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
