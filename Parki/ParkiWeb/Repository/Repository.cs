using Newtonsoft.Json;
using ParkiWeb.Repository.Contract;
using Service.Library.ServiceResponder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkiWeb.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _httpClient;
       
        public Repository(IHttpClientFactory httpClient)
        {         
            _httpClient = httpClient;
        }

        public async Task<bool> CreateAsync(string url, T ModelToCreate)
        {
            //First create a new request object with POST to call Create
            var _request = new HttpRequestMessage(HttpMethod.Post, url);

            // now lets check if are not sending null object to our api
            if (ModelToCreate != null)
            {
                //Lets appned required content to our request and serlise our data to send it over API, add encoding and application type
                _request.Content = new StringContent(JsonConvert.SerializeObject(ModelToCreate), Encoding.UTF8, "application/json");
            }
            else // If object is null
            {
                return false;
            }

            //set up client to send request
            var _client = _httpClient.CreateClient();

            //set up var to recieve responce
            HttpResponseMessage responseMessage = await _client.SendAsync(_request);

            //let checks what we got in responce from API
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string url, int Id)
        {
            //First create a new request object with Delete 
            var _request = new HttpRequestMessage(HttpMethod.Delete, url + Id);

            //set up client to send request
            var _client = _httpClient.CreateClient();

            //set up var to recieve responce
            HttpResponseMessage responseMessage = await _client.SendAsync(_request);

            //let checks what we got in responce from API
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<IEnumerable<T>> GetAllAsync(string url)
        {
            //First create a new request object with Get 
            var _request = new HttpRequestMessage(HttpMethod.Get, url);

            //set up client to send request
            var _client = _httpClient.CreateClient();

            //set up var to recieve responce
            HttpResponseMessage responseMessage = await _client.SendAsync(_request);

            //let checks what we got in responce from API
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Get response content 
                var jsonString = await responseMessage.Content.ReadAsStringAsync();

                //convert responce content to Enumerable of type T by Deserialize it
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<TServiceResponce> GetAllAsyncServiceWrapper<TServiceResponce>(string url)
        { //https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-5-0

            try
            {
                //First create a new request object with Get 
                //var _request = new HttpRequestMessage(HttpMethod.Get, url+ "GetWithWarpper");
                //First create a new request object with Get 
                var _request = new HttpRequestMessage(HttpMethod.Get, url );

                //set up client to send request
                var _client = _httpClient.CreateClient();

                //set up var to recieve responce
                HttpResponseMessage responseMessage = await _client.SendAsync(_request);

                responseMessage.EnsureSuccessStatusCode();
                //Get response content 
                var jsonString = await responseMessage.Content.ReadAsStringAsync();

                //convert responce content to Enumerable of type T by Deserialize it
                var apiResponse = JsonConvert.DeserializeObject<TServiceResponce>(jsonString);
                return apiResponse;


            }
            catch (Exception e)
            {

                ServiceResponse<TServiceResponce> serviceResponseDto = new()
                {
                    Message = "Error",
                    Success = false,
                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                };

                var res = JsonConvert.SerializeObject(serviceResponseDto);
                var apiResponseDto = JsonConvert.DeserializeObject<TServiceResponce>(res);
                return apiResponseDto;
            }
        }


        public async Task<T> GetAsync(string url, int Id)
        {
            //First create a new request object with Get
            var _request = new HttpRequestMessage(HttpMethod.Get, url + Id);

            //set up client to send a request
            var _client = _httpClient.CreateClient();

            //revieve response
            HttpResponseMessage responseMessage = await _client.SendAsync(_request);

            //Lets check what we got in response
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Get content
                var jsonString = await responseMessage.Content.ReadAsStringAsync();

                //Convert response content to of type T by deserlising it
                return JsonConvert.DeserializeObject<T>(jsonString);

            }
            else
            {
                return null;
            }


        }


        public async Task<TServiceResponce> GetAsyncServiceWrapper<TServiceResponce>(string url, int Id)
        {

            try
            {
                //First create a new request object with Get 
                var _request = new HttpRequestMessage(HttpMethod.Get, url + "GetWithWarpper" +Id);

                //set up client to send request
                var _client = _httpClient.CreateClient();

                //set up var to recieve responce
                HttpResponseMessage responseMessage = await _client.SendAsync(_request);

                responseMessage.EnsureSuccessStatusCode();
                //Get response content 
                var jsonString = await responseMessage.Content.ReadAsStringAsync();

                //convert responce content to Enumerable of type T by Deserialize it
                var apiResponse = JsonConvert.DeserializeObject<TServiceResponce>(jsonString);
                return apiResponse;


            }
            catch (Exception e)
            {

                ServiceResponse<T> serviceResponseDto = new()
                {
                    Message = "Error",
                    Success = false,
                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                };

                var res = JsonConvert.SerializeObject(serviceResponseDto);
                var apiResponseDto = JsonConvert.DeserializeObject<TServiceResponce>(res);
                return apiResponseDto;
            }
        }

        public async Task<bool> UpdateAsync(string url, T ModelToUpdate)
        {
            //First create a new request object with ptach
            var _request = new HttpRequestMessage(HttpMethod.Patch, url);

            //Let check if we are not sening a empty object
            if (ModelToUpdate != null)
            {
                //Add required object to our request and serlise the object, send it over API, add encoding and application type
                _request.Content = new StringContent(JsonConvert.SerializeObject(ModelToUpdate), Encoding.UTF8, "application/json");
            }
            else // if object is null
            {
                return false;
            }

            // we can set up a client now as we have our request set up with Url + Content object we want to send
            var _client = _httpClient.CreateClient();

            //lets send the request and recieve a response
            HttpResponseMessage responseMessage = await _client.SendAsync(_request);

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }




}
