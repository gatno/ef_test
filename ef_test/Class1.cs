using System;
using ef_test.Database;
using Microsoft.Extensions.DependencyInjection;
using server.Database;

namespace ef_test
{
    public class Class1
    {
        private DatabaseCore _databaseCore;
        private static IServiceProvider _serviceProvider;

        public Class1()
        {
            _databaseCore = new DatabaseCore();

            var serviceCollection = new ServiceCollection();

            _databaseCore.OnResourceStartHandler(() =>
            {
                _serviceProvider = serviceCollection
                    .AddSingleton(ContextFactory.Instance)
                    .BuildServiceProvider();
                LoadServices();
            });

        }

        private static void LoadServices()
        {
            //test
        }

    }
}
