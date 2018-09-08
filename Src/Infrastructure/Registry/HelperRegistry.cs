using Src.Helpers;

namespace Src.Infrastructure.Registry
{
    public class HelperRegistry : StructureMap.Registry
    {
        public HelperRegistry()
        {
            For<IUserHelper>().Use<UserHelper>().Transient();
        }
    }
}
