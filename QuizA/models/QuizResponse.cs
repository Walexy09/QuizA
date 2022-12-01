using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizA.models
{
    class QuizResponse
    {
        public int response_code { get; set; }

        public List<Quiz> results { get; set; }

    }
}
