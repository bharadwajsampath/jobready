using Models.Implementations;
using System;
using System.Collections.Generic;
using System.Net.Http;


namespace Worker.Helpers
{
    public static class WebWrapper
    {
        private static HttpClient _httpClient;

        public static HttpClient GetClient(JRSettings settings)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(settings.Consumers.JobreadyURL);
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",settings.Consumers.Api);
            return _httpClient;
        }





    }


}