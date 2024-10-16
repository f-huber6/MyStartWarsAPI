using Microsoft.EntityFrameworkCore;

namespace Database;

public static class NewMethods
{
    //a bissl ausprowian
    public static async Task AddAsync<TEntity>(DbContext dbContext, TEntity? entity) where TEntity : class
    {
        if (entity == null)
        {
            throw new ArgumentException( "Cannot be null");
        }

        var entityType = dbContext.Model.FindEntityType(typeof(TEntity));
        if (entityType != null)
        {
            var tableName = entityType.GetTableName();
        
            var nameProperty = typeof(TEntity).GetProperty(entityType.Name)?.GetValue(entity)?.ToString();
            var factionProperty = typeof(TEntity).GetProperty("Faction")?.GetValue(entity)?.ToString();
            var homeWorldProperty = typeof(TEntity).GetProperty("HomeWorld")?.GetValue(entity)?.ToString();
            var speciesProperty = typeof(TEntity).GetProperty("Species")?.GetValue(entity)?.ToString();

            if (!String.IsNullOrEmpty(nameProperty) && !String.IsNullOrEmpty(factionProperty) && !String.IsNullOrEmpty(homeWorldProperty) && !String.IsNullOrEmpty(speciesProperty))
            {
                var sql =
                    $"Insert Into {tableName} (Name, Faction, HomeWorld, Species) Values (@Name, @Faction, @HomeWorld, @Species)";
                await dbContext.Database.ExecuteSqlRawAsync(sql, nameProperty, factionProperty, homeWorldProperty, speciesProperty);
            }
            else
            {
                throw new InvalidOperationException("Values of entities cannot be null");
            }
        }
    }

    /*
    public static async Task AddAsync2<TEntity>(DbContext dbContext, TEntity entity) where TEntity : class
    {
        if(entity == null)
        {
            throw new ArgumentException("Cannot be null");
        }
        
        var entityType = dbContext.Model.FindEntityType(typeof(TEntity));
        var tableName = entityType.GetTableName();
        var properties = entityType.GetProperties();

        var columnNames = new List<TEntity>();
        var values = new List<TEntity>();

        foreach (var property in properties)
        {
            columnNames.Add(property.GetColumnName());
            values.Add(typeof(TEntity).GetProperty(property.Name).GetValue(entity).ToString());
        }
        
    }
    */
}