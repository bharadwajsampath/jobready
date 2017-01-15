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
using System.Xml;
using System.Text;

namespace Worker.Services
{
    public class EventService
    {
        private HttpClient _httpClient;
        private JRSettings _settings;

        public EventService(JRSettings settings)
        {
            _settings = settings;
            _httpClient = WebWrapper.GetClient(settings);
        }

        public async Task<Events> GetAllEventsForToday()
        {
            try
            {
                var endPointForCourse = _settings.EndPoints.Events;
                endPointForCourse = endPointForCourse.Replace("{{eventsDateFrom}}", JRDateTimeConvert.ConvertToJRDateTimeFormat(DateTime.Now));
                endPointForCourse = endPointForCourse.Replace("{{eventsDateTo}}", JRDateTimeConvert.ConvertToJRDateTimeFormat(DateTime.Now));
                var responseStream = await _httpClient.GetAsync(endPointForCourse);
                var response = await responseStream.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(response);
                var json = JsonConvert.SerializeXNode(doc.Descendants("events").FirstOrDefault());
                var eventRoot = JsonConvert.DeserializeObject<EventsRoot>(json);
                return eventRoot.events;
            }
            catch (Exception ex)
            {
                var error = new ExceptionModel { ErrorCode = "EventError:001", ErrorMessage = ex.Message };
                return new Events();
            }
        }


        public async Task<Event> GetEvent(string eventId)
        {
            try
            {
                var endPointForCourse = _settings.EndPoints.Event;
                endPointForCourse = endPointForCourse.Replace("{{eventId}}", eventId);
                var responseStream = await _httpClient.GetAsync(endPointForCourse);
                var response = await responseStream.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(response);
                var json = JsonConvert.SerializeXNode(doc.Descendants("event").FirstOrDefault());
                var eventRoot = JsonConvert.DeserializeObject<EventRoot>(json);
                return eventRoot.Event;
            }
            catch (Exception ex)
            {
                var error = new ExceptionModel { ErrorCode = "EventError:001", ErrorMessage = ex.Message };
                return new Event();
            }
        }

        public async Task<InviteeRoot> GetAllInviteesForTheEvent(string courseId, string eventId)
        {
            try
            {
                var endPointForCourse = _settings.EndPoints.PartiesInThatCourseEvent;
                endPointForCourse = endPointForCourse.Replace("{{courseId}}", courseId);
                endPointForCourse = endPointForCourse.Replace("{{eventId}}", eventId);
                var responseStream = await _httpClient.GetAsync(endPointForCourse);
                var response = await responseStream.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(response);
                var json = JsonConvert.SerializeXNode(doc.Descendants("invitees").FirstOrDefault());
                return JsonConvert.DeserializeObject<InviteeRoot>(json);
            }
            catch (Exception ex)
            {
                var error = new ExceptionModel { ErrorCode = "EventError:002", ErrorMessage = ex.Message };
                return new InviteeRoot();
            }
        }

        public async Task<Attendees> GetEventAttendees(string courseId, string eventId)
        {
            try
            {
                var endPointForCourse = _settings.EndPoints.Attendees;
                endPointForCourse = endPointForCourse.Replace("{{courseId}}", courseId);
                endPointForCourse = endPointForCourse.Replace("{{eventId}}", eventId);
                var responseStream = await _httpClient.GetAsync(endPointForCourse);
                var response = await responseStream.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(response);
                var json = JsonConvert.SerializeXNode(doc.Descendants("attendees").FirstOrDefault());
                return JsonConvert.DeserializeObject<AttendeesRoot>(json).Attendees;
            }
            catch (Exception ex)
            {
                var error = new ExceptionModel { ErrorCode = "EventError:003", ErrorMessage = ex.Message };
                return new Attendees();
            }
        }

        public async Task<string> GetEventAttendeesAsXML(string courseId, string eventId)
        {
            try
            {
                var endPointForCourse = _settings.EndPoints.Attendees;
                endPointForCourse = endPointForCourse.Replace("{{courseId}}", courseId);
                endPointForCourse = endPointForCourse.Replace("{{eventId}}", eventId);
                var responseStream = await _httpClient.GetAsync(endPointForCourse);
                var response = await responseStream.Content.ReadAsStringAsync();
                return response;
            }
            catch (Exception ex)
            {
                var error = new ExceptionModel { ErrorCode = "EventError:003", ErrorMessage = ex.Message };
                return "";
            }
        }


        public async Task<Attendee> MarkAttendance(string courseNumber, string eventId, Attendee attendee)
        {


            try
            {
                Attendees attendeeRoot = new Attendees();
                attendeeRoot.Attendee = new List<Attendee>();
                attendeeRoot.Attendee.Add(attendee);
                var json = JsonConvert.SerializeObject(attendeeRoot);
                XDocument doc = JsonConvert.DeserializeXNode(json);
                doc.Descendants().Where(x => x.IsEmpty || string.IsNullOrEmpty(x.Value)).Remove();
                var httpContent = new StringContent(doc.ToString(),Encoding.UTF8,"application/xml");

                var endPoint = _settings.EndPoints.Attendees;
                endPoint = endPoint.Replace("{{courseId}}", courseNumber);
                endPoint = endPoint.Replace("{{eventId}}", eventId);
                //_httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/xml"));

                var responseStream = await _httpClient.PostAsync(endPoint, httpContent);

                var response = await responseStream.Content.ReadAsStringAsync();
                var responseDoc = XDocument.Parse(response);
                var jsonFromResponse = JsonConvert.SerializeXNode(responseDoc.Descendants("attendee").FirstOrDefault());
                return JsonConvert.DeserializeObject<Attendees>(jsonFromResponse).Attendee.First();
            }
            catch (Exception ex)
            {

                var stacktrace = ex.StackTrace;
            }

            return new Attendee();

        }
    }
}
