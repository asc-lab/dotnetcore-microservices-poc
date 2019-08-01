using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.Api.Queries.Dtos
{
    public class ChoiceDto
    {
        public string Code { get; set; }
        public string Label { get; set; }
    }
}
