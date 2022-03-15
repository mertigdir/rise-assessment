using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utility.Dependency;

namespace Utility
{
    public class UtilityModule
        : Autofac.Module
    {


        public UtilityModule()
        {

        }

        protected override void Load(ContainerBuilder builder)
        {

            var assembly = Assembly.GetAssembly(typeof(UtilityModule));

            builder.RegisterAssemblyTypes(assembly)
                    .Where(x => !string.IsNullOrWhiteSpace(x.GetInterface(nameof(ITransientDependency))?.Name))
                    .AsSelf()
                    .AsImplementedInterfaces()
                    .InstancePerDependency();

            builder.RegisterAssemblyTypes(assembly)
                .Where(x => !string.IsNullOrWhiteSpace(x.GetInterface(nameof(ISingletonDependency))?.Name))
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
