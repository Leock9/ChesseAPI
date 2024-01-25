using Domain.Services.Requests;
using Domain.ValueObjects;

namespace Domain.Services;

public interface IItemMenuService
{
    public void Create(BaseItemMenuRequest itemMenuRequest);
    public Task Update(UpdateItemMenuRequest itemMenuRequest);
    public void Delete(Guid id);
    public Task<ItemMenu> Get(Guid id);
    public Task<IEnumerable<ItemMenu>> GetByCategory(int categoryId);
}

