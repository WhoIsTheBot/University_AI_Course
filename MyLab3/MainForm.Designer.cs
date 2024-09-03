namespace SimpleWinFormsApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnPhotoUpload;
        private Button btnCalculateVector;
        private PictureBox pictureBox;
        private TextBox txtBlackPixelCount;

        private void InitializeComponent()
        {
            this.btnPhotoUpload = new System.Windows.Forms.Button();
            this.btnCalculateVector = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.txtBlackPixelCount = new System.Windows.Forms.TextBox();

            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();

            // btnPhotoUpload
            this.btnPhotoUpload.Location = new System.Drawing.Point(550, 50);
            this.btnPhotoUpload.Name = "btnPhotoUpload";
            this.btnPhotoUpload.Size = new System.Drawing.Size(300, 40);
            this.btnPhotoUpload.TabIndex = 0;
            this.btnPhotoUpload.Text = "Photo Upload";
            this.btnPhotoUpload.UseVisualStyleBackColor = true;
            this.btnPhotoUpload.Click += new System.EventHandler(this.btnLoadImage_Click);

            // btnCalculateVector
            this.btnCalculateVector.Location = new System.Drawing.Point(550, 140);
            this.btnCalculateVector.Name = "btnCalculateVector";
            this.btnCalculateVector.Size = new System.Drawing.Size(300, 40);
            this.btnCalculateVector.TabIndex = 1;
            this.btnCalculateVector.Text = "Calculate Vector";
            this.btnCalculateVector.UseVisualStyleBackColor = true;
            this.btnCalculateVector.Click += new System.EventHandler(this.BtnCalculateVector_Click);

            // pictureBox
            this.pictureBox.Location = new System.Drawing.Point(20, 50);
            this.pictureBox.Size = new System.Drawing.Size(400, 500);
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

            // txtBlackPixelCount
            this.txtBlackPixelCount.Location = new System.Drawing.Point(550, 200);
            this.txtBlackPixelCount.Name = "txtBlackPixelCount";
            this.txtBlackPixelCount.Multiline = true;
            this.txtBlackPixelCount.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBlackPixelCount.Size = new System.Drawing.Size(400, 200);

            // MainForm
            this.ClientSize = new System.Drawing.Size(1000, 590);
            this.Controls.Add(this.btnPhotoUpload);
            this.Controls.Add(this.btnCalculateVector);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.txtBlackPixelCount);
            this.Name = "MainForm";
            this.Text = "AI Photo";

            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
