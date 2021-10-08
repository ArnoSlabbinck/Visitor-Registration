using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VisitorRegistrationApp.Models;

namespace VisitorRegistrationApp.Helper
{
    public class VisitorViewModelListConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new System.NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IEnumerable<VisitorViewModel>).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject item = JObject.Load(reader);

                if (item["visitors"] != null)
                {
                    var users = item["visitors"].ToObject<IList<VisitorViewModel>>(serializer);


                    return new List<VisitorViewModel>(users);
                }
            }
            else
            {
                JArray array = JArray.Load(reader);

                var users = array.ToObject<IList<VisitorViewModel>>();

                return new List<VisitorViewModel>(users);
            }

            // This should not happen. Perhaps better to throw exception at this point?
            return null;
        }


    }

}
