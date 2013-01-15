﻿namespace Launcher
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonSinglePlayer = new System.Windows.Forms.Button();
            this.buttonAlgorithm1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonAlgorithm2 = new System.Windows.Forms.Button();
            this.buttonCredits = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSinglePlayer
            // 
            this.buttonSinglePlayer.Location = new System.Drawing.Point(29, 94);
            this.buttonSinglePlayer.Name = "buttonSinglePlayer";
            this.buttonSinglePlayer.Size = new System.Drawing.Size(155, 23);
            this.buttonSinglePlayer.TabIndex = 0;
            this.buttonSinglePlayer.Text = "Single Player";
            this.buttonSinglePlayer.UseVisualStyleBackColor = true;
            this.buttonSinglePlayer.Click += new System.EventHandler(this.buttonSinglePlayer_Click);
            // 
            // buttonAlgorithm1
            // 
            this.buttonAlgorithm1.Location = new System.Drawing.Point(29, 123);
            this.buttonAlgorithm1.Name = "buttonAlgorithm1";
            this.buttonAlgorithm1.Size = new System.Drawing.Size(155, 23);
            this.buttonAlgorithm1.TabIndex = 0;
            this.buttonAlgorithm1.Text = "Genetic Algorithm 1";
            this.buttonAlgorithm1.UseVisualStyleBackColor = true;
            this.buttonAlgorithm1.Click += new System.EventHandler(this.buttonAlgorithm1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(46, 44);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select Level";
            // 
            // buttonAlgorithm2
            // 
            this.buttonAlgorithm2.Location = new System.Drawing.Point(29, 152);
            this.buttonAlgorithm2.Name = "buttonAlgorithm2";
            this.buttonAlgorithm2.Size = new System.Drawing.Size(155, 23);
            this.buttonAlgorithm2.TabIndex = 0;
            this.buttonAlgorithm2.Text = "Genetic Algorithm 2";
            this.buttonAlgorithm2.UseVisualStyleBackColor = true;
            // 
            // buttonCredits
            // 
            this.buttonCredits.Location = new System.Drawing.Point(69, 220);
            this.buttonCredits.Name = "buttonCredits";
            this.buttonCredits.Size = new System.Drawing.Size(75, 23);
            this.buttonCredits.TabIndex = 3;
            this.buttonCredits.Text = "Credits";
            this.buttonCredits.UseVisualStyleBackColor = true;
            this.buttonCredits.Click += new System.EventHandler(this.buttonCredits_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(212, 264);
            this.Controls.Add(this.buttonCredits);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.buttonAlgorithm2);
            this.Controls.Add(this.buttonAlgorithm1);
            this.Controls.Add(this.buttonSinglePlayer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(228, 302);
            this.MinimumSize = new System.Drawing.Size(228, 302);
            this.Name = "Form1";
            this.Text = "Launcher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSinglePlayer;
        private System.Windows.Forms.Button buttonAlgorithm1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonAlgorithm2;
        private System.Windows.Forms.Button buttonCredits;
    }
}

