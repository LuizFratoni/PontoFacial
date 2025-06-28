
// Para ter acesso a classes como DbContext e DbSet
using Microsoft.EntityFrameworkCore;

// É bom incluir para ter acesso a classes como Dictionary, List, etc., se necessário.
using System.Collections.Generic;

namespace PontoFacial.Api;

// Representa a nossa tabela na base de dados
public class RecognizedPerson
{
    public int Id { get; set; } // Chave primária automática
    public string UserId { get; set; } // ID fornecido pelo utilizador (ex: matrícula)
    public string Name { get; set; }
    public string FaceEncodingData { get; set; } // Encoding facial guardado como string JSON
}

// Contexto da base de dados que faz a ponte entre os nossos objetos C# e a base de dados
public class FaceDbContext : DbContext
{
    public DbSet<RecognizedPerson> People { get; set; }
   
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Se as opções ainda não foram configuradas, configura para usar SQLite.
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=faces.db");
        }
    }

}

