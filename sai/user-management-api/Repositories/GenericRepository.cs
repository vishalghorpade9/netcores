using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using user_management_api.Data;

namespace user_management_api.Repositories
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal UserManagementDbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(UserManagementDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual bool HasDuplicates(
        Dictionary<string, object> columnValues,
        Dictionary<string, object> notEqualColumnValues,
        TEntity entityToExclude = null) 
        {
            if (columnValues == null || !columnValues.Any())
            {
                throw new ArgumentException("Column values dictionary cannot be null or empty.");
            }

            var parameter = Expression.Parameter(typeof(TEntity), "e");
            Expression combinedPredicate = null;

            foreach (var entry in columnValues)
            {
                var propertyName = entry.Key;
                var propertyValue = entry.Value;

                var property = typeof(TEntity).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                {
                    throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(TEntity).Name}'.");
                }

                var propertyAccess = Expression.Property(parameter, property);
                var constantValue = Expression.Constant(propertyValue, property.PropertyType);
                var equalityExpression = Expression.Equal(propertyAccess, constantValue);

                if (combinedPredicate == null)
                {
                    combinedPredicate = equalityExpression;
                }
                else
                {
                    combinedPredicate = Expression.AndAlso(combinedPredicate, equalityExpression);
                }
                
            }

            // not equal conditions
            foreach (var entry in notEqualColumnValues)
            {
                var propertyName = entry.Key;
                var propertyValue = entry.Value;

                var property = typeof(TEntity).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                {
                    throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(TEntity).Name}'.");
                }

                var propertyAccess = Expression.Property(parameter, property);
                var constantValue = Expression.Constant(propertyValue, property.PropertyType);
                var equalityExpression = Expression.Equal(propertyAccess, constantValue);

                if (combinedPredicate == null)
                {
                    combinedPredicate = equalityExpression;
                }
                else
                {
                    combinedPredicate = Expression.NotEqual(combinedPredicate, equalityExpression);
                }

            }

            // If an entityToExclude is provided, add a condition to exclude it (useful for updates)
            if (entityToExclude != null)
            {
                var primaryKeyProperty = typeof(TEntity).GetProperties()
                                                        .FirstOrDefault(p => p.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>() != null);
                if (primaryKeyProperty == null)
                {
                    throw new InvalidOperationException($"Entity '{typeof(TEntity).Name}' does not have a primary key defined. Cannot exclude entity.");
                }

                var primaryKeyAccess = Expression.Property(parameter, primaryKeyProperty);
                var primaryKeyValue = primaryKeyProperty.GetValue(entityToExclude);
                var primaryKeyConstant = Expression.Constant(primaryKeyValue, primaryKeyProperty.PropertyType);
                var notEqualExpression = Expression.NotEqual(primaryKeyAccess, primaryKeyConstant);

                combinedPredicate = Expression.AndAlso(combinedPredicate, notEqualExpression);
            }

            var lambda = Expression.Lambda<Func<TEntity, bool>>(combinedPredicate, parameter);

            return dbSet.Any(lambda);
        }
    }
}
