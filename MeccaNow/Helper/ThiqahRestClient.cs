using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MeccaNow.Helper
{
    /// <summary>
    /// This will define the authentication used within the request
    /// </summary>
    public enum AuthenticationMode
    {
        None,
        Basic,
        Token
    }

    /// <summary>
    /// This will define the language of the return massage on the APIException to be Arabic or English.
    /// </summary>
    public enum AcceptLanguage
    {
        ar, en
    }

    /// <summary>
    /// This class will define the extra headers you need to add to the request header.
    /// </summary>
    public class RequestHeader
    {
        /// <summary>
        /// This is the name of the header you need to add it to the header.
        /// </summary>
        public string HeaderName { get; set; }
        /// <summary>
        /// This is the value of the header you need to add it to the header.
        /// </summary>
        public string HeaderValue { get; set; }
    }

    /// <summary>
    /// This class will map to one of the headers returned by the response
    /// </summary>
    public class ResponseHeader
    {
        /// <summary>
        /// This is the name of the header you need to add it to the header.
        /// </summary>
        public string HeaderName { get; set; }
        /// <summary>
        /// This is the value of the header you need to add it to the header.
        /// </summary>
        public List<string> HeaderValues { get; set; }

        public ResponseHeader()
        {
            HeaderValues = new List<string>();
        }
    }

    /// <summary>
    /// This class will convert the response of the API to the specified type T
    /// </summary>
    public class APIResponse<T>
    {
        /// <summary>
        /// A generic type parameters that the client specifies in order to convert the returned json response content to the specified type.
        /// </summary>
        public T Response;
        /// <summary>
        /// This list will define all headers returned by response.
        /// </summary>
        public List<ResponseHeader> ResponseHeaderList;
    }

    /// <summary>
    /// This class is defining the error information return for the API.
    /// </summary>
    public class APIError
    {
        /// <summary>
        /// The ErrorCode property contains the error code that is associated with the error that caused the exception.
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// The ErrorMessage property contains the massage that is associated with the error that caused the exception based on the langauge defind in the AcceptLanguage.
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// When the error is a validation error the API will return a list error that associate with the validation.
        /// </summary>
        public List<ValidationError> ValidationList { get; set; }
    }

    /// <summary>
    /// This class is defining the validation error information return for the API.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// The ErrorCode property contains the error code that is associated with the validation error that caused the exception.
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// The ErrorMessage property contains the massage that is associated with the validation error that caused the exception based on the langauge defind in the AcceptLanguage.
        /// </summary>
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// APIException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299
    /// </summary>
    [Serializable]
    public class APIException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public APIError APIError { get; set; }

        public List<ResponseHeader> ResponseHeaderList;

        public APIException()
        { }

        protected APIException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }

    /// <summary>
    /// GeneralException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299
    /// </summary>
    [Serializable]
    public class GeneralException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public string APIErrorJson { get; set; }

        public List<ResponseHeader> ResponseHeaderList;
        public GeneralException()
        {

        }

        protected GeneralException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }

    /// <summary>
    /// This library is created by Thiqah Integration team in order to simplfiy the calling to Rest Services.
    /// Version 1.0.1
    /// </summary>
    public class ThiqahRestClient
    {
        private string BaseURL { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        private string Token { get; set; }
        private AuthenticationMode AuthenticationMode { get; set; }
        private AcceptLanguage AcceptLanguage { get; set; }
        private List<RequestHeader> CustomHeaderList { get; set; }
        private bool generateAPIException { get; set; }

        /// <summary>
        /// Initializes a new instance of the ThiqahRestClient using None Authentication mode
        /// </summary>
        /// <param name="baseURL">The base URL used for all operations</param>
        /// <param name="acceptLanguage">The Accept-Language request HTTP header to determine which language the client uses</param>
        /// <param name="generateAPIException">if true the service return APIEception if false it will return a GeneralException</param>
        public ThiqahRestClient(string baseURL, AcceptLanguage acceptLanguage, bool generateAPIException)
        {
            this.BaseURL = baseURL;
            this.AcceptLanguage = acceptLanguage;
            this.generateAPIException = generateAPIException;
            this.AuthenticationMode = AuthenticationMode.None;
        }

        /// <summary>
        /// Initializes a new instance of the ThiqahRestClient using None Authentication mode
        /// </summary>
        /// <param name="baseURL">The base URL used for all operations</param>
        /// <param name="acceptLanguage">The Accept-Language request HTTP header to determine which language the client uses</param>
        /// <param name="generateAPIException">if true the service return APIEception if false it will return a GeneralException</param>
        /// <param name="customHeaderList">This is a list will define the extra headers need to be added to the request header.</param>
        public ThiqahRestClient(string baseURL, AcceptLanguage acceptLanguage, bool generateAPIException, List<RequestHeader> customHeaderList) : this(baseURL, acceptLanguage, generateAPIException)
        {
            this.CustomHeaderList = customHeaderList;
        }

        /// <summary>
        /// Initializes a new instance of the ThiqahRestClient using Basic Authentication
        /// </summary>
        /// <param name="baseURL">The base URL used for all operations</param>
        /// <param name="acceptLanguage">The Accept-Language request HTTP header to determine which language the client uses</param>
        /// <param name="username">An identification used by the client to access the API</param>
        /// <param name="password">A password is a word or string of characters used for authentication to prove identity or access approval to gain access to the API. which is to be kept secret from those not allowed access.</param>
        /// <param name="generateAPIException">if true the service return APIEception if false it will return a GeneralException</param>
        public ThiqahRestClient(string baseURL, AcceptLanguage acceptLanguage, string username, string password, bool generateAPIException)
        {
            this.BaseURL = baseURL;
            this.AcceptLanguage = AcceptLanguage;
            this.Username = username;
            this.Password = password;
            this.AuthenticationMode = AuthenticationMode.Basic;
            this.generateAPIException = generateAPIException;
        }

        /// <summary>
        /// Initializes a new instance of the ThiqahRestClient using Basic Authentication
        /// </summary>
        /// <param name="baseURL">The base URL used for all operations</param>
        /// <param name="acceptLanguage">The Accept-Language request HTTP header  to determine which language the client use</param>
        /// <param name="username">An identification used by the client to access the API</param>
        /// <param name="password">A password is a word or string of characters used for authentication to prove identity or access approval to gain access to the API. which is to be kept secret from those not allowed access.</param>
        /// <param name="customHeaderList">This is a list will define the extra headers need to be added to the request header.</param>
        /// <param name="generateAPIException">if true the service return APIEception if false it will return a GeneralException</param>
        public ThiqahRestClient(string baseURL, AcceptLanguage acceptLanguage, string username, string password, bool generateAPIException, List<RequestHeader> customHeaderList) : this(baseURL, acceptLanguage, username, password, generateAPIException)
        {
            this.CustomHeaderList = customHeaderList;
        }

        /// <summary>
        /// Initializes a new instance of the ThiqahRestClient using Token based authentication
        /// </summary>
        /// <param name="baseURL">The base URL used for all operations</param>
        /// <param name="acceptLanguage">The Accept-Language request HTTP header  to determine which language the client use</param>
        /// <param name="token">API token is a unique identifier of an application requesting access to your service.</param>
        /// <param name="generateAPIException">if true the service return APIEception if false it will return a GeneralException</param>
        public ThiqahRestClient(string baseURL, AcceptLanguage acceptLanguage, string token, bool generateAPIException)
        {
            this.BaseURL = baseURL;
            this.AcceptLanguage = AcceptLanguage;
            this.Token = token;
            this.AuthenticationMode = AuthenticationMode.Token;
            this.generateAPIException = generateAPIException;
        }

        /// <summary>
        /// Initializes a new instance of the ThiqahRestClient using Token based authentication
        /// </summary>
        /// <param name="baseURL">The base URL used for all operations</param>
        /// <param name="acceptLanguage">The Accept-Language request HTTP header  to determine which language the client use</param>
        /// <param name="token">API token is a unique identifier of an application requesting access to your service.</param>
        /// <param name="customHeaderList">This is a list will define the extra headers need to be added to the request header.</param>
        /// <param name="generateAPIException">if true the service return APIEception if false it will return a GeneralException</param>
        public ThiqahRestClient(string baseURL, AcceptLanguage acceptLanguage, string token, bool generateAPIException, List<RequestHeader> customHeaderList) : this(baseURL, acceptLanguage, token, generateAPIException)
        {
            this.CustomHeaderList = customHeaderList;
        }

        /// <summary>
        /// Send a GET request to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="pathAndQuery">The PathAndQuery property contains the path for the API resource and the query information or parameter information sent with the request.</param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        public APIResponse<T> Get<T>(string pathAndQuery)
        {
            using (var client = this.Initialize())
            {
                var response = client.GetAsync(pathAndQuery).Result;
                List<ResponseHeader> headerList = FillResponseHeaderList(response);
                string content = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    throw GenerateException(response.StatusCode, content, headerList);
                else
                    return CreateAPIResponse<T>(content, headerList);
            }
        }

        /// <summary>
        /// Send a GET request asynchronously to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="pathAndQuery">The PathAndQuery property contains the path for the API resource and the query information or parameter information sent with the request.</param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        public async Task<APIResponse<T>> GetAsync<T>(string pathAndQuery)
        {
            using (var client = this.Initialize())
            {
                var response = await client.GetAsync(pathAndQuery);
                List<ResponseHeader> headerList = FillResponseHeaderList(response);
                string content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw GenerateException(response.StatusCode, content, headerList);
                else
                    return CreateAPIResponse<T>(content, headerList);
            }
        }

        /// <summary>
        /// Send a POST request to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="path">The path property contains the path for the API resource</param>
        /// <param name="requestBody">The request body object that should be parsed as JSON <c><b>** DO NOT CONVERT THE OBJECT TO JSON **</b></c></param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        public APIResponse<T> Post<T>(string path, object requestBody)
        {
            using (var client = this.Initialize())
            {
              
                var response = client.PostAsync(path, new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json")).Result;
                List<ResponseHeader> headerList = FillResponseHeaderList(response);
                string content = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    throw GenerateException(response.StatusCode, content, headerList);
                else
                    return CreateAPIResponse<T>(content, headerList);
            }
        }

        /// <summary>
        /// Send a POST request asynchronously to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="path">The path property contains the path for the API resource</param>
        /// <param name="requestBody">The request body object that should be parsed as JSON <c><b>** DO NOT CONVERT THE OBJECT TO JSON **</b></c></param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        public async Task<APIResponse<T>> PostAsync<T>(string path, object requestBody)
        {
            using (var client = this.Initialize())
            {
                
                var response = await client.PostAsync(path, new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));
                List<ResponseHeader> headerList = FillResponseHeaderList(response);
                string content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw GenerateException(response.StatusCode, content, headerList);
                else
                    return CreateAPIResponse<T>(content, headerList);
            }
        }

        /// <summary>
        /// Send a Put request to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="path">The path property contains the path for the API resource</param>
        /// <param name="requestBody">The request body object that should be parsed as JSON <c><b>** DO NOT CONVERT THE OBJECT TO JSON **</b></c></param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        public APIResponse<T> Put<T>(string path, object requestBody)
        {
            using (var client = this.Initialize())
            {
                var response = client.PutAsync(path, new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json")).Result;
                List<ResponseHeader> headerList = FillResponseHeaderList(response);
                string content = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    throw GenerateException(response.StatusCode, content, headerList);
                else
                    return CreateAPIResponse<T>(content, headerList);
            }
        }

        /// <summary>
        /// Send a Put request asynchronously to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="path">The path property contains the path for the API resource</param>
        /// <param name="requestBody">The request body object that should be parsed as JSON <c><b>** DO NOT CONVERT THE OBJECT TO JSON **</b></c></param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        public async Task<APIResponse<T>> PutAsync<T>(string path, object requestBody)
        {
            using (var client = this.Initialize())
            {
                var response = await client.PutAsync(path, new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));
                List<ResponseHeader> headerList = FillResponseHeaderList(response);
                string content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw GenerateException(response.StatusCode, content, headerList);
                else
                    return CreateAPIResponse<T>(content, headerList);
            }
        }

        /// <summary>
        /// Send a Delete request to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="path">The path property contains the path for the API resource</param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        public APIResponse<T> Delete<T>(string path)
        {
            using (var client = this.Initialize())
            {
                var response = client.DeleteAsync(path).Result;
                List<ResponseHeader> headerList = FillResponseHeaderList(response);
                string content = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    throw GenerateException(response.StatusCode, content, headerList);
                else
                    return CreateAPIResponse<T>(content, headerList);
            }
        }

        /// <summary>
        /// Send a Delete request asynchronously to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="path">The path property contains the path for the API resource</param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        public async Task<APIResponse<T>> DeleteAsync<T>(string path)
        {
            using (var client = this.Initialize())
            {
                var response = await client.DeleteAsync(path);
                List<ResponseHeader> headerList = FillResponseHeaderList(response);
                string content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw GenerateException(response.StatusCode, content, headerList);
                else
                    return CreateAPIResponse<T>(content, headerList);
            }
        }

        #region "Private Methods"

        private HttpClient Initialize()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (this.AuthenticationMode == AuthenticationMode.Basic)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(Username + ":" + Password)));
            else if (this.AuthenticationMode == AuthenticationMode.Token)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Token);
            client.DefaultRequestHeaders.Add("Accept-Language", this.AcceptLanguage.ToString());
            if (CustomHeaderList != null)
            {
                foreach (var customHeader in CustomHeaderList)
                {
                    client.DefaultRequestHeaders.Add(customHeader.HeaderName, customHeader.HeaderValue);
                }
            }
            client.DefaultRequestHeaders.Add("X-sadad-application-key", "refund_system_key");
            return client;
        }

        private List<ResponseHeader> FillResponseHeaderList(HttpResponseMessage response)
        {
            List<ResponseHeader> headerList = new List<ResponseHeader>();

            var headers = response.Headers.ToList();
            foreach (var header in headers)
            {
                var apiHeader = new ResponseHeader();
                IEnumerable<string> values = null;
                if (response.Headers.TryGetValues(header.Key, out values))
                {
                    apiHeader.HeaderName = header.Key;
                    apiHeader.HeaderValues = values.ToList();
                    headerList.Add(apiHeader);
                }
            }

            return headerList;
        }

        private APIResponse<T> CreateAPIResponse<T>(string content, List<ResponseHeader> responseHeaderList)
        {
            APIResponse<T> apiResponse = new APIResponse<T>();
            apiResponse.ResponseHeaderList = responseHeaderList;
            apiResponse.Response = JsonConvert.DeserializeObject<T>(content);
            return apiResponse;
        }

        private APIException GenerateException(HttpStatusCode httpStatusCode, string content, List<ResponseHeader> responseHeaderList)
        {
            if (this.generateAPIException)
            {
                var apiError = JsonConvert.DeserializeObject<APIError>(content);
                return new APIException { HttpStatusCode = httpStatusCode, APIError = apiError, ResponseHeaderList = responseHeaderList };
            }
            else
                throw new GeneralException { HttpStatusCode = httpStatusCode, APIErrorJson = content, ResponseHeaderList = responseHeaderList };
        }

        #endregion
    }
}
