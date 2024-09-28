namespace SimpleWinFormsApp
{
    partial class MainForm
    {
        private Button btnPhotoUpload;
        private Button btnCalculateVector;
        private Button btnTrain; 
        private Button btnDrawing;
        private PictureBox pictureBox;
        private TextBox txtBlackPixelCount;

        private void InitializeComponent()
{
    this.btnPhotoUpload = new System.Windows.Forms.Button();
    this.btnCalculateVector = new System.Windows.Forms.Button();
    this.btnTrain = new System.Windows.Forms.Button(); 
    this.btnDrawing = new System.Windows.Forms.Button(); 
    this.pictureBox = new System.Windows.Forms.PictureBox();
    this.txtBlackPixelCount = new System.Windows.Forms.TextBox();

    ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
    this.SuspendLayout();

    // 
    // btnPhotoUpload
    // 
    this.btnPhotoUpload.Location = new System.Drawing.Point(520, 50);
    this.btnPhotoUpload.Name = "btnPhotoUpload";
    this.btnPhotoUpload.Size = new System.Drawing.Size(350, 40);
    this.btnPhotoUpload.TabIndex = 0;
    this.btnPhotoUpload.Text = "Завантажити Фото";
    this.btnPhotoUpload.UseVisualStyleBackColor = true;
    this.btnPhotoUpload.Click += new System.EventHandler(this.btnLoadImage_Click);

    // 
    // btnCalculateVector
    // 
    this.btnCalculateVector.Location = new System.Drawing.Point(520, 140);
    this.btnCalculateVector.Name = "btnCalculateVector";
    this.btnCalculateVector.Size = new System.Drawing.Size(400, 40);
    this.btnCalculateVector.TabIndex = 1;
    this.btnCalculateVector.Text = "Обчислення";
    this.btnCalculateVector.UseVisualStyleBackColor = true;
    this.btnCalculateVector.Click += new System.EventHandler(this.BtnTrain_Click); // Corrected

    // 
    // btnTrain
    // 
    this.btnTrain.Location = new System.Drawing.Point(520, 230);
    this.btnTrain.Name = "btnTrain"; // Зміна імені кнопки
    this.btnTrain.Size = new System.Drawing.Size(400, 40);
    this.btnTrain.TabIndex = 2; // Зміна TabIndex
    this.btnTrain.Text = "Визначити Клас";
    this.btnTrain.UseVisualStyleBackColor = true;
    this.btnTrain.Click += new System.EventHandler(this.BtnCalculateVector_Click); // Corrected

    // 
    // btnDrawing
    // 
    this.btnDrawing.Location = new System.Drawing.Point(880, 50); // Changed position to avoid overlap
    this.btnDrawing.Name = "btnDrawing"; // Changed name for uniqueness
    this.btnDrawing.Size = new System.Drawing.Size(40, 40);
    this.btnDrawing.TabIndex = 3; // Зміна TabIndex
    this.btnDrawing.Text = " "; // Changed button text for clarity
    this.btnDrawing.UseVisualStyleBackColor = true;
    this.btnDrawing.Click += new System.EventHandler(this.BtnDrawing_Click);

    // 
    // pictureBox
    // 
    this.pictureBox.Location = new System.Drawing.Point(20, 50);
    this.pictureBox.Size = new System.Drawing.Size(400, 500);
    this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
    this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

    // 
    // txtBlackPixelCount
    // 
    this.txtBlackPixelCount.Location = new System.Drawing.Point(520, 350); // Adjusted position for better layout
    this.txtBlackPixelCount.Name = "txtBlackPixelCount";
    this.txtBlackPixelCount.Multiline = true;
    this.txtBlackPixelCount.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
    this.txtBlackPixelCount.Size = new System.Drawing.Size(400, 200);

    // 
    // MainForm
    // 
    this.ClientSize = new System.Drawing.Size(1000, 590);
    this.Controls.Add(this.btnPhotoUpload);
    this.Controls.Add(this.btnCalculateVector);
    this.Controls.Add(this.btnTrain); // Додано до контролів
    this.Controls.Add(this.btnDrawing); // Added this line
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
