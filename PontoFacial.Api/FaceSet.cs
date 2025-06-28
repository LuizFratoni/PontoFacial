using Microsoft.EntityFrameworkCore;

namespace PontoFacial.Api;

public class PersonIdentity
{
    public object Id { get; set; }
    public string Name { get; set; }
    public double[] EncodingData { get; set; }
}

public class IdentyInfo
{
    public object Id { get; set; }
    public string Name { get; set;  }
}

public class FaceSet
{

    private static FaceSet instance;


    public Dictionary<string, PersonIdentity> KnownFaces { get; set; } = new Dictionary<string, PersonIdentity>();

    public FaceSet Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FaceSet();
                instance.LoadKnownFacesFromDb();
            }
            return instance;
        }
    }
    private FaceSet()
    {

    }

    private void LoadKnownFacesFromDb()
    {
        using var dbContext = new FaceDbContext();
        var people = dbContext.People.ToList();
        KnownFaces.Clear();

        foreach (var person in people)
        {
            double[] encodingData = JsonSerializer.Deserialize<double[]>(person.FaceEncodingData);
            KnownFaces[person.UserId] = new PersonIdentity
            {
                Id = person.UserId,
                Name = person.Name,
                EncodingData = encodingData
            };
        }
        Console.WriteLine($"Carregados {_knownFaces.Count} rostos conhecidos da base de dados.");
    }

    public async Task RegisterNewPerson(string userId, string name, string rawEncoding, double[] encodingData)
    {
        using FaceDbContext dbContext = new FaceDbContext();

        
        RecognizedPerson pessoaExistente = await dbContext.People.FirstOrDefaultAsync(p => p.UserId == userId);

        if (pessoaExistente != null)
        {
            Console.WriteLine($"Atualizando utilizador existente: {name} (ID: {userId})");
            pessoaExistente.Name = name;
            pessoaExistente.FaceEncodingData = encodingJson;


            dbContext.People.Update(pessoaExistente); // Marca a entidade como modificada
            await dbContext.SaveChangesAsync();

            PersonIdentity p = _knownFaces[userId];
            p.EncodingData = encodingData;

            return (true, $"Usuário '{userId}' foi atualizado com sucesso.");
        }
        else
        {
            Console.WriteLine($"Registando novo utilizador: {name} (ID: {userId})");
            var newPerson = new RecognizedPerson
            {
                UserId = userId,
                Name = name,
                FaceEncodingData = encodingJson
            };

            dbContext.People.Add(newPerson);
            await dbContext.SaveChangesAsync();

            PersonIdentity p = new PersonIdentity
            {
                Id = userId,
                Name = name,
                EncodingData = rawEncoding
            };

            KnownFaces[userId] = p;
            return (true, $"Utilizador '{name}' registado com sucesso.");
        }
    }


}   