using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProductService.Api.Queries.Dtos;

namespace ProductService.Api.Queries.Converters
{
    class QuestionDtoConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof(QuestionDto));
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
            if (value is QuestionDto questionAnswer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("index");
                serializer.Serialize(writer, questionAnswer.Index);
                writer.WritePropertyName("questionCode");
                serializer.Serialize(writer, questionAnswer.QuestionCode);
                writer.WritePropertyName("questionType");
                serializer.Serialize(writer, questionAnswer.QuestionType);
                writer.WritePropertyName("text");
                serializer.Serialize(writer, questionAnswer.Text);
                if (questionAnswer is ChoiceQuestionDto choiceQuestion)
                {
                    writer.WritePropertyName("choices");
                    serializer.Serialize(writer, choiceQuestion.Choices);
                }
                writer.WriteEndObject();
            }
        }

        private static QuestionDto Create(JObject jsonObject)
        {
            // examine the $type value
            var typeName = Enum.Parse<QuestionType>(jsonObject["questionType"].ToString());
            switch (typeName)
            {
                case QuestionType.Date:
                    return new DateQuestionDto
                    {
                        QuestionCode = jsonObject["questionCode"].ToString(),
                        Text = jsonObject["text"].ToString()
                    };
                case QuestionType.Numeric:
                    return new NumericQuestionDto
                    {
                        QuestionCode = jsonObject["questionCode"].ToString(),
                        Text = jsonObject["text"].ToString()
                    };
                case QuestionType.Choice:
                    return new ChoiceQuestionDto
                    {
                        QuestionCode = jsonObject["questionCode"].ToString(),
                        Text = jsonObject["text"].ToString()
                    };
                default:
                    throw new ApplicationException($"Unexpected question type {typeName}");
            }
        }
    }
}
