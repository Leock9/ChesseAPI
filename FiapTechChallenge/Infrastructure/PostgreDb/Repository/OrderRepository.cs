using Domain;
using Domain.Ports;
using Npgsql;
using System.Text.Json;

namespace Infrastructure.PostgreDb.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly Context _context;

    public OrderRepository(Context context)
    {
        _context = context;
        EnsureTableExistsAsync().Wait();
    }

    public void Create(Order order)
    {
        var conn = _context.GetConnection();
        conn.Open();

        var jsonItemMenu = JsonSerializer.Serialize(order);
        using var cmd = new NpgsqlCommand("INSERT INTO orders (id, order_data) VALUES (@id, @data::jsonb)", conn);


        cmd.Parameters.AddWithValue("@id", order.Id);
        cmd.Parameters.AddWithValue("@data", jsonItemMenu);

        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public async Task<IEnumerable<Order>> GetAll()
    {
        var orders = new List<Order>();

        await using var conn = _context.GetConnection();
        await conn.OpenAsync();

        const string commandText = "SELECT order_data FROM orders;";
        await using var cmd = new NpgsqlCommand(commandText, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var orderJson = reader.GetString(0); 
            try
            {
                var order = await JsonSerializer.DeserializeAsync<Order>(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(orderJson)));
                if (order != null)
                {
                    orders.Add(order);
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro ao desserializar o objeto Order: {ex.Message}");
            }
        }

        return orders;
    }

    public async Task UpdateAsync(Order order)
    {
        await using var conn = _context.GetConnection();
        await conn.OpenAsync();

        var orderJson = JsonSerializer.Serialize(order);

        var commandText = @"
            UPDATE orders
            SET order_data = @orderData, updated_at = NOW()
            WHERE id = @id;
        ";

        await using var cmd = new NpgsqlCommand(commandText, conn);
        cmd.Parameters.AddWithValue("@id", order.Id);
        cmd.Parameters.AddWithValue("@orderData", orderJson);

        var rowsAffected = await cmd.ExecuteNonQueryAsync();
        if (rowsAffected == 0)
        {
            throw new InvalidOperationException($"Pedido com ID {order.Id} não encontrado.");
        }
    }

    private async Task EnsureTableExistsAsync()
    {
        var conn = _context.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand
                (
                   "CREATE TABLE IF NOT EXISTS orders (id uuid PRIMARY KEY, order_data jsonb)",
                   conn
                );

        await cmd.ExecuteNonQueryAsync();
        conn.Close();
    }
}
