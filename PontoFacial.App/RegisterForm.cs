using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace PontoFacial.App;

public partial class RegisterForm : Form
{
    // --- CONFIGURAÇÃO DA API ---
    private const string ApiBaseUrl = "http://10.10.0.13:5097"; // <-- MUDE AQUI SE NECESSÁRIO
    private const string ApiRegisterEndpoint = "/api/register";
    // -------------------------

    private static readonly HttpClient _httpClient = new HttpClient();
    private byte[] _capturedImageBytes;

    // --- COMPONENTES OPENCV ---
    private VideoCapture _capture;
    private BackgroundWorker _bgWorker;
    private Mat _currentFrame;

    public RegisterForm()
    {
        InitializeComponent();
        _currentFrame = new Mat();

        _bgWorker = new BackgroundWorker();
        _bgWorker.DoWork += BgWorker_DoWork;
        _bgWorker.WorkerSupportsCancellation = true;
    }

    private void btnToggleCamera_Click(object sender, EventArgs e)
    {
        if (_bgWorker.IsBusy)
        {
            // Desligar a câmera
            _bgWorker.CancelAsync();
            _capture?.Release();
            _capture = null;
            btnToggleCamera.Text = "Ligar Câmera";
            btnCapturePhoto.Enabled = false;
        }
        else
        {
            // Ligar a câmera
            _capture = new VideoCapture(0); // Câmera padrão
            if (!_capture.IsOpened())
            {
                MessageBox.Show("Não foi possível acessar a webcam.", "Erro de Câmera", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _bgWorker.RunWorkerAsync();
            btnToggleCamera.Text = "Desligar Câmera";
            btnCapturePhoto.Enabled = true;
        }
    }

    private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
    {
        var worker = sender as BackgroundWorker;
        while (!worker.CancellationPending)
        {
            _capture.Read(_currentFrame);
            if (_currentFrame.Empty()) continue;

            // Exibe o frame ao vivo na PictureBox da webcam
            var bmp = _currentFrame.ToBitmap();
            picWebcamLive.Invoke((MethodInvoker)delegate
            {
                picWebcamLive.Image?.Dispose();
                picWebcamLive.Image = bmp;
            });
            System.Threading.Thread.Sleep(30);
        }
    }

    private void btnCapturePhoto_Click(object sender, EventArgs e)
    {
        if (_currentFrame != null && !_currentFrame.Empty())
        {
            // Converte o frame atual para bytes e o armazena
            _capturedImageBytes = _currentFrame.ToBytes(".jpg");

            // Exibe a imagem capturada na PictureBox de pré-visualização
            picCapturedFace.Image?.Dispose();
            picCapturedFace.Image = _currentFrame.ToBitmap();

            lblStatus.Text = "Foto capturada com sucesso.";
            lblStatus.ForeColor = Color.DarkGreen;
        }
        else
        {
            MessageBox.Show("Câmera não está pronta ou nenhum frame foi capturado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void btnRegister_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtUserId.Text) || string.IsNullOrWhiteSpace(txtUserName.Text))
        {
            MessageBox.Show("Por favor, preencha os campos ID e Nome.", "Campos Obrigatórios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (_capturedImageBytes == null)
        {
            MessageBox.Show("Por favor, capture uma foto para o registo.", "Foto Obrigatória", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnRegister.Enabled = false;
        lblStatus.Text = "A registar... Por favor, aguarde.";
        lblStatus.ForeColor = Color.Blue;

        await RegisterPersonAsync();

        btnRegister.Enabled = true;
    }

    private async Task RegisterPersonAsync()
    {
        try
        {
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(txtUserId.Text), "userId");
                content.Add(new StringContent(txtUserName.Text), "userName");
                var imageContent = new ByteArrayContent(_capturedImageBytes);
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                content.Add(imageContent, "imageFile", $"{txtUserId.Text}.jpg");

                var response = await _httpClient.PostAsync(ApiBaseUrl + ApiRegisterEndpoint, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Utilizador registado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    lblStatus.Text = $"Erro da API: {responseString}";
                    lblStatus.ForeColor = Color.Red;
                    MessageBox.Show($"Ocorreu um erro no registo: {responseString}", "Erro da API", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        catch (Exception ex)
        {
            lblStatus.Text = $"Erro de conexão: {ex.Message}";
            lblStatus.ForeColor = Color.Red;
            MessageBox.Show($"Não foi possível conectar à API: {ex.Message}", "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void RegisterForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        // Garante que a câmera seja desligada ao fechar
        if (_bgWorker != null && _bgWorker.IsBusy)
        {
            _bgWorker.CancelAsync();
        }
        _capture?.Release();
        _currentFrame?.Dispose();
    }
}
