using System.Net;
using AzureFunc.Models.School;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Snoopy.Function
{
    public class HttpWebApi
    {
        private readonly SchoolContext _context;

        public HttpWebApi(ILoggerFactory loggerFactory, SchoolContext context)
        {
            _logger = loggerFactory.CreateLogger<HttpWebApi>();
            _context = context;
        }

        private readonly ILogger<HttpWebApi> _logger;

        public HttpWebApi(ILogger<HttpWebApi> logger)
        {
            _logger = logger;
        }

        [Function("HttpWebApi")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }

        [Function("GetStudents")]
        public HttpResponseData GetStudents(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "students")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP GET/posts trigger function processed a request in GetStudents().");

            var students = _context.Students.ToArray();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");

            response.WriteStringAsync(JsonConvert.SerializeObject(students));

            return response;
        }

    }
}
