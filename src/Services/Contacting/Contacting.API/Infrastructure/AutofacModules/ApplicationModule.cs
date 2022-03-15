using Contacting.Application.Commands;
using Contacting.Application.Queries;
using Contacting.Domain.SeedWork;
using Contacting.Infrastructure;
using Contacting.Infrastructure.Idempotency;
using Contacting.Infrastructure.Repositoryies;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utility.Dependency;
using Utility.Reflection;

namespace Contacting.API.Infrastructure.AutofacModules
{
    public class ApplicationModule
        : Autofac.Module
    {

        public string QueriesConnectionString { get; }

        public ApplicationModule(string qconstr)
        {
            QueriesConnectionString = qconstr;

        }

        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = new AssemblyFinder().GetAllAssemblies().Where(x => x.GetName().FullName.StartsWith("Contacting"));

            builder.RegisterAssemblyTypes(assemblies.ToArray())
                    .Where(x => !string.IsNullOrWhiteSpace(x.GetInterface(nameof(ITransientDependency))?.Name))
                    .AsSelf()
                    .AsImplementedInterfaces()
                    .InstancePerDependency();

            builder.RegisterAssemblyTypes(assemblies.ToArray())
                .Where(x => !string.IsNullOrWhiteSpace(x.GetInterface(nameof(ISingletonDependency))?.Name))
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();


            builder.RegisterType<ContactingContext>()
               .As<IUnitOfWork>()
               .InstancePerLifetimeScope();

            #region GenericRepository
            RegisterRepositories(builder, typeof(IRepository<,>), typeof(EfCoreRepositoryBase<,,>));
            #endregion

            #region Queries
            builder.Register(c => new PersonQueries(QueriesConnectionString))
                .As<IPersonQueries>()
                .InstancePerLifetimeScope();
            #endregion

            builder.RegisterType<RequestManager>()
               .As<IRequestManager>()
               .InstancePerLifetimeScope();
        }

        private void RegisterRepositories(
            ContainerBuilder builder,
            Type repositoryInterfaceWithPrimaryKey,
            Type repositoryImplementationWithPrimaryKey)
        {

            ITypeFinder typeFinder = new TypeFinder(new AssemblyFinder());

            var dbContextTypes =
                typeFinder.Find(type =>
                    type.IsPublic &&
                    !type.IsAbstract &&
                    type.IsClass &&
                    typeof(Microsoft.EntityFrameworkCore.DbContext).IsAssignableFrom(type)
                    );


            foreach (var dbContextType in dbContextTypes)
            {
                var list = GetEntityTypeInfos(dbContextType).ToList();

                foreach (var entityTypeInfo in list)
                {
                    var primaryKeyType = GetPrimaryKeyType(entityTypeInfo.EntityType);

                    var genericRepositoryTypeWithPrimaryKey = repositoryInterfaceWithPrimaryKey.MakeGenericType(entityTypeInfo.EntityType, primaryKeyType);

                    var implType = repositoryImplementationWithPrimaryKey.MakeGenericType(entityTypeInfo.DeclaringType, entityTypeInfo.EntityType, primaryKeyType);

                    builder.RegisterType(implType)
                        .As(genericRepositoryTypeWithPrimaryKey)
                        .InstancePerLifetimeScope();
                }
            }
        }

        private IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type dbContextType)
        {
            return
                from property in dbContextType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where
                (ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(Microsoft.EntityFrameworkCore.DbSet<>))) &&
                ReflectionHelper.IsAssignableToGenericType(property.PropertyType.GenericTypeArguments[0], typeof(IAggregateRoot))

                select new EntityTypeInfo(property.PropertyType.GenericTypeArguments[0], property.DeclaringType);
        }

        private Type GetPrimaryKeyType(Type entityType)
        {
            foreach (var interfaceType in entityType.GetTypeInfo().GetInterfaces())
            {
                if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEntity<>))
                {
                    return interfaceType.GenericTypeArguments[0];
                }
            }

            throw new Exception("Can not find primary key type of given entity type: " + entityType + ". Be sure that this entity type implements IEntity<TPrimaryKey> interface");
        }
    }
}
