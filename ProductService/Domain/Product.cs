using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Domain
 {
    public class Product
     {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Image { get; private set; }
        public string Description { get; private set; }
        public IList<Cover> Covers { get; private set; }
        public IList<Question> Questions { get; private set; }
        public int MaxNumberOfInsured { get; private set; }

        private Product()
        { }

        public Product(string code, string name, string image, string description, int maxNumberOfInsured)
        {
            Id = Guid.NewGuid();
            Code = code;
            Name = name;
            Image = image;
            Description = description;
            Covers = new List<Cover>();
            Questions = new List<Question>();
            MaxNumberOfInsured = maxNumberOfInsured;
        }

        public void AddCover(string code, string name, string description, bool optional, decimal? sumInsured)
        {
            Covers.Add(new Cover(code, name, description, optional, sumInsured));
        }

        public void AddQuestions(IList<Question> questions)
        {
            foreach (var q in questions)
                Questions.Add(q);
        }        
    }
}
