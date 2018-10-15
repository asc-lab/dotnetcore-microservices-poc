using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PolicyService.Api.Commands.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolicyService.Api.Converters
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
            var target = Create(objectType, jsonObject);
            serializer.Populate(jsonObject.CreateReader(), target);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var questionAnswer = value as QuestionAnswer;
            if (questionAnswer != null)
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

        QuestionAnswer Create(Type objectType, JObject jsonObject)
        {
            // examine the $type value
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
