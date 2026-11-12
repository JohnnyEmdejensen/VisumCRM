using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace VisumCRM
{
    public class DARAddresses
    {
        private static readonly HttpClient client = new HttpClient();

        // Husk at indsætte din egen API-nøgle fra Datafordeleren
        private const string ApiKey = "DIN_DATAFORDELER_API_KEY";

        // Tjek Dataoversigten på Datafordeleren for nyeste versionsnummer (f.eks. v2 eller v3)
        private const string Endpoint = $"https://datafordeler.dk{ApiKey}";

        public static async Task Main()
        {
            string adresseTilTjek = "Rådhuspladsen 1, 1550 København V";
            bool erGyldig = await KontrollerAdresseAsync(adresseTilTjek);

            Console.WriteLine($"Er adressen '{adresseTilTjek}' gyldig i DAR? {erGyldig}");
        }

        public static async Task<bool> KontrollerAdresseAsync(string adresseBetegnelse)
        {
            // Datafordeleren kræver bitemporale parametre (registreringstid og virkningstid i UTC)
            string nuUtc = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

            // GraphQL query mod DAR_Adresse
            string query = @"
        query TjekAdresse($betegnelse: String!, $tid: DafDateTime!) {
          DAR_Adresse(
            registreringstid: $tid
            virkningstid: $tid
            where: {
              adressebetegnelse: { eq: $betegnelse }
              status: { eq: ""3"" }
            }
          ) {
            nodes {
              id_lokalId
              adressebetegnelse
              status
            }
          }
        }";

            var payload = new
            {
                query = query,
                variables = new
                {
                    betegnelse = adresseBetegnelse,
                    tid = nuUtc
                }
            };

            string jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(Endpoint, content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            // Parse JSON-svaret og tjek om nodes-listen indeholder et match
            using JsonDocument doc = JsonDocument.Parse(responseBody);

            if (doc.RootElement.TryGetProperty("errors", out JsonElement errors))
            {
                throw new Exception($"GraphQL fejl: {errors}");
            }

            JsonElement nodes = doc.RootElement
                .GetProperty("data")
                .GetProperty("DAR_Adresse")
                .GetProperty("nodes");

            return nodes.GetArrayLength() > 0;
        }
    }
}
