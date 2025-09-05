using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace MappingService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller _processInstaller;
        private ServiceInstaller _serviceInstaller;
        public ProjectInstaller()
        {
            //InitializeComponent();

            _processInstaller = new ServiceProcessInstaller();
            _serviceInstaller = new ServiceInstaller();

            _processInstaller.Account = ServiceAccount.LocalSystem;

            _serviceInstaller.ServiceName = "MappingService";
            _serviceInstaller.StartType = ServiceStartMode.Automatic;
            _serviceInstaller.Description = "Serviço de monitoramento de pasta";

            Installers.Add(_processInstaller);
            Installers.Add(_serviceInstaller);
        }
    }
}
