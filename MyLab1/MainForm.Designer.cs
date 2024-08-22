namespace SimpleWinFormsApp
{
    partial class MainForm
    {
        private Button btnPhotoUpload;
        private Button btnCalculateVector;
        private PictureBox pictureBox;
        TextBox txtBlackPixelCount = new TextBox();
        private Bitmap bmpImage;
        private void InitializeComponent()
        {
            this.btnPhotoUpload = new System.Windows.Forms.Button();
            this.btnCalculateVector = new System.Windows.Forms.Button();
            this.SuspendLayout();

            //btnPhotoUpload
            this.btnPhotoUpload.Location = new System.Drawing.Point(390, 50);
            this.btnPhotoUpload.Name = "btnPhotoUpload";
            this.btnPhotoUpload.Size = new System.Drawing.Size(300, 40);
            this.btnPhotoUpload.TabIndex = 0;
            this.btnPhotoUpload.Text = "Photo Upload";
            this.btnPhotoUpload.UseVisualStyleBackColor = true;
            this.btnPhotoUpload.Click += new System.EventHandler(this.btnLoadImage_Click);

            //btnСalculateVector
            this.btnCalculateVector.Location = new System.Drawing.Point(390, 140);
            this.btnCalculateVector.Name = "btnCalculateVector";
            this.btnCalculateVector.Size = new System.Drawing.Size(300, 40);
            this.btnCalculateVector.TabIndex = 1;
            this.btnCalculateVector.Text = "Calculate Vector";
            this.btnCalculateVector.UseVisualStyleBackColor = true;
            this.btnCalculateVector.Click += new System.EventHandler(this.BtnCalculateVector_Click);

            //PictureBox
            this.pictureBox = new PictureBox();
            this.pictureBox.Location = new Point(20, 50); 
            this.pictureBox.Size = new Size(300, 400); 
            this.pictureBox.BorderStyle = BorderStyle.Fixed3D; 
            this.pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBox);

            //TextBox
            this.txtBlackPixelCount.Name = "txtBlackPixelCount";
            this.txtBlackPixelCount.Multiline = true; 
            this.txtBlackPixelCount.ScrollBars = ScrollBars.Vertical; 
            this.txtBlackPixelCount.Location = new Point(350, 200); 
            this.txtBlackPixelCount.Size = new Size(400, 200); 
            this.Controls.Add(txtBlackPixelCount);

            //MainForm
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.btnPhotoUpload);
            this.Controls.Add(this.btnCalculateVector);
            this.Name = "MainForm";
            this.Text = "AI Photo";
            this.ResumeLayout(false);
        }
    }
}
