using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Acquirer.Client.UnitTests
{
    public sealed class HttpMessageHandlerStub : HttpMessageHandler
    {
        private readonly IList<HttpRequestMessage> capturedRequestMessages = new List<HttpRequestMessage>();

        public HttpResponseMessage ResponseMessage { get; set; }

        public IList<HttpRequestMessage> CapturedRequestMessages => capturedRequestMessages;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            capturedRequestMessages.Add(request);
            return Task.FromResult(ResponseMessage);
        }
    }
}