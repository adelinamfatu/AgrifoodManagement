using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class SentimentData
    {
        [LoadColumn(0), ColumnName("Comment")]
        public string Comment { get; set; }

        [LoadColumn(1), ColumnName("Label")]
        public string Label { get; set; }
    }
}
