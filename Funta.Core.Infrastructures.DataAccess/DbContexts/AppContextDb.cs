using Funta.Core.Domain.Entity;
using Funta.Core.Domain.Entity.Auth;
using Funta.Core.Domain.Entity.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Remotion.Linq.Parsing.ExpressionVisitors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;


namespace Funta.Core.Infrastructures.DataAccess
{
    public class AppContextDb : DbContext, IUnitOfWorks
    {
        public IDbConnection Connection { get; set; }
        private readonly IConfigurationRoot _configuration;
        #region CTOR
        public AppContextDb(DbContextOptions<AppContextDb> options) : base(options)
        {
            this._configuration = new ConfigurationRoot(new List<IConfigurationProvider>());
        }
        #endregion

        #region DBSETS
        public virtual DbSet<Users> UsersCtx { get; set; }
        public virtual DbSet<UserToken> UserTokenCtx { get; set; }
        public virtual DbSet<Roles> RolesCtx { get; set; }
        public virtual DbSet<UserRole> UserRoleCtx { get; set; }
        #endregion

        #region CONFIG
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApplyConfigurations(modelBuilder);
            ApplyQueryFilter(modelBuilder);

            modelBuilder.Ignore(typeof(BaseEntity<>));
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigureWarnings(optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }

        private void ApplyQueryFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(IAuditable).IsAssignableFrom(e.ClrType)))
            {
                modelBuilder
                    .Entity(entityType.ClrType)
                    .HasQueryFilter(ConvertFilterExpression<IAuditable>(e => !e.IsRemoved, entityType.ClrType));
            }
        }
        private static void ConfigureWarnings(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warning =>
            {
                warning.Log(CoreEventId.IncludeIgnoredWarning);
                warning.Log(RelationalEventId.QueryClientEvaluationWarning);
            })
                .UseLoggerFactory(new LoggerFactory().AddConsole((message, logLevel) =>
                {
                    return logLevel == LogLevel.Debug || logLevel == LogLevel.Error &&
                           message.StartsWith("Microsoft.EntityFrameworkCore.Database.Command");
                }));
        }
        private static LambdaExpression ConvertFilterExpression<TInterface>(
            Expression<Func<TInterface, bool>> filterExpression,
            Type entityType)
        {
            var newParam = Expression.Parameter(entityType);
            var newBody = ReplacingExpressionVisitor.Replace(filterExpression.Parameters.Single(), newParam, filterExpression.Body);

            return Expression.Lambda(newBody, newParam);
        }

        private static void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            var implementedConfigTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetInterfaces()
                    .Any(gi => gi.IsGenericType && gi.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
                .ToList();

            foreach (var type in implementedConfigTypes)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
        }

        #endregion

        #region UOW
        public void Begin() =>
            this.Database.BeginTransaction();

        public void Commit() =>
            this.Database.CommitTransaction();

        public void Rollback() =>
            this.Database.RollbackTransaction();

        #endregion
    }
}
