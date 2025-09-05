using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using MappingService.Salesforce;
using MappingService.Sap;
using MappingService.Utils;

namespace MappingService.Services
{
    public partial class Service1 : ServiceBase
    {
        private SalesforceAuth _auth;
        private SalesforceApi _api;

        public Service1()
        {
            InitializeComponent();
            _auth = new SalesforceAuth();
            _api = new SalesforceApi();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                if (!System.Diagnostics.EventLog.SourceExists("MappingService"))
                    System.Diagnostics.EventLog.CreateEventSource("MappingService", "Application");

                Task.Run(async () =>
                {
                    try
                    {
                        // 1. Autentica Salesforce
                        var token = await _auth.GetValidToken();
                        Logger.Log("Token obtido com sucesso!");

                        // 2. Testa conexão SAP
                        var sap = new SapConnector("HANADB:30015", "SBO_ACOS_TESTE", "B1ADMIN", "S4P@2Q60_tm2");
                        var invoice = sap.GetOneInvoice();
                        if (invoice != null)
                        {
                            Logger.Log($"Conexão SAP OK. Exemplo OINV: {string.Join(", ", invoice)}");
                        }
                        else
                        {
                            Logger.Log("Nenhum resultado encontrado em OINV.");
                        }

                        // 3. Envia POST exemplo para Salesforce
                        var condicao = new
                        {
                            Name = "Example Text",
                            CA_CodCondPagamento__c = "Example Text",
                            CA_NumeroGrupo__c = "Teste",
                            CA_FonteDados__c = "I",
                            CA_NumPrestacoes__c = "Teste",
                            CA_MetodoCredito__c = "E",
                            CA_DataAtualizacao__c = "2025-06-18",
                            CA_CondAcoflex__c = "N",
                            CA_QuantParcelas__c = "0",
                            CA_CondSifra__c = "N",
                            CA_GeraAtendimento__c = "N",
                            CA_PrazoMedioCond__c = "Example Text",
                            CA_Ativo__c = true
                        };

                        var result = await _api.PostCondicaoPagamento(token, condicao);
                        Logger.Log($"POST Condição Pagamento retornou: {result}");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Erro no OnStart: {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("MappingService",
                    $"Error in OnStart : {ex.Message}",
                    System.Diagnostics.EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            Logger.Log("Serviço parado.");
        }
    }
}