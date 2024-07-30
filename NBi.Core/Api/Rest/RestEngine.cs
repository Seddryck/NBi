using NBi.Core.Api.Authentication;
using NBi.Extensibility.Resolving;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Api.Rest
{
    public class RestEngine
    {
        public IAuthentication Authentication { get; }
        public IScalarResolver<string> BaseUrl { get; }
        public IScalarResolver<string> Path { get; }
        public IEnumerable<ParameterRest> Parameters { get; }
        public IEnumerable<SegmentRest> Segments { get; } = Array.Empty<SegmentRest>();
        public IEnumerable<HeaderRest> Headers { get; } = Array.Empty<HeaderRest>();

        public RestEngine(IAuthentication authentication, IScalarResolver<string> baseUrl, IScalarResolver<string> path, IEnumerable<ParameterRest> parameters, IEnumerable<SegmentRest> segments, IEnumerable<HeaderRest> headers)
            => (Authentication, BaseUrl, Path, Parameters, Segments, Headers) = (authentication, baseUrl, path, parameters ?? Array.Empty<ParameterRest>(), segments ?? Array.Empty<SegmentRest>(), headers ?? Array.Empty<HeaderRest>());

        public string Execute()
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var baseUrl = BaseUrl.Execute() ?? string.Empty;
            var client = new RestClient(baseUrl)
            {
                Authenticator = Authentication.GetAuthenticator()
            };
            var path = Path?.Execute() ?? string.Empty;
            var request = new RestRequest(path, Method.GET);

            foreach (var parameter in Parameters)
                request.AddParameter(
                    parameter.Name.Execute() ?? throw new NullReferenceException()
                    , parameter.Value.Execute() ?? string.Empty
                );

            foreach (var segment in Segments)
                request.AddUrlSegment(
                    segment.Name.Execute() ?? throw new NullReferenceException()
                    , segment.Value.Execute() ?? string.Empty
                );

            foreach (var header in Headers)
                request.AddHeader(
                    header.Name.Execute() ?? throw new NullReferenceException()
                    , header.Value.Execute() ?? string.Empty
                );

            var response = client.Execute(request);
            return response.Content; 
        }
    }
}
