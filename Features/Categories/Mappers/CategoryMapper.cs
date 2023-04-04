using LinqKit;
using ERP.Shared.Categories;
using System.Linq.Expressions;
using ERP.Ticketing.HttpApi.Features.Ticketing;
using ERP.Ticketing.HttpApi.Features.Users.Mappers;

namespace ERP.Ticketing.HttpApi.Features.Categories.Mappers;

public static class CategoryMapper
{
    public static Expression<Func<Category, CategoryDto>> ListMapper = category => new CategoryDto()
    {
        Id = category.Id,
        Title = category.Title,
        Creator = CreatorMapper.Mapper.Invoke(category.Creator),
        CreatedAt = category.CreatedAt
    };
    
    public static Expression<Func<TicketCategory, SimpleCategoryDto>> TicketCategoryMapper = category => new SimpleCategoryDto()
    {
        Id = category.CategoryId,
        Title = category.Category.Title,
    };
}