
using Autofac;
using Reporting.MongoDb;
using Reporting.MongoDb.Repositories;
using Reporting.MongoDb.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utility.Dependency;
using Utility.Reflection;

namespace Reporting.API.Infrastructure.AutofacModules
{
    public class ApplicationModule
        : Autofac.Module
    {


        public ApplicationModule( )
        {

        }

        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = new AssemblyFinder().GetAllAssemblies().Where(x => x.GetName().FullName.StartsWith("Reporting"));

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


            builder.RegisterType<UnitOfWork>()
               .As<IUnitOfWork>()
               .InstancePerLifetimeScope();

            #region GenericRepository
            builder
                .RegisterGeneric(typeof(BaseRepository<>))
                .As(typeof(IRepository<>))
                .InstancePerDependency();
            #endregion

        }
    }
}
