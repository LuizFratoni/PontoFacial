using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore; 
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using FaceRecognitionDotNet;
using System.Drawing;

namespace PontoFacial.Api;

public class FaceRecognitionService
{
    private FaceRecognition _faceRecognition;

    private FaceSet faces;

    // A memória cache agora armazena o nome e o array de doubles (o encoding bruto).

    public FaceRecognitionService()
    {
        faces = FaceSet.Instance;
        var modelsDirectory = Directory.GetCurrentDirectory();
        _faceRecognition = FaceRecognition.Create(modelsDirectory);
    }


    public async Task<PersonIdentity> RegisterNewPerson(string userId, string name, byte[] imageBytes)
    {
        Console.WriteLine($"Tentando registar novo utilizador: {name} (ID: {userId})");


        using var ms = new MemoryStream(imageBytes);
        using var bitmap = new Bitmap(ms);
        var image = FaceRecognition.LoadImage(bitmap);

        var encodings = _faceRecognition.FaceEncodings(image).ToArray();

        if (encodings.Length != 1)
        {
            Console.WriteLine($"A imagem deve conter exatamente um rosto. Foram encontrados {encodings.Length}");
            return null;
        }

        var encoding = encodings.First();
        var rawEncoding = encoding.GetRawEncoding().ToArray();
        var encodingJson = JsonSerializer.Serialize(rawEncoding);

        return await faces.RegisterNewPerson(userId, name, encodingJson, rawEncoding);
    }

    public PersonIdentity IdentifyFace(FaceRecognitionDotNet.Image unknownImage)
    {
        Console.WriteLine("-- Verificando face");
        if (!faces.KnownFaces.Any()) return null;

        var unknownEncodings = _faceRecognition.FaceEncodings(unknownImage).ToArray();
        if (unknownEncodings.Length == 0) return null;

        var unknownRawEncoding = unknownEncodings.First().GetRawEncoding().ToArray();

        double bestDistance = 1.0;
        PersonIdentity bestMatch = null;

        foreach (var knownFace in faces.KnownFaces)
        {
            // CORREÇÃO: Usamos um cálculo manual da distância em vez de FaceRecognition.FaceDistance
                Console.WriteLine("** Calculando Distancia");
            var distance = CalculateDistance(knownFace.Value.EncodingData, unknownRawEncoding);
            if (distance < bestDistance)
            {

                bestDistance = distance;
                bestMatch = knownFace.Value;
            }
        }

        double tolerance = 0.55;
        if (bestDistance <= tolerance)
        {
             Console.WriteLine("** Pessoa encontrada");
            return bestMatch;
        }

        return null;
    }

    // NOVO MÉTODO PRIVADO PARA CALCULAR A DISTÂNCIA
    private double CalculateDistance(double[] v1, double[] v2)
    {
        if (v1 == null || v2 == null || v1.Length != v2.Length)
        {
            // Idealmente, isto nunca deveria acontecer com a lógica atual.
            return double.MaxValue;
        }

        double sum = 0;
        for (int i = 0; i < v1.Length; i++)
        {
            sum += Math.Pow(v1[i] - v2[i], 2);
        }
        return Math.Sqrt(sum);
    }
}
