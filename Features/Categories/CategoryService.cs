using LinqKit;
using Mapster;
using ERP.Shared.Commons;
using ERP.Shared.Categories;
using ERP.Ticketing.HttpApi.Data;
using Microsoft.EntityFrameworkCore;
using ERP.Ticketing.HttpApi.Commons.Exceptions;
using ERP.Ticketing.HttpApi.Commons.Extensions;
using ERP.Ticketing.HttpApi.Features.Categories.Mappers;

namespace ERP.Ticketing.HttpApi.Features.Categories;

public class CategoryService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CategoryService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<PagedResult<CategoryDto>> ListOfCategory(CategoryListFilter filters, CancellationToken cancellationToken = default)
    {
        var predicate = PredicateBuilder.New<Category>(true);

        if (filters.Q != null)
        {
            predicate = predicate.And(x => x.Title.Contains(filters.Q));
        }

        return await _dbContext.Categories.AsExpandable()
            .Include(x => x.Creator)
            .Where(predicate)
            .Where(x => x.CreatorId == _httpContextAccessor.HttpContext!.User.GetUserId())
            .OrderByDescending(x => x.Id)
            .Select(CategoryMapper.ListMapper)
            .SortAndPagedResultAsync(filters, cancellationToken);
    }
    
    public async Task<CategoryDto> GetById(Guid categoryId, CancellationToken cancellationToken = default)
    {
        var category = await _dbContext.Categories
            .Where(x => x.CreatorId == _httpContextAccessor.HttpContext!.User.GetUserId())
            .FirstOrDefaultAsync(x => x.Id == categoryId, cancellationToken);

        if (category == null)
        {
            throw new CategoryNotFoundException();
        }

        return category.Adapt<CategoryDto>();
    }

    public async Task Create(CategoryCreateDto request, CancellationToken cancellationToken = default)
    {
        _dbContext.Categories.Add(new Category()
        {
            Title = request.Title
        });

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task Update(Guid categoryId, CategoryCreateDto request, CancellationToken cancellationToken = default)
    {
        var category = await _dbContext.Categories
            .Where(x => x.CreatorId == _httpContextAccessor.HttpContext!.User.GetUserId())
            .FirstOrDefaultAsync(x => x.Id == categoryId, cancellationToken);

        if (category == null)
        {
            throw new CategoryNotFoundException();
        }
        
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task Delete(Guid categoryId, CancellationToken cancellationToken = default)
    {
        var category = await _dbContext.Categories
            .Where(x => x.CreatorId == _httpContextAccessor.HttpContext!.User.GetUserId())
            .FirstOrDefaultAsync(x => x.Id == categoryId, cancellationToken);

        if (category == null)
        {
            throw new CategoryNotFoundException();
        }

        var checkCategoryAssignedInTicket = await _dbContext.TicketCategories
            .Where(x => x.CategoryId == categoryId)
            .FirstOrDefaultAsync(cancellationToken);

        if (checkCategoryAssignedInTicket != null)
        {
            throw new CategoryAssignedToTicketException();
        }
        
        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}