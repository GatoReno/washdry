namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class GooglePlaces
    {
        [JsonProperty("html_attributions")]
        public object[] HtmlAttributions { get; set; }

        [JsonProperty("results")]
        public Result[] Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("business_status")]
        public BusinessStatus BusinessStatus { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("icon")]
        public Uri Icon { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("opening_hours", NullValueHandling = NullValueHandling.Ignore)]
        public OpeningHours OpeningHours { get; set; }

        [JsonProperty("photos")]
        public Photo[] Photos { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("plus_code")]
        public PlusCode PlusCode { get; set; }

        [JsonProperty("price_level", NullValueHandling = NullValueHandling.Ignore)]
        public long? PriceLevel { get; set; }

        [JsonProperty("rating")]
        public double Rating { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("scope")]
        public Scope Scope { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }

        [JsonProperty("user_ratings_total")]
        public long UserRatingsTotal { get; set; }

        [JsonProperty("vicinity")]
        public string Vicinity { get; set; }

        [JsonProperty("permanently_closed", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PermanentlyClosed { get; set; }
    }

    public partial class Geometry
    {
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("viewport")]
        public Viewport Viewport { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }

    public partial class Viewport
    {
        [JsonProperty("northeast")]
        public Location Northeast { get; set; }

        [JsonProperty("southwest")]
        public Location Southwest { get; set; }
    }

    public partial class OpeningHours
    {
        [JsonProperty("open_now")]
        public bool OpenNow { get; set; }
    }

    public partial class Photo
    {
        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("html_attributions")]
        public string[] HtmlAttributions { get; set; }

        [JsonProperty("photo_reference")]
        public string PhotoReference { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }
    }

    public partial class PlusCode
    {
        [JsonProperty("compound_code")]
        public string CompoundCode { get; set; }

        [JsonProperty("global_code")]
        public string GlobalCode { get; set; }
    }

    public enum BusinessStatus { ClosedTemporarily, Operational };

    public enum Scope { Google };

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                BusinessStatusConverter.Singleton,
                ScopeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class BusinessStatusConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(BusinessStatus) || t == typeof(BusinessStatus?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "CLOSED_TEMPORARILY":
                    return BusinessStatus.ClosedTemporarily;
                case "OPERATIONAL":
                    return BusinessStatus.Operational;
            }
            throw new Exception("Cannot unmarshal type BusinessStatus");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (BusinessStatus)untypedValue;
            switch (value)
            {
                case BusinessStatus.ClosedTemporarily:
                    serializer.Serialize(writer, "CLOSED_TEMPORARILY");
                    return;
                case BusinessStatus.Operational:
                    serializer.Serialize(writer, "OPERATIONAL");
                    return;
            }
            throw new Exception("Cannot marshal type BusinessStatus");
        }

        public static readonly BusinessStatusConverter Singleton = new BusinessStatusConverter();
    }

    internal class ScopeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Scope) || t == typeof(Scope?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "GOOGLE")
            {
                return Scope.Google;
            }
            throw new Exception("Cannot unmarshal type Scope");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Scope)untypedValue;
            if (value == Scope.Google)
            {
                serializer.Serialize(writer, "GOOGLE");
                return;
            }
            throw new Exception("Cannot marshal type Scope");
        }

        public static readonly ScopeConverter Singleton = new ScopeConverter();
    }
}
