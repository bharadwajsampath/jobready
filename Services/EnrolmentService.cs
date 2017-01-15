using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Models;
using Worker.Helpers;
using Models.Implementations;
using Models.Exceptions;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Xml.Linq;
namespace Worker.Services
{
    public class EnrolmentService
    {
        private HttpClient _httpClient;
        private JRSettings _settings;

        public EnrolmentService(JRSettings settings)
        {
            _settings = settings;
            _httpClient = WebWrapper.GetClient(settings);
        }


        public async Task<string> Get(string enrolmentId)
        {
            try
            {
                var endPointForCourse = _settings.EndPoints.Enrolments;
                endPointForCourse = endPointForCourse.Replace("{{enrolmentId}}", enrolmentId);
                var responseStream = await _httpClient.GetAsync(endPointForCourse);
                var response = await responseStream.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(response);
                var json = JsonConvert.SerializeXNode(doc.Descendants("enrolment").FirstOrDefault());
                return (json);
            }
            catch (Exception ex)
            {
                var error = new ExceptionModel { ErrorCode = "EnrolmentError:001", ErrorMessage = ex.Message };
                return JsonConvert.SerializeObject(error);
            }
        }
    }
}
