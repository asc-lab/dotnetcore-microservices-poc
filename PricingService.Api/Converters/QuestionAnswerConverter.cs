using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PricingService.Api.Commands.Dto;

namespace PricingService.Api.Converters
{
    class QuestionAnswerConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof(QuestionAnswer));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var target = Create(jsonObject);
            serializer.Populate(jsonObject.CreateReader(), target);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is QuestionAnswer questionAnswer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("questionCode");
                serializer.Serialize(writer, questionAnswer.QuestionCode);
                writer.WritePropertyName("questionType");
                serializer.Serialize(writer, questionAnswer.QuestionType);
                writer.WritePropertyName("answer");
                serializer.Serialize(writer, questionAnswer.GetAnswer());
                writer.WriteEndObject();
            }
            
        }

        private static QuestionAnswer Create(JObject jsonObject)
        {
            var typeName = Enum.Parse<QuestionType>(jsonObject["questionType"].ToString());
            switch (typeName)
            {
                case QuestionType.Text:
                    return new TextQuestionAnswer
                    {
                        QuestionCode = jsonObject["questionCode"].ToString(),
                        Answer = jsonObject["answer"].ToString()
                    };
                case QuestionType.Number:
                    return new NumericQuestionAnswer
                    {
                        QuestionCode = jsonObject["questionCode"].ToString(),
                        Answer = jsonObject["answer"].Value<decimal>()
                    };
                case QuestionType.Choice:
                    return new ChoiceQuestionAnswer
                    {
                        QuestionCode = jsonObject["questionCode"].ToString(),
                        Answer = jsonObject["answer"].ToString()
                    };
                default:
                    throw new ApplicationException($"Unexpected question type {typeName}");
            }
        }
    }
}
