using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AngOE.Common.Installer;
using AngOE.Logic.Installer;
using AngOE.Repository.Installer;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace AngOE.API.Installer
{
    public class WindsorBootstrapper
    {
        public static IWindsorContainer Container { get; private set; }

        public static void Initialize()
        {
            Container = new WindsorContainer();
            Container.Install(
                FromAssembly.This(),
                FromAssembly.Containing<LogicInstaller>(),
                FromAssembly.Containing<RepositoryInstaller>(),
                FromAssembly.Containing<CommonInstaller>()
                );

            Container.Register(Component.For<IWindsorContainer>().Instance(Container).LifestyleSingleton());
        }
    }
}