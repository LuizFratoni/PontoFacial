using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace PontoFacial.App;



public partial class RecognizeForm : Form
{
    // --- CONFIGURAÇÃO DA API ---
    private const string ApiBaseUrl = "http://10.10.0.13:5097"; 
    private const string ApiEndpoint = "/api/recognize";

    // Componentes para comunicação com a API
    private static readonly HttpClient _httpClient = new HttpClient();
    private DateTime _lastApiCall = DateTime.MinValue;
    private readonly TimeSpan _apiCallCooldown = TimeSpan.FromSeconds(3); // Enviar a cada 3 segundos


    // Componentes principais do OpenCV
    private VideoCapture _capture;
    private CascadeClassifier _faceCascade;
    private BackgroundWorker _bgWorker;

    // Caminho para o classificador Haar Cascade
    private const string CascadeFileName = "haarcascade_frontalface_default.xml";
    private const string CascadeFileUrl = "https://raw.githubusercontent.com/opencv/opencv/master/data/haarcascades/haarcascade_frontalface_default.xml";


    public RecognizeForm()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        // Inicializa o BackgroundWorker para processar o vídeo sem travar a UI
        _bgWorker = new BackgroundWorker();
        _bgWorker.DoWork += BgWorker_DoWork;
        _bgWorker.WorkerSupportsCancellation = true;

        // Garante que o arquivo de classificação de face exista
        if (!File.Exists(CascadeFileName))
        {
            lblStatus.Text = $"Baixando modelo de detecção ({CascadeFileName})...";
            try
            {
                // Tenta baixar o arquivo
                using (var client = new WebClient())
                {
                    client.DownloadFile(CascadeFileUrl, CascadeFileName);
                    lblStatus.Text = "Modelo baixado com sucesso.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao baixar o arquivo do classificador: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                lblStatus.Text = "Erro: Não foi possível obter o modelo.";
                return;
            }

        }

        // Carrega o classificador de faces
        _faceCascade = new CascadeClassifier(CascadeFileName);
        if (_faceCascade.Empty())
        {
            MessageBox.Show("Não foi possível carregar o classificador de faces.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblStatus.Text = "Erro: Classificador inválido.";
        }

        iniciaCamera();
    }

    private void iniciaCamera()
    {
        if (_bgWorker.IsBusy)
        {
            // Para a câmera
            _bgWorker.CancelAsync();
            lblStatus.Text = "Câmera parada.";
        }
        else
        {
            // Inicia a câmera
            _capture = new VideoCapture(0); // 0 para a câmera padrão
            if (!_capture.IsOpened())
            {
                MessageBox.Show("Não foi possível acessar a webcam.", "Erro de Câmera", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _bgWorker.RunWorkerAsync();

            lblStatus.Text = "Câmera em execução...";
        }
    }

    private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
    {
        var worker = sender as BackgroundWorker;

        while (!worker.CancellationPending)
        {
            using (var frameMat = new Mat())
            {
                _capture.Read(frameMat);
                if (frameMat.Empty()) break;

                using (var grayMat = new Mat())
                {
                    Cv2.CvtColor(frameMat, grayMat, ColorConversionCodes.BGR2GRAY);
                    Cv2.EqualizeHist(grayMat, grayMat);

                    Rect[] faces = _faceCascade.DetectMultiScale(
                        grayMat, 1.1, 5, HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(60, 60));

                    if (faces.Length > 0)
                    {
                        // Desenha o retângulo na primeira face encontrada
                        var faceRect = faces[0];
                        Cv2.Rectangle(frameMat, faceRect, Scalar.LimeGreen, 2);
                        
                        // Lógica de Cooldown: verifica se já passou tempo suficiente desde o último envio
                        if (DateTime.UtcNow - _lastApiCall > _apiCallCooldown)
                        {
                            _lastApiCall = DateTime.UtcNow; // Atualiza o tempo do último envio

                            // Recorta a imagem do rosto
                            using (var faceImage = new Mat(frameMat, faceRect))
                            {
                                // Converte para bytes
                                byte[] imageBytes = faceImage.ToBytes(".jpg");

                                // Inicia a tarefa de envio para a API sem bloquear o loop de vídeo
                                Task.Run(() => SendFaceToApiAsync(imageBytes));
                            }
                        }
                    }
                    else
                    {
                        UpdateStatusLabel("Procurando por rostos...");
                    }
                }

                // Exibe o frame na tela
                var bmp = frameMat.ToBitmap();
                picWebcam.Invoke((MethodInvoker)delegate
                {
                    picWebcam.Image?.Dispose();
                    picWebcam.Image = bmp;
                });
            }
            System.Threading.Thread.Sleep(30);
        }
    }

    private async Task SendFaceToApiAsync(byte[] imageBytes)
    {
        try
        {
            UpdateStatusLabel("Enviando rosto para a API...");

            // Cria o conteúdo da requisição HTTP como 'multipart/form-data'
            using (var content = new MultipartFormDataContent())
            {
                var imageContent = new ByteArrayContent(imageBytes);
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                
                // O nome 'imageFile' deve ser o mesmo esperado pelo parâmetro na API
                content.Add(imageContent, "imageFile", "face.jpg");

                // Envia a requisição POST
                var response = await _httpClient.PostAsync(ApiBaseUrl + ApiEndpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
  
                    // Aqui podemos deserializar a resposta se quisermos, por enquanto, apenas exibimos uma mensagem.
                    UpdateStatusLabel("API respondeu: Rosto recebido!");
                }
                else
                {
                    UpdateStatusLabel($"Erro da API: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            UpdateStatusLabel($"Erro de conexão: {ex.Message.Substring(0, 30)}...");
        }
    }
    
      // Método auxiliar para atualizar a label de status de forma segura entre threads
    private void UpdateStatusLabel(string text)
    {
        if (lblStatus.InvokeRequired)
        {
            lblStatus.Invoke((MethodInvoker)delegate { lblStatus.Text = text; });
        }
        else
        {
            lblStatus.Text = text;
        }
    }

    
    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        // Garante que a câmera seja liberada ao fechar o formulário
        if (_bgWorker != null && _bgWorker.IsBusy)
        {
            _bgWorker.CancelAsync();
        }
        _capture?.Release();
        _faceCascade?.Dispose();
    }
}
