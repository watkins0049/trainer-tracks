using System;
using System.Collections.Generic;
using System.Security.Claims;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrainerTracks.Web.Data.Model.DTO.Account;

namespace TrainerTracks.Web.Test.Controller.IT
{
    public class UserClaimsDtoJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(UserClaimsDTO));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            JToken claimTokens = jo["claims"];
            List<Claim> claims = new List<Claim>();
            foreach(JToken token in claimTokens)
            {
                string type = (string)token["type"];
                string value = (string)token["value"];
                string valueType = (string)token["valueType"];
                string issuer = (string)token["issuer"];
                string originalIssuer = (string)token["originalIssuer"];
                Claim claim = new Claim(type, value, valueType, issuer, originalIssuer);
                claims.Add(claim);
            }

            return new UserClaimsDTO
            {
                Claims = claims,
                Token = (string)jo["token"]
            };
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
