using Autofac;
using ECommerce.Membership.Models;
using ECommerce.Membership.Repositories;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ECommerce.Membership
{
    public class MembershipModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RegisterModel>().AsSelf();
            builder.RegisterType<LoginModel>().AsSelf();
            builder.RegisterType<EmailConfirmModel>().AsSelf();
            builder.RegisterType<AccountRepository>().As<IAccountRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ActionContextAccessor>().As<IActionContextAccessor>()
                .SingleInstance();
            base.Load(builder);
        }
    }
}
