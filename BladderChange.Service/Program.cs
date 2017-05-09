using System.Configuration;
using Topshelf;

namespace BladderChange.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<BladderChangeDataService>(s =>
                {
                    s.ConstructUsing(name => new BladderChangeDataService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.SetDescription(ConfigurationManager.AppSettings["ServiceDescription"]);
                x.SetDisplayName(ConfigurationManager.AppSettings["ServiceDisplayName"]);
                x.SetServiceName(ConfigurationManager.AppSettings["ServiceName"]);

                x.StartAutomatically();
                x.RunAsLocalService();
            });
        }
    }
}
