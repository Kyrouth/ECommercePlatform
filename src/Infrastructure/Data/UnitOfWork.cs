using Application.Common.Interfaces;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        UpdateBaseAuditableEntities();
        return _dbContext.SaveChangesAsync(cancellationToken);
    }


    private void UpdateBaseAuditableEntities()
    {
        var entires = _dbContext.ChangeTracker
            .Entries<BaseAuditableEntity>();

        foreach (var entityEntry in entires)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(e => e.Created).CurrentValue = DateTime.UtcNow;
                entityEntry.Property(e => e.LastModified).CurrentValue = DateTime.UtcNow;
            }
            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(e => e.LastModified).CurrentValue = DateTime.UtcNow;
            }
        }
    }
}