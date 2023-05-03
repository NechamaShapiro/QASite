using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QASite.Data
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<QuestionsTags> QuestionsTags { get; set; }
    }
}
