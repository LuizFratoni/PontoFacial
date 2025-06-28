
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PontoFacial.App;

public partial class RegisterForm
{
    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool disposing) { if (disposing && (components != null)) { components.Dispose(); } base.Dispose(disposing); }

    private void InitializeComponent()
    {
        this.label1 = new System.Windows.Forms.Label();
        this.txtUserId = new System.Windows.Forms.TextBox();
        this.txtUserName = new System.Windows.Forms.TextBox();
        this.label2 = new System.Windows.Forms.Label();
        this.picCapturedFace = new System.Windows.Forms.PictureBox();
        this.btnCapturePhoto = new System.Windows.Forms.Button();
        this.btnRegister = new System.Windows.Forms.Button();
        this.lblStatus = new System.Windows.Forms.Label();
        this.picWebcamLive = new System.Windows.Forms.PictureBox();
        this.btnToggleCamera = new System.Windows.Forms.Button();
        this.label3 = new System.Windows.Forms.Label();
        ((System.ComponentModel.ISupportInitialize)(this.picCapturedFace)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.picWebcamLive)).BeginInit();
        this.SuspendLayout();
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label1.Location = new System.Drawing.Point(354, 25);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(89, 17);
        this.label1.TabIndex = 0;
        this.label1.Text = "ID (Matrícula):";
        // 
        // txtUserId
        // 
        this.txtUserId.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtUserId.Location = new System.Drawing.Point(456, 22);
        this.txtUserId.Name = "txtUserId";
        this.txtUserId.Size = new System.Drawing.Size(216, 25);
        this.txtUserId.TabIndex = 1;
        // 
        // txtUserName
        // 
        this.txtUserName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtUserName.Location = new System.Drawing.Point(456, 60);
        this.txtUserName.Name = "txtUserName";
        this.txtUserName.Size = new System.Drawing.Size(216, 25);
        this.txtUserName.TabIndex = 3;
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label2.Location = new System.Drawing.Point(354, 63);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(100, 17);
        this.label2.TabIndex = 2;
        this.label2.Text = "Nome Completo:";
        // 
        // picCapturedFace
        // 
        this.picCapturedFace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.picCapturedFace.Location = new System.Drawing.Point(456, 119);
        this.picCapturedFace.Name = "picCapturedFace";
        this.picCapturedFace.Size = new System.Drawing.Size(216, 170);
        this.picCapturedFace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.picCapturedFace.TabIndex = 4;
        this.picCapturedFace.TabStop = false;
        // 
        // btnCapturePhoto
        // 
        this.btnCapturePhoto.Enabled = false;
        this.btnCapturePhoto.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnCapturePhoto.Location = new System.Drawing.Point(179, 295);
        this.btnCapturePhoto.Name = "btnCapturePhoto";
        this.btnCapturePhoto.Size = new System.Drawing.Size(149, 35);
        this.btnCapturePhoto.TabIndex = 5;
        this.btnCapturePhoto.Text = "Capturar Foto";
        this.btnCapturePhoto.UseVisualStyleBackColor = true;
        this.btnCapturePhoto.Click += new System.EventHandler(this.btnCapturePhoto_Click);
        // 
        // btnRegister
        // 
        this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnRegister.Location = new System.Drawing.Point(522, 295);
        this.btnRegister.Name = "btnRegister";
        this.btnRegister.Size = new System.Drawing.Size(150, 48);
        this.btnRegister.TabIndex = 6;
        this.btnRegister.Text = "Registar";
        this.btnRegister.UseVisualStyleBackColor = true;
        this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
        // 
        // lblStatus
        // 
        this.lblStatus.AutoSize = true;
        this.lblStatus.Location = new System.Drawing.Point(23, 340);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new System.Drawing.Size(161, 13);
        this.lblStatus.TabIndex = 7;
        this.lblStatus.Text = "Por favor, preencha todos os campos.";
        // 
        // picWebcamLive
        // 
        this.picWebcamLive.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.picWebcamLive.Location = new System.Drawing.Point(26, 22);
        this.picWebcamLive.Name = "picWebcamLive";
        this.picWebcamLive.Size = new System.Drawing.Size(302, 267);
        this.picWebcamLive.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.picWebcamLive.TabIndex = 8;
        this.picWebcamLive.TabStop = false;
        // 
        // btnToggleCamera
        // 
        this.btnToggleCamera.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnToggleCamera.Location = new System.Drawing.Point(26, 295);
        this.btnToggleCamera.Name = "btnToggleCamera";
        this.btnToggleCamera.Size = new System.Drawing.Size(147, 35);
        this.btnToggleCamera.TabIndex = 9;
        this.btnToggleCamera.Text = "Ligar Câmera";
        this.btnToggleCamera.UseVisualStyleBackColor = true;
        this.btnToggleCamera.Click += new System.EventHandler(this.btnToggleCamera_Click);
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label3.Location = new System.Drawing.Point(453, 99);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(99, 17);
        this.label3.TabIndex = 10;
        this.label3.Text = "Foto Capturada:";
        // 
        // RegisterForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(696, 368);
        this.Controls.Add(this.label3);
        this.Controls.Add(this.btnToggleCamera);
        this.Controls.Add(this.picWebcamLive);
        this.Controls.Add(this.lblStatus);
        this.Controls.Add(this.btnRegister);
        this.Controls.Add(this.btnCapturePhoto);
        this.Controls.Add(this.picCapturedFace);
        this.Controls.Add(this.txtUserName);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.txtUserId);
        this.Controls.Add(this.label1);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "RegisterForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Registar Novo Utilizador via Webcam";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RegisterForm_FormClosing);
        ((System.ComponentModel.ISupportInitialize)(this.picCapturedFace)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.picWebcamLive)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtUserId;
    private System.Windows.Forms.TextBox txtUserName;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.PictureBox picCapturedFace;
    private System.Windows.Forms.Button btnCapturePhoto;
    private System.Windows.Forms.Button btnRegister;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.PictureBox picWebcamLive;
    private System.Windows.Forms.Button btnToggleCamera;
    private System.Windows.Forms.Label label3;
}