using System;
using System.Collections.Generic;
using System.Linq;
using ProductService.Domain;
using ProductService.Test.TestData;
using Xunit;

namespace ProductService.Test.Domain;

public class ProductTest
{
    public static IEnumerable<object[]> CoversData =>
        new List<object[]>
        {
            new object[] { "C2", "Illness", "", false, 5000.0M },
            new object[] { "C3", "Assistance", "", true, null }
        };

    [Theory]
    [InlineData("TRI",
        "Safe Traveller",
        "/static/travel.jpg",
        "Travel insurance",
        10,
        "plane")]
    public void ProductDraftIsCreated(string code, string name, string image, string description,
        int maxNumberOfInsured, string icon)
    {
        var product = Product.CreateDraft(code, name, image, description, maxNumberOfInsured, icon);

        Assert.NotEqual(Guid.Empty, product.Id);
        Assert.NotNull(product.Questions);
        Assert.Empty(product.Questions);
        Assert.NotNull(product.Covers);
        Assert.Equal(ProductStatus.Draft, product.Status);
        Assert.Empty(product.Covers);
    }

    [Theory]
    [MemberData(nameof(CoversData))]
    public void Product_CoverIsAdded(string code, string name, string description, bool optional, decimal? sumInsured)
    {
        var product = TestProductFactory.EmptyTravel();

        product.AddCover(code, name, description, optional, sumInsured);

        Assert.NotEmpty(product.Covers);
        Assert.Single(product.Covers);
        Assert.NotEqual(Guid.Empty, product.Covers.First().Id);
    }

    [Fact]
    public void Product_QuestionsAreAdded()
    {
        var product = TestProductFactory.EmptyTravel();

        var testQuestions = new List<Question>
        {
            TestQuestionFactory.ChoiceQuestion(),
            TestQuestionFactory.DateQuestion(),
            TestQuestionFactory.NumericQuestion()
        };

        product.AddQuestions(testQuestions);

        Assert.NotEmpty(product.Questions);
        Assert.Equal(testQuestions.Count, product.Questions.Count);
        Assert.All(product.Questions, item => Assert.NotEqual(Guid.Empty, item.Id));
    }

    [Fact]
    public void Product_DraftCanBeActivated()
    {
        var product = TestProductFactory.InactiveTravel();

        product.Activate();

        Assert.Equal(ProductStatus.Active, product.Status);
    }

    [Fact]
    public void Product_ActiveCannotBeActivated()
    {
        var product = TestProductFactory.Travel();

        var ex = Assert.Throws<ApplicationException>(() => product.Activate());
        Assert.Equal("Only draft version can be modified and activated", ex.Message);
    }
}