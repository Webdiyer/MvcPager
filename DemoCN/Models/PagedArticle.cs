using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace Webdiyer.MvcPagerDemo.Models
{
    [NotMapped]
    public class PagedArticle:Article,IPagedList<string>
    {
        private readonly IEnumerable<string> _pagedContent;
        private PagedArticle(){}
        public PagedArticle(Article art, int pageIndex)
        {
            //获取文章内容中的分页符的正则表达式，分页符通过ckeditor等富文本编辑器的“插入分页符”功能插入到文章中，html代码为“<div style="page-break-after: always"> <span style="display: none;">&nbsp;</span></div>”
            var re = new Regex("<div\\s+style=\"page-break-after:\\s*always;?\">[\\r\\n]*.*?</div>",
                               RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled |
                               RegexOptions.IgnorePatternWhitespace);
            _pagedContent = re.Split(art.Content);
            if(pageIndex>_pagedContent.Count()||pageIndex<1)
                throw new InvalidOperationException("页索引超出界限");
            ID = art.ID;
            Title = art.Title;
            Author = art.Author;
            Content = _pagedContent.ToArray()[pageIndex - 1];
            PubDate = art.PubDate;
            Source = art.Source;

            CurrentPageIndex = pageIndex;
            PageSize = 1;
            TotalItemCount = _pagedContent.Count();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _pagedContent.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int CurrentPageIndex { get; set; }
        public int PageSize { get { return 1; } set{} }
        public int TotalItemCount { get; set; }
    }
}