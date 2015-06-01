using Webdiyer.WebControls.Mvc;

namespace Webdiyer.MvcPagerDemo.Models
{
    public class CompositeArticles
    {
        public PagedList<Article> ArticleList1 { get; set; }
        public PagedList<Article> ArticleList2 { get; set; }
        public PagedList<Article> ArticleList3 { get; set; } 
    }
}