
namespace PontoFacial.App;

public class IdentificacaoResultado
{
    // Os nomes das propriedades devem corresponder às chaves do JSON retornado pelo seu back-end.
    // Ex: O back-end retorna { "funcionarioId": 1, "nome": "Carlos Silva" }
    public int FuncionarioId { get; set; }
    public string Nome { get; set; }
    public string Cargo { get; set; } // Opcional, mas útil
}