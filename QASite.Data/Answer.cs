using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QASite.Data
{
    public class Answer
    {
        public int id { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
