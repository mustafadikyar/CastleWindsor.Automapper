using AutoMapper;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CastleWindsor.Automapper
{
    public class AutoMapperInstaller : IWindsorInstaller
    {      
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMapper>().UsingFactoryMethod(x =>
            {
                return new MapperConfiguration(c =>
                {
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("YourPath")))
                    {
                        var profiles = from t in assembly.GetTypes()
                                       where typeof(Profile).IsAssignableFrom(t) && t.Assembly != typeof(Profile).Assembly
                                       select Activator.CreateInstance(t) as Profile;

                        foreach (var profile in profiles)
                        {
                            c.AddProfile(profile);
                        }
                    }
                }).CreateMapper();
            }));
        }
    }
}