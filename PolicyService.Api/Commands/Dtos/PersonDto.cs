using System;
using System.Collections.Generic;
using System.Text;

namespace PolicyService.Api.Commands.Dtos
{
    public class PersonDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TaxId { get; set; }
    }
}
