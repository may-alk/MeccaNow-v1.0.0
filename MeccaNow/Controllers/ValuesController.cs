using MeccaNow.Helper;
using MeccaNow.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace MeccaNow.Controllers
{
    public class ValuesController : ApiController
    {

        protected string googleplus_client_id = "927426634901-1bi5bj4pmfgp9jvip73ljpnlr4vdvq4o.apps.googleusercontent.com";  
        protected string googleplus_client_sceret = "PX_fakVjZ_w_wORXRnXxYuDG";                                              
        protected string googleplus_redirect_url = "https://www.google.com";
        protected string _serviceURL = "https://automl.googleapis.com/v1beta1/projects/my-project-1502722499598/locations/us-central1/models/ICN7676025168801481446:predict";
        
        protected string Parameters;

        [HttpPost]

        [AllowAnonymous]
        public async Task<IHttpActionResult> CreatePred([FromBody] Request request)
        {
            //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/oauth2/v3/token");
            //webRequest.Method = "POST";
            //Parameters = "code=4/MABpU2A4MpHpAhUjYRW-HJaIYmWkaSevQ1H3pzD2K5BAGdpCTjM-VN3epO1Btkl1TJvZHxhNKaYmscFtsHY1XII#&client_id=" + googleplus_client_id + "&client_secret=" + googleplus_client_sceret + "&redirect_uri=" + googleplus_redirect_url + "&grant_type=authorization_code";
            //byte[] byteArray = Encoding.UTF8.GetBytes(Parameters);
            //webRequest.ContentType = "application/x-www-form-urlencoded";
            //webRequest.ContentLength = byteArray.Length;
            //Stream postStream = webRequest.GetRequestStream();
            //// Add the post data to the web request
            //postStream.Write(byteArray, 0, byteArray.Length);
            //postStream.Close();

            //WebResponse response = webRequest.GetResponse();
            //postStream = response.GetResponseStream();
            //StreamReader reader = new StreamReader(postStream);
            //string responseFromServer = reader.ReadToEnd();

            //GooglePlusAccessToken serStatus = JsonConvert.DeserializeObject<GooglePlusAccessToken>(responseFromServer);

            //if (serStatus != null)
            //{
            //    string accessToken = string.Empty;
            //    accessToken = serStatus.access_token;

            //    if (!string.IsNullOrEmpty(accessToken))
            //    {
                    AutomlRequest Req = null;
                    APIResponse<AutomlResponse> Res = null;



                    List<RequestHeader> ReqList = new List<RequestHeader>();
                    ReqList.Add(new RequestHeader() { HeaderName = "Authorization", HeaderValue = "Bearer ya29.GlwMBno2odeW3i8qw_EIg1jxCYpO0Njbro8JKp_TzFN-G0DwZvcnHMEwgxoOVn9dWsZVRFr4NBQ_I-TJJ28G3LNd1UUUW7DELgo3JAge-FCk9FElZoxDSbd8p5j2Gw" });
                    ThiqahRestClient thiqahRestClient = new ThiqahRestClient(_serviceURL, AcceptLanguage.en, false, ReqList);
                    Req = new AutomlRequest() { payload = new payload() { image = new image() { imageBytes = request.Image } } };
                    Res = await thiqahRestClient.PostAsync<AutomlResponse>(_serviceURL, Req);

                    DB db = new DB();
                    Li item=Res.Response.payload.First();
            string s = String.Format("{0:P2}", item.classification.score).Replace("%","");
            double d = Convert.ToDouble(s);
            long i = (int)Math.Floor(d);
                    db.CreatePred(i, item.displayName);
             //   }

          //  }
            return Ok();
        }





        [HttpGet]

        [AllowAnonymous]
        public   IHttpActionResult GetPred()
        {
            DB db = new DB();

            return Ok(db.GetPred());
        }
    }
}
