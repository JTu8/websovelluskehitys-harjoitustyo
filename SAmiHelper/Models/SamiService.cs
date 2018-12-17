using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SAmiHelper.Models
{
    public class SamiService
    {
        private static HttpClient httpClient;

        public SamiService()
        {
            if(null == httpClient)
            {
                httpClient = new HttpClient();
                
            }
        }

        public async Task<List<SensorModel>> GetSensors(SearchModel searchModel)
        {
            if(searchModel == null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

            string url = $"sensors/{searchModel.Key}";
            string resultString;
            var result = await httpClient.GetAsync(url);
            List<SensorModel> sensors = null;
            if(result.IsSuccessStatusCode)
            {
                resultString = await result.Content.ReadAsStringAsync();
                sensors = JsonConvert.DeserializeObject<List<SensorModel>>(resultString);
                return sensors;
            }
            else
            {
                Debug.WriteLine($"Error reading sensors: HTTP result was {result.StatusCode} - {result.ReasonPhrase}");
                return null;
            }
        }

        public async Task<List<SamiMeasurementModel>> GetSamiMeasurements(SearchModel searchModel)
        {
            if (searchModel == null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

            string url = $"measurements/{searchModel.Key}";

            List<string> uriParams = new List<string>();
            if(false == string.IsNullOrEmpty(searchModel.Obj))
            {
                uriParams.Add($"obj={searchModel.Obj}");
            }
            if (false == string.IsNullOrEmpty(searchModel.Tag))
            {
                uriParams.Add($"tag={searchModel.Tag}");
            }
            if(searchModel.Take.HasValue)
            {
                uriParams.Add($"take={searchModel.Take}");
            }
            if (false == string.IsNullOrEmpty(searchModel.Sensors))
            {
                uriParams.Add($"data-tags={searchModel.Sensors}");
            }
            if(searchModel.From.HasValue)
            {
                uriParams.Add($"from={Uri.EscapeUriString(searchModel.From.Value.ToString("yyyy-MM-ddTHH:mm:ss"))}");
            }
            if (searchModel.To.HasValue)
            {
                uriParams.Add($"to={Uri.EscapeUriString(searchModel.From.Value.ToString("yyyy-MM-ddTHH:mm:ss"))}");
            }

            if(uriParams.Count > 0)
            {
                url += $"?{string.Join("&", uriParams)}";
            }

            string resultString;
            var result = await httpClient.GetAsync(url);
            List<SamiMeasurementModel> data = null;
            if (result.IsSuccessStatusCode)
            {
                resultString = await result.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<List<SamiMeasurementModel>>(resultString);
            }
            else
            {
                Debug.WriteLine($"Error reading sensors: HTTP result was {result.StatusCode} - {result.ReasonPhrase}");
                return null;
            }
            return data;
        }
    }
}
