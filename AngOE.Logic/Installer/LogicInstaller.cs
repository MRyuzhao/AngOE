using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace AngOE.Logic.Installer
{
    public class LogicInstaller:IWindsorInstaller
    {
        public void Install(IWindsorContainer container,IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining<LogicInstaller>()
                .Where(x=>x.Name.EndsWith("Logic"))
                .WithService.DefaultInterfaces()
                .LifestylePerWebRequest());
        }
    }
}