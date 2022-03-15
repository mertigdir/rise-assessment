
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utility.Dependency;

namespace Utility.Reflection
{
    public class AssemblyFinder : IAssemblyFinder, ISingletonDependency
    {

        public AssemblyFinder()
        {
        }

        public List<Assembly> GetAllAssemblies()
        {
            List<Assembly> listOfAssemblies = new List<Assembly>();
            var mainAsm = Assembly.GetEntryAssembly();
            listOfAssemblies.Add(mainAsm);

            foreach (var refAsmName in mainAsm.GetReferencedAssemblies())
            {
                listOfAssemblies.Add(Assembly.Load(refAsmName));
            }
            return listOfAssemblies.Distinct().ToList();
        }
    }
}