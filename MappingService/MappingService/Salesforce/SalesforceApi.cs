using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MappingService.Config;

namespace MappingService.Salesforce
{
    public class SalesforceApi
    {
        public async Task<string> PostCondicaoPagamento(string token, object condicao)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var json = JsonConvert.SerializeObject(condicao);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(ConfigUrls.ApiCondicaoPagamento, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new System.Exception($"Erro ao enviar Condição Pagamento: {result}");

                return result;
            }
        }
    }
}