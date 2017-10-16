namespace Test
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button2 = new System.Windows.Forms.Button();
            this.comTxt = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.outputTxt = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.resultListView = new System.Windows.Forms.ListView();
            this.button6 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(118, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "打开端口";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comTxt
            // 
            this.comTxt.Location = new System.Drawing.Point(12, 12);
            this.comTxt.Name = "comTxt";
            this.comTxt.Size = new System.Drawing.Size(100, 21);
            this.comTxt.TabIndex = 1;
            this.comTxt.Text = "COM8";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(240, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 23);
            this.button3.TabIndex = 0;
            this.button3.Text = " 关闭端口";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(118, 50);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(94, 23);
            this.button4.TabIndex = 0;
            this.button4.Text = "读卡请求";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // outputTxt
            // 
            this.outputTxt.Location = new System.Drawing.Point(12, 50);
            this.outputTxt.Name = "outputTxt";
            this.outputTxt.Size = new System.Drawing.Size(100, 21);
            this.outputTxt.TabIndex = 1;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(240, 50);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(94, 23);
            this.button5.TabIndex = 0;
            this.button5.Text = "写卡";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // resultListView
            // 
            this.resultListView.HoverSelection = true;
            this.resultListView.Location = new System.Drawing.Point(12, 132);
            this.resultListView.Name = "resultListView";
            this.resultListView.ShowGroups = false;
            this.resultListView.Size = new System.Drawing.Size(1118, 379);
            this.resultListView.TabIndex = 4;
            this.resultListView.UseCompatibleStateImageBehavior = false;
            this.resultListView.View = System.Windows.Forms.View.List;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(351, 50);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(94, 23);
            this.button6.TabIndex = 0;
            this.button6.Text = "读卡";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1142, 523);
            this.Controls.Add(this.resultListView);
            this.Controls.Add(this.outputTxt);
            this.Controls.Add(this.comTxt);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox comTxt;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox outputTxt;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ListView resultListView;
        private System.Windows.Forms.Button button6;
    }
}

