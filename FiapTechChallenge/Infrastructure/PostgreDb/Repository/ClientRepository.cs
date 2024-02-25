using Domain;
using Domain.Ports;
using Npgsql;

namespace Infrastructure.PostgreDb.Repository;

public class ClientRepository : IClientRepository
{
    private readonly Context _context;

    public ClientRepository(Context context)
    {
        _context = context;
        EnsureTableExistsAsync();
    }

    public void Create(Client client)
    {
        var conn = _context.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand
            (
            "INSERT INTO clients (id, name, document, email) VALUES (@id, @name, @document, @email)",
            conn
            );
        cmd.Parameters.AddWithValue("id", client.Id);
        cmd.Parameters.AddWithValue("name", client.Name);
        cmd.Parameters.AddWithValue("document", client.Document);
        cmd.Parameters.AddWithValue("email", client.Email);
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public async Task<Client> GetByDocumentAsync(string document)
    {
        await using var conn = _context.GetConnection();
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand("SELECT * FROM clients WHERE document = @document", conn);
        cmd.Parameters.AddWithValue("@document", document);
        await using var reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync()) return null;
        var client =  new Client
        (
            reader.GetString(1), // Supondo que esta é a coluna 'name'
            reader.GetString(2), // Supondo que esta é a coluna 'document'
            reader.GetString(3)  // Supondo que esta é a coluna 'email'
        );

        await conn.CloseAsync();
        return client;
    }

    private Task EnsureTableExistsAsync()
    {
        var conn = _context.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand
        (
         "CREATE TABLE IF NOT EXISTS clients (id uuid PRIMARY KEY, name varchar(100), document varchar(11), email varchar(100))",
         conn
        );
        cmd.ExecuteNonQuery();
        conn.Close();
        return Task.CompletedTask;
    }
}
