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
    public class CourseService
    {
        private HttpClient _httpClient;
        private JRSettings _settings;

        public CourseService(JRSettings settings)
        {
            _settings = settings;
            _httpClient = WebWrapper.GetClient(settings);
        }

        /// <summary>
        /// Gets the course details for the course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<Course> Get(string courseId)
        {
            try
            {
                var endPointForCourse = _settings.EndPoints.Courses;
                endPointForCourse = endPointForCourse.Replace("{{courseId}}", courseId);
                var responseStream = await _httpClient.GetAsync(endPointForCourse);
                var response = await responseStream.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(response);
                var json = JsonConvert.SerializeXNode(doc.Descendants("course").FirstOrDefault());
                return JsonConvert.DeserializeObject<CourseRoot>(json).course;
            }
            catch (Exception ex)
            {
                var error = new ExceptionModel { ErrorCode = "CourseError:001", ErrorMessage = ex.Message };
                return new Course();
            }

        }

    }
}
