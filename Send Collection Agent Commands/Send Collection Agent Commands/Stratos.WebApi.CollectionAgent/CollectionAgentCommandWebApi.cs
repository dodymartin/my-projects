using System.Collections.Generic;
using System.Threading.Tasks;
using Stratos.Base.WebApi;

namespace Stratos.WebApi.CollectionAgent
{
    public class CollectionAgentCommandWebApi
    {
        #region Properties

        private const int DEFAULT_TIMEOUT_MINS = 30;
        private IHttpBaseClient Client { get; set; }

        #endregion

        #region Ctors

        public CollectionAgentCommandWebApi(IEnumerable<string> collectionAgentBaseUris, int timeout = DEFAULT_TIMEOUT_MINS)
        {
            // Non-standard -- for Install Application only, typically
            // we use the ConfigurationWebApiV2
            //
            // Please do not replicate this pattern !!
            var config = new HttpBaseClientConfig
            {
                AcceptType = SerializationTypes.Json,
                AuthenticationType = AuthenticationTypes.Token,
                ContentType = SerializationTypes.Json,
                TimeoutMinutes = timeout,
                WebProxy = null
            };
            Client = new HttpBaseClient(collectionAgentBaseUris, config);

        }

        #endregion

        public async Task<string> GetLogFileAsync(string worker, bool throwExceptions = true)
        {
            var endpoint = $"workers/{worker}/log-files/latest";
            return await Client.PostAsync(endpoint, string.Empty, throwExceptions);
        }

        public async Task<string> PassthroughAsync(string machineName, string command, bool throwExceptions = true)
        {
            var endpoint = $"machines/{machineName}/passthrough";
            return await Client.PostAsync(endpoint, command, throwExceptions);
        }

        public async Task<string> DeleteServiceAsync(string serviceName, bool throwExceptions = true)
        {
            var endpoint = $"services/{serviceName}/delete";
            return await Client.PostAsync(endpoint, string.Empty, throwExceptions);
        }

        public async Task<string> RunWorkerAsync(string worker, bool throwExceptions = true)
        {
            var endpoint = $"workers/{worker}/run";
            return await Client.PostAsync(endpoint, string.Empty, throwExceptions);
        }
    }
}