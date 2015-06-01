using System;
using System.ComponentModel.DataAnnotations;

namespace Webdiyer.MvcPagerDemo.Models
{
    public class Article
    {
        [Display(Name="文章编号")]
        public int ID { get; set; }

        [Display(Name="文章标题")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Display(Name = "文章内容")]
        public string Content { get; set; }

        [Display(Name = "发布日期")]
        public DateTime PubDate { get; set; }

        [Display(Name = "作者")]
        [MaxLength(20)]
        public string Author { get; set; }

        [Display(Name = "文章来源")]
        [MaxLength(20)]
        public string Source { get; set; }
    }
}