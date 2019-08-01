using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnsureThat;

namespace ProductService.Domain
 {
    public class Product
     {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public ProductStatus Status { get; private set; }
        public string Image { get; private set; }
        public string Description { get; private set; }
        public IList<Cover> Covers { get; private set; }
        public IList<Question> Questions { get; private set; }
        public int MaxNumberOfInsured { get; private set; }

        private Product()
        { }

        protected Product(string code, string name, string image, string description, int maxNumberOfInsured)
        {
            Id = Guid.NewGuid();
            Code = code;
            Name = name;
            Status = ProductStatus.Draft;
            Image = image;
            Description = description;
            Covers = new List<Cover>();
            Questions = new List<Question>();
            MaxNumberOfInsured = maxNumberOfInsured;
        }

         public static Product CreateDraft(string code, string name, string image, string description, int maxNumberOfInsured)
         {
             return new Product(code,name,image,description,maxNumberOfInsured);
         }

         public void Activate()
         {
             Ensure.That(Status).Equals(ProductStatus.Draft);
             Status = ProductStatus.Active;
         }

         public void Discontinue()
         {
             Status = ProductStatus.Discontinued;
         }

         public void AddCover(string code, string name, string description, bool optional, decimal? sumInsured)
         {
             Ensure.That(Status).Equals(ProductStatus.Draft);
             Covers.Add(new Cover(code, name, description, optional, sumInsured));
         }
        
        public void AddQuestions(IList<Question> questions)
        {
            Ensure.That(Status).Equals(ProductStatus.Draft);
            foreach (var q in questions)
                Questions.Add(q);
        }        
    }

    public enum ProductStatus
    {
     Draft,
     Active,
     Discontinued
    }
 }
