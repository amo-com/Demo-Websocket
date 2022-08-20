using System;

namespace Service.Api.Common
{
    public static class JsonExtensions
    {
        public static T Deserialize<T>(string json)
        {
            try
            {
                Newtonsoft.Json.JsonSerializerSettings jsetting = new Newtonsoft.Json.JsonSerializerSettings();
                jsetting.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, jsetting);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object Deserialize(string json, Type type)
        {
            try
            {
                Newtonsoft.Json.JsonSerializerSettings jsetting = new Newtonsoft.Json.JsonSerializerSettings();
                jsetting.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                return Newtonsoft.Json.JsonConvert.DeserializeObject(json, type, jsetting);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string Serialize<T>(T obj)
        {
            try
            {
                Newtonsoft.Json.JsonSerializerSettings jsetting = new Newtonsoft.Json.JsonSerializerSettings();
                jsetting.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj, jsetting);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string ToJson<T>(this T self)
        {
            Newtonsoft.Json.JsonSerializerSettings jsetting = new Newtonsoft.Json.JsonSerializerSettings();
            jsetting.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            jsetting.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver { IgnoreSerializableInterface = true };
            return Newtonsoft.Json.JsonConvert.SerializeObject(self, jsetting);
        }
    }
}
