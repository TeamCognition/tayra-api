using System;
using System.Text.Json;
using Cog.Core;
using Newtonsoft.Json;
using Tayra.Analytics;
using Tayra.Common;

namespace Tayra.Services
{
    public class MetricValue
    {
        [System.Text.Json.Serialization.JsonIgnore]
        [JsonIgnore]
        public MetricType Type { get; set; }
        public DatePeriod Period { get; set; }
        public float Value { get; set; }

        public MetricValue(MetricType metricType, DatePeriod period, MetricShard[] raws, EntityTypes entityType)
        {
            this.Type = metricType;
            this.Period = period;
            this.Value = entityType == EntityTypes.Profile ? metricType.Calc(raws, period) : metricType.CalcGroup(raws, period);
        }

        protected MetricValue(float value, int dateId, MetricType type)
        {
            this.Type = type;
            this.Period = new DatePeriod(dateId, dateId);
            this.Value = value;
        }
    }

    public class MetricValueConverter : System.Text.Json.Serialization.JsonConverter<MetricValue>
    {
        public override MetricValue
            Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            throw new NotImplementedException();


        public override void Write(Utf8JsonWriter writer, MetricValue value, JsonSerializerOptions options) =>
            writer.WriteNumber(nameof(value.Value), value.Value);
    }
}