using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VisitorRegistrationApp.Models;

namespace VisitorRegistrationApp.Data.Helper
{
    public class CompanyViewModelListConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new System.NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IEnumerable<CompanyViewModel>).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject item = JObject.Load(reader);

                if (item["companies"] != null)
                {
                    var companies = item["companies"].ToObject<IEnumerable<CompanyViewModel>>(serializer);


                    return companies;
                }
            }
            else
            {
                JArray array = JArray.Load(reader);

                var user = array.ToObject<IEnumerable<CompanyViewModel>>();

                return user;
            }

            throw new JsonSerializationException("Couldn't convert the object to a list companyviewmodels");


        }


    }
}
