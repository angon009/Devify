using Autofac;
using ECommerce.Core.DbContexts;

namespace ECommerce.Core
{
    public class CoreModule : Module
    {
        private readonly string _connectionString;
        private readonly string _assemblyName;


        public CoreModule(string connectionString, string assemblyName)
        {
            _connectionString = connectionString;
            _assemblyName = assemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CoreDbContext>().AsSelf()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("assemblyName", _assemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<CoreDbContext>().As<ICoreDbContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("assemblyName", _assemblyName)
                .InstancePerLifetimeScope();
        }
    }
}
