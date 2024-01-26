using Domain;
using Domain.Ports;
using Infrastructure.MongoDb.Documents;
using MongoDB.Driver;

namespace Infrastructure.MongoDb.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly Context _context;

    public OrderRepository(Context context)
    {
        _context = context;
    }

    public void Create(Order order)
    {
        var document = new OrderDocument
        {
            Id = order.Id,
            ItemMenus = order.ItemMenus,
            TotalOrder = order.TotalOrder,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };

        _context.GetCollection<OrderDocument>("orders").InsertOne(document);
    }

    public void UpdateAsync(Order order)
    {
        _context.GetCollection<OrderDocument>("orders")
                .ReplaceOne(x => x.Id == order.Id, new OrderDocument
                {
                    Id = order.Id,
                    ItemMenus = order.ItemMenus,
                    TotalOrder = order.TotalOrder,
                    Status = order.Status,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt
                });
    }

    public Task<IEnumerable<Order>> GetAll()
    {
        var result = _context.GetCollection<OrderDocument>("orders")
                       .Find(x => true)
                       .ToListAsync();

        var orders = result.Result.Select(x => new Order
               (
                x.TotalOrder,
                x.Document,
                x.ItemMenus
                ));

       return Task.FromResult(orders);
    }
}
