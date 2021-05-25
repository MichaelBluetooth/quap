using System;
using System.Collections.Generic;

namespace Quap.Models.DTO
{
    public class QuestionSearchResults
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int totalPages { get; set; }
        public int totalCount { get; set; }
        public bool hasPrevious => pageNumber > 1;
        public bool hasNext => pageNumber < totalPages;

        public ICollection<QuestionSummary> questions { get; set; }

        public QuestionSearchResults(List<QuestionSummary> questions, int totalCount, int pageNumber, int pageSize)
        {
            this.totalCount = totalCount;
            this.pageSize = pageSize;
            this.pageNumber = pageNumber;
            this.totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            this.questions = questions;
        }
    }
}