using System;
using System.Collections.Generic;
using System.Text;

namespace DinerBusinessLogic.HelperModels
{
    class WordParagraph
    {
        public List<(string, WordParagraphProperties)> Texts { get; set; }

        public WordParagraphProperties TextProperties { get; set; }
    }
}
