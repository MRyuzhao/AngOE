using System;
using System.Collections.Generic;

namespace AngOE.Logic.ViewModels
{
    public class PagedCollection<T> where T : class
    {
        public const int PerPageResults = 10;
        private const int MaxShouldBeDisplayedNumberOfPages = 5;
        public IEnumerable<T> Results { get; }
        public PagedCollection(int pageNumber, int totalRecordes, IEnumerable<T> results)
        {
            FirstPage = 1;
            CurrentPage = pageNumber;
            TotalRecordes = totalRecordes;
            if (totalRecordes == 0)
            {
                LastPage = 0;
            }
            else
            {
                LastPage = Math.Max(1, (totalRecordes + (PerPageResults - 1)) / PerPageResults);
            }

            CalculateShouldBeDisplayedStartPageAndEndPage();
            Results = results;
        }

        private void CalculateShouldBeDisplayedStartPageAndEndPage()
        {
            if (LastPage <= MaxShouldBeDisplayedNumberOfPages)
            {
                StartPage = FirstPage;
                EndPage = LastPage;
            }
            else
            {
                var numberOfLeftPartPages = MaxShouldBeDisplayedNumberOfPages / 2;
                StartPage = (CurrentPage - numberOfLeftPartPages) < FirstPage ? FirstPage : (CurrentPage - numberOfLeftPartPages);
                if (StartPage + MaxShouldBeDisplayedNumberOfPages > LastPage + 1)
                {
                    StartPage = LastPage - MaxShouldBeDisplayedNumberOfPages + 1;
                }
                EndPage = StartPage + MaxShouldBeDisplayedNumberOfPages - 1;
            }
        }

        public bool ShowFirstPage => StartPage > FirstPage;
        public bool ShowLastPage => EndPage < LastPage;
        public int FirstPage { get; set; }
        public int LastPage { get; set; }
        public int StartPage { get; set; }
        public int TotalRecordes { get; set; }

        public List<int> PageRange
        {
            get
            {
                var range = new List<int>();
                for (int i = StartPage; i <= EndPage; i++)
                {
                    range.Add(i);
                }

                return range;
            }
        }
        public int CurrentPage { get; set; }
        public int EndPage { get; set; }

    }
}