# 📄 MappingService

## 🚀 Sobre o Projeto
O **MappingService** é um serviço Windows desenvolvido em C# (.NET Framework 4.8) que realiza integração entre **SAP HANA** (via ODBC) e **Salesforce** (via API REST).  

- Recupera dados do SAP através de queries ODBC.  
- Envia registros formatados para objetos customizados do Salesforce.  
- Mantém logs detalhados em arquivo e no Event Viewer do Windows.  

---

## ⚙️ Estrutura do Projeto

```text
MappingService
│
├─ Config
│   ├─ ConfigCred.cs         # Centraliza credenciais do Salesforce (ClientId e ClientSecret)
│   └─ ConfigUrls.cs         # Centraliza URLs de autenticação e endpoints da API
│
├─ Salesforce
│   ├─ SalesforceAuth.cs     # Autenticação no Salesforce (OAuth2 Client Credentials)
│   └─ SalesforceApi.cs      # Chamadas de API (GET/POST para objetos no Salesforce)
│
├─ Sap
│   └─ SapConnector.cs       # Conexão ODBC com SAP HANA (ex.: SELECT TOP 1 * FROM OINV)
│
├─ Utils
│   └─ Logger.cs             # Grava logs em arquivo (C:\Logs\MappingService) e Event Viewer
│
├─ Services
│   └─ Service1.cs           # Orquestra o fluxo (SAP → Salesforce) e ciclo de vida do Windows Service
│
├─ Program.cs                # Entry point do serviço (ServiceBase.Run)
└─ ProjectInstaller.cs       # Define instalação do serviço no Windows (nome, descrição, conta de execução)
```

---

## 🛠️ Pré-requisitos

- Visual Studio 2019/2022  
- .NET Framework 4.8  
- Driver ODBC do SAP HANA (HDBODBC) instalado  
- Acesso ao banco de dados SAP (usuário e senha válidos)  
- Credenciais Salesforce (ClientId e ClientSecret do Connected App)  

---

## 🔧 Instalação do Serviço

Compile o projeto em **Release** e vá até a pasta `bin\Release` (ou `bin\x64\Release` se estiver usando 64 bits).  

### ▶️ Instalar o serviço
**Any CPU / x86**
```powershell
& "C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" MappingService.exe
```

**x64**
```powershell
& "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe" MappingService.exe
```

### 🗑️ Desinstalar o serviço
```powershell
& "C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" /u MappingService.exe
```

---

## 📌 Funcionamento do Serviço

1. **Start (`OnStart`)**
   - Obtém o token de acesso do Salesforce (OAuth2).  
   - Conecta ao SAP HANA via ODBC e executa `SELECT TOP 1 * FROM OINV`.  
   - Loga o resultado da query (sucesso ou falha).  
   - Envia um `POST` de teste para o objeto `CA_CondicaoPagamento__c` no Salesforce.  
   - Registra tudo em log (`C:\Logs\MappingService\LogService.txt`) e no **Event Viewer**.  

2. **Stop (`OnStop`)**
   - Escreve no log que o serviço foi interrompido.  

---

## 📝 Logs

- Local: `C:\Logs\MappingService\LogService.txt`  
- Event Viewer: **Application → Source: MappingService**  

Cada entrada contém:  
```text
[Data Hora] - [Máquina] - [Mensagem]
```

---

## 🔮 Próximos Passos (Roadmap)

- [ ] Implementar execução periódica (Timer para rodar a cada X minutos).  
- [ ] Parametrizar queries do SAP (não fixar apenas OINV).  
- [ ] Evoluir configuração (usar JSON/variáveis de ambiente em vez de headers hardcoded).  
- [ ] Adicionar tratamento de retry com backoff para chamadas Salesforce.  
- [ ] Possível integração futura com **Azure Key Vault** para gestão de segredos.  
