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
    public class VisitorViewModelConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new System.NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(VisitorViewModel).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject item = JObject.Load(reader);

                if (item["visitor"] != null)
                {
                    var user = item["visitor"].ToObject<VisitorViewModel>(serializer);


                    return user;
                }
            }
            else
            {
                JArray array = JArray.Load(reader);

                var user = array.ToObject<VisitorViewModel>();

                return user;
            }

            throw new JsonSerializationException("Couldn't convert the object to a visitorviewmodel");


        }


    }
}
