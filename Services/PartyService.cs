using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.Implementations;
using System.Xml.Linq;
using Models.Exceptions;
using Newtonsoft.Json;
using Worker.Helpers;
using System.Net.Http;

namespace Worker.Services
{
    public class PartyService
    {
        private HttpClient _httpClient;
        private JRSettings _settings;

        public PartyService(JRSettings settings)
        {
            _settings = settings;
            _httpClient = WebWrapper.GetClient(settings);
        }

        public async Task<PartyRoot> Get(string partyId)
        {
            try
            {
                var endPointForCourse = _settings.EndPoints.Party;
                endPointForCourse = endPointForCourse.Replace("{{partyId}}", partyId);
                var responseStream = await _httpClient.GetAsync(endPointForCourse);
                var response = await responseStream.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(response);
                var json = JsonConvert.SerializeXNode(doc.Descendants("party").FirstOrDefault());
                return JsonConvert.DeserializeObject<PartyRoot>(json);
            }
            catch (Exception ex)
            {
                var error = new ExceptionModel { ErrorCode = "PartyError:001", ErrorMessage = ex.Message };
                return new PartyRoot();
            }
        }

        public bool AddFileNote(string partyId, FileNote note)
        {
            return false;
        }

        public async Task<string> GetFileNotes(string partyId)
        {
            try
            {
                var endPointForCourse = _settings.EndPoints.PartyFileNotes;
                endPointForCourse = endPointForCourse.Replace("{{partyId}}", partyId);
                var responseStream = await _httpClient.GetAsync(endPointForCourse);
                var response = await responseStream.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(response);
                var json = JsonConvert.SerializeXNode(doc.Descendants("party").FirstOrDefault());
                return (json);
            }
            catch (Exception ex)
            {
                var error = new ExceptionModel { ErrorCode = "PartyError:002", ErrorMessage = ex.Message };
                return JsonConvert.SerializeObject(error);
            }
        }

    }
}
