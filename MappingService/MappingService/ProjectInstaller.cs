using System.ComponentModel;
using System.ServiceProcess;

namespace MappingService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller _processInstaller;
        private ServiceInstaller _serviceInstaller;

        public ProjectInstaller()
        {
            _processInstaller = new ServiceProcessInstaller();
            _serviceInstaller = new ServiceInstaller();

            _processInstaller.Account = ServiceAccount.LocalSystem;

            _serviceInstaller.ServiceName = "MappingService";
            _serviceInstaller.DisplayName = "Mapping Service - Integração Salesforce + SAP";
            _serviceInstaller.StartType = ServiceStartMode.Automatic;
            _serviceInstaller.Description = "Serviço que integra SAP HANA (ODBC) e Salesforce";

            Installers.Add(_processInstaller);
            Installers.Add(_serviceInstaller);
        }
    }
}