using System.ServiceProcess;

namespace MappingService
{
    internal static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Services.Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}