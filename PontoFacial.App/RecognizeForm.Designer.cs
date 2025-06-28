namespace PontoFacial.App;
public partial class RecognizeForm
{
    private System.ComponentModel.IContainer components = null;
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
        this.picWebcam = new System.Windows.Forms.PictureBox();
        this.lblStatus = new System.Windows.Forms.Label();
        ((System.ComponentModel.ISupportInitialize)(this.picWebcam)).BeginInit();
        this.SuspendLayout();
        // 
        // picWebcam
        // 
        this.picWebcam.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.picWebcam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.picWebcam.Location = new System.Drawing.Point(12, 12);
        this.picWebcam.Name = "picWebcam";
        this.picWebcam.Size = new System.Drawing.Size(760, 430);
        this.picWebcam.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.picWebcam.TabIndex = 0;
        this.picWebcam.TabStop = false;
        // 
        // lblStatus
        // 
        this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.lblStatus.AutoSize = true;
        this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblStatus.Location = new System.Drawing.Point(12, 468);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new System.Drawing.Size(49, 15);
        this.lblStatus.TabIndex = 2;
        this.lblStatus.Text = "Pronto.";
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(784, 511);
        this.Controls.Add(this.lblStatus);
        this.Controls.Add(this.picWebcam);
        this.MinimumSize = new System.Drawing.Size(600, 400);
        this.Name = "Form1";
        this.Text = "Cliente de Reconhecimento Facial";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
        this.Load += new System.EventHandler(this.Form1_Load);
        ((System.ComponentModel.ISupportInitialize)(this.picWebcam)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }
    private System.Windows.Forms.PictureBox picWebcam;
    private System.Windows.Forms.Label lblStatus;
}
