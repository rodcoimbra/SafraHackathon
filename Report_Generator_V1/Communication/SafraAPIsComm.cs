using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Report_Generator_V1.Model;
using Report_Generator_V1.Model.TransactionData;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Generator_V1.Communication
{
    public class SafraAPIsComm
    {
        public Root TransactionRq(string token)
        {
            var client = new RestClient("https://af3tqle6wgdocsdirzlfrq7w5m.apigateway.sa-saopaulo-1.oci.customer-oci.com/fiap-sandbox/open-banking/v1/accounts/00711234511/transactions");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            string separator = "\":\"";
            int ini = 0;

            try
            {
                ini = token.IndexOf(separator) + separator.Length;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            string encryptedToken = token.Substring(ini, token.Length - ini);
            request.AddHeader("Authorization", "Bearer " + encryptedToken);
            IRestResponse response;

            try
            {
                response = client.Execute(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var deserializedJson = JsonConvert.DeserializeObject<Root>(response.Content);

            return deserializedJson;
            
        }

        public string OAuthRq(string clientToken)
        {
            var client = new RestClient("https://idcs-902a944ff6854c5fbe94750e48d66be5.identity.oraclecloud.com/oauth2/v1/token");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "Basic " + clientToken);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("postman-token", "280d6ac2-0e1c-d7ed-fc20-85de145f3d1c");
            request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&scope=urn:opc:resource:consumer::all", ParameterType.RequestBody);
            IRestResponse response;

            try
            {
                response = client.Execute(request);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return response.Content;
        }

    }
}
