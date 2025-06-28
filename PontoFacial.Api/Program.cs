

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using PontoFacial.Api;
using System.Drawing; //
using FaceRecognitionDotNet;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});



builder.Services.AddDbContextFactory<FaceDbContext>();
builder.Services.AddScoped<FaceRecognitionService>();

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);

using (var scope = app.Services.CreateScope())
{
    var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<FaceDbContext>>();
    using (var dbContext = dbContextFactory.CreateDbContext())
    {
        dbContext.Database.EnsureCreated();
    }
}

// Configuração para um endpoint de teste na raiz da API.
app.MapGet("/", () => "A API de Reconhecimento Facial está online!");

app.MapPost("/api/register", async (
    [FromForm] string userId,
    [FromForm] string userName,
    IFormFile imageFile,
    FaceRecognitionService recognitionService) =>
{
    if (imageFile == null || imageFile.Length == 0 || string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(userName))
    {
        return Results.BadRequest(new { message = "ID, Nome e um ficheiro de imagem são obrigatórios." });
    }

    Console.WriteLine("Regisrando nova pessoa:");
    using var ms = new MemoryStream();
    await imageFile.CopyToAsync(ms);
    var imageBytes = ms.ToArray();

    var result = await recognitionService.RegisterNewPerson(userId, userName, imageBytes);

    return result.success ? Results.Ok(new { message = result.message }) : Results.BadRequest(new { message = result.message });
})
.DisableAntiforgery();

// Este é o nosso endpoint principal.
// Ele ficará escutando por requisições POST no endereço "/api/recognize".

// ENDPOINT DE RECONHECIMENTO
app.MapPost("/api/recognize", async (IFormFile imageFile, FaceRecognitionService recognitionService) =>
{
    if (imageFile == null || imageFile.Length == 0)
    {
        return Results.BadRequest(new { message = "Nenhum arquivo de imagem foi enviado." });
    }

    using var ms = new MemoryStream();
    await imageFile.CopyToAsync(ms);
    
    // CORREÇÃO: Reposiciona o stream para o início antes de ser lido.
    ms.Position = 0; 

    using var bitmap = new Bitmap(ms);
    var image = FaceRecognition.LoadImage(bitmap);

    PersonIdentity identity = recognitionService.IdentifyFace(image);

    if (identity != null)
    {
        return Results.Ok(identity);
    }
    else
    {
        return Results.NotFound(new { message = "Nenhuma pessoa conhecida foi encontrada na imagem." });
    }
})
.DisableAntiforgery();

app.Run();
