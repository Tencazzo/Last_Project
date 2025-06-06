using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Project3.Services;
using Project3.Forms;
using Project3.Data;
using System;
using Component = Castle.MicroKernel.Registration.Component;

namespace Project3.Infrastructure
{
    public static class DIContainer
    {
        private static WindsorContainer? _container;

        public static void Initialize()
        {
            _container = new WindsorContainer();

            // Регистрация сервисов
            _container.Register(Component.For<ILogger>().ImplementedBy<FileLogger>().LifestyleSingleton());
            _container.Register(Component.For<ILocalizationService>().ImplementedBy<LocalizationService>().LifestyleSingleton());
            _container.Register(Component.For<IDatabaseService>().ImplementedBy<PostgreSQLService>().LifestyleSingleton());
            _container.Register(Component.For<IUserService>().ImplementedBy<UserService>().LifestyleSingleton());
            _container.Register(Component.For<IGameService>().ImplementedBy<GameService>().LifestyleSingleton());
            _container.Register(Component.For<INetworkService>().ImplementedBy<NetworkService>().LifestyleSingleton());

            // Регистрация форм
            _container.Register(Component.For<LoginForm>().LifestyleTransient());
            _container.Register(Component.For<RegisterForm>().LifestyleTransient());
            _container.Register(Component.For<MainForm>().LifestyleTransient());
            _container.Register(Component.For<GameForm>().LifestyleTransient());
            _container.Register(Component.For<ResultForm>().LifestyleTransient());
        }

        public static T Resolve<T>() where T : class
        {
            if (_container == null)
                throw new InvalidOperationException("Container not initialized");

            return _container.Resolve<T>();
        }

        public static void Dispose()
        {
            _container?.Dispose();
        }
    }
}
