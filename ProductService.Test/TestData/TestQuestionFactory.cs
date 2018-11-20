using ProductService.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.Test.TestData
{
    public class TestQuestionFactory
    {
        internal static ChoiceQuestion ChoiceQuestion()
        {
            return new ChoiceQuestion("TYP", 1, "Apartment / House", new List<Choice> {
                            new Choice("APT", "Apartment"),
                            new Choice("HOUSE", "House")
                    });
        }

        internal static NumericQuestion NumericQuestion()
        {
            return new NumericQuestion("AREA", 2, "Area");
        }

        internal static DateQuestion DateQuestion()
        {
            return new DateQuestion("BIRTHDATE", 3, "Birhtdate");
        }

    }
}
