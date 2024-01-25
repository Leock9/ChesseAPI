using Domain;
using Domain.Ports;
using Domain.ValueObjects;
using Infrastructure.MongoDb.Documents;
using MongoDB.Driver;

namespace Infrastructure.MongoDb.Repository;

public class ItemMenuRepository : IItemMenuRepository
{
    private readonly Context _context;

    public ItemMenuRepository(Context context)
    {
        _context = context;
    }

    public void Create(ItemMenu itemMenu)
    {
        var document = new ItemMenuDocument
        {
            Id = itemMenu.Id,
            Name = itemMenu.Name,
            Description = itemMenu.Description,
            Price = itemMenu.Price,
            Stock = itemMenu.Stock,
            Ingredients = itemMenu.Ingredients,
            Size = itemMenu.Size,
            Category = itemMenu.Category,
            IsActive = itemMenu.IsActive,
            CreateAt = itemMenu.CreateAt,
            UpdateAt = itemMenu.UpdateAt
        };

        _context.GetCollection<ItemMenuDocument>("itemmenus").InsertOne(document);
    }

    public void Delete(Guid id) => _context.GetCollection<ItemMenuDocument>("itemmenus").DeleteOne(x => x.Id == id);

    public Task<ItemMenu> Get(Guid id)
    {
        var itemMenuDocument = _context.GetCollection<ItemMenuDocument>("itemmenu")
                                         .Find(x => x.Id == id)
                                         .FirstOrDefaultAsync();

        return itemMenuDocument.ContinueWith
            (x => new ItemMenu
             (
                x.Result.Name,
                x.Result.Description,
                x.Result.Price,
                x.Result.Stock,
                x.Result.Ingredients,
                x.Result.Size,
                x.Result.Category
             )
            );
    }

    public Task<IEnumerable<ItemMenu>> GetByCategory(Category category)
    {
        var filter = Builders<ItemMenuDocument>.Filter.Eq(x => x.Category, category);
        var result = _context.GetCollection<ItemMenuDocument>("itemmenu").Find(filter).ToList();

        var itemMenus =  result.Select(x => new ItemMenu
                                      (
                                      x.Name,
                                      x.Description,
                                      x.Price,
                                      x.Stock,
                                      x.Ingredients,
                                      x.Size,
                                      x.Category
                                      )).ToList();

        return Task.FromResult(itemMenus.AsEnumerable());
    }

    public void Update(ItemMenu itemMenu)
    {
        _context.GetCollection<ItemMenuDocument>("itemmenus")
                .ReplaceOne(x => x.Id == itemMenu.Id, new ItemMenuDocument
                {
                    Name = itemMenu.Name,
                    Description = itemMenu.Description,
                    Price = itemMenu.Price,
                    Stock = itemMenu.Stock,
                    Ingredients = itemMenu.Ingredients,
                    Size = itemMenu.Size,
                    IsActive = itemMenu.IsActive,
                    UpdateAt = itemMenu.UpdateAt
                });
    }
}