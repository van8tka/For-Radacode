using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using BlankVk.Model;
using Newtonsoft.Json.Linq;

namespace BlankVk
{
    public class GetDataFromVK
    {
      // https://api.vk.com/method/database.getCities?country_id=12

        const string getCoutries = "database.getCountries";
        const string getCities = "database.getCities?country_id=";
        const string getUniver = "database.getUniversities?city_id=";


        const string Url = "https://api.vk.com/method/";
        //настройки клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }
        //получаем список стран
        public async Task<IEnumerable<VKData>> GetCountries()
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url+getCoutries);
          //распарсим ответ и получим значение response 
            JObject o = JObject.Parse(result);
            JArray array = (JArray)o["response"];

         return JsonConvert.DeserializeObject<IEnumerable<VKData>>(array.ToString());
        }
        //получаем список городов
        public async Task<IEnumerable<VKData>> GetCities(int idcountry)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + getCities+idcountry.ToString());

            //распарсим ответ и получим значение response 
            JObject o = JObject.Parse(result);
            JArray array = (JArray)o["response"];

            return JsonConvert.DeserializeObject<IEnumerable<VKData>>(array.ToString());
        }
        //получаем список университетов
        public async Task<IEnumerable<VKDataUnivers>> GetUnivers(int idcity)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + getUniver + idcity.ToString());

            //распарсим ответ и получим значение response 
            JObject o = JObject.Parse(result);
            JArray array = (JArray)o["response"];

             int idpt1 = array.ToString().IndexOf(',');
            int idpt2 = array.ToString().IndexOf('[');
            string newAr = array.ToString().Remove(idpt2+1, idpt1 );
            return JsonConvert.DeserializeObject<IEnumerable<VKDataUnivers>>(newAr);
        }
    }
}
