using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Webdiyer.WebControls.Mvc;

namespace MvcPager.Tests
{
    [TestClass]
    public class PagedListTest
    {
        [TestMethod]
        [TestCategory("PagedList")]
        public void StartRecordIndexAndEndRecordIndex_ShouldReturnCorrectValue()
        {
            var list=new PagedList<int>(Enumerable.Range(1,88),3,8);
            Assert.AreEqual(list.StartItemIndex,17);
            Assert.AreEqual(list.EndItemIndex,24);
        }

        [TestMethod]
        [TestCategory("PagedList")]
        public void PagedListItem_ShouldReturnCorrectValue()
        {
            var list = new PagedList<int>(Enumerable.Range(1, 88), 3, 8);
            Assert.AreEqual(list[0], 17);
            list = new PagedList<int>(Enumerable.Range(1, 88), 8, 8);
            Assert.AreEqual(list[0],57,"Value at 0 of PagedList is incorrect");
            Assert.AreEqual(list[7],64,"Value at 7 of PagedList is incorrect");
        }

        [TestMethod]
        [TestCategory("PagedList")]
        public void EnumerablePagedListItem_ShouldReturnCorrectValue()
        {
            int pageSize = 8;
            int pageIndex = 2;
            int startIndex = (pageIndex - 1)*pageSize+1;
            var list = new PagedList<int>(Enumerable.Range(startIndex, startIndex + pageSize), pageIndex, pageSize, 88);
            Assert.AreEqual(list[0], 9);
            pageIndex = 8;
            startIndex = (pageIndex - 1) * pageSize + 1;
            list = new PagedList<int>(Enumerable.Range(startIndex, startIndex + pageSize), pageIndex, pageSize, 88);
            Assert.AreEqual(list[0], 57);
            Assert.AreEqual(list[7], 64);
        }


        [TestMethod]
        [TestCategory("PagedList")]
        public void ToPagedList_PageIndexOutOfRange_ShouldReturnDataOfTheLastPage()
        {
            IEnumerable<int> items = Enumerable.Range(1, 9).ToList();
            PagedList<int> list = items.ToPagedList(1,8);
            Assert.AreEqual(list[0], 1,"first item value should be 1");
            list = items.ToPagedList(2, 8);
            Assert.AreEqual(list[0], 9,"first item value in second page should be 9");
            list = items.AsQueryable().ToPagedList(5, 8);
            Assert.AreEqual(list[0], 9,"first item value in last page should be 9");
        }


        [TestMethod]
        [TestCategory("PagedList")]
        public void TotalItemsIs88AndPageSizeIs5_TotalPageCountShouldBe18()
        {
            var list = new PagedList<int>(Enumerable.Range(1, 88), 3, 5);
            Assert.AreEqual(list.TotalPageCount,18,"TotalPageCount should be 18");
        }


        [TestMethod]
        [TestCategory("PagedList")]
        public void PageIndexIs4_PageSizeIs5_StartRecordIndexShouldBe16()
        {
            var list = new PagedList<int>(Enumerable.Range(1, 88), 4, 5);
            Assert.AreEqual(list.StartItemIndex,16);
        }

        [TestMethod]
        [TestCategory("PagedList")]
        public void PageIndexIs3_PageSizeIs5_EndRecordIndexShouldBe15()
        {
            var list = new PagedList<int>(Enumerable.Range(1, 88), 3, 5);
            Assert.AreEqual(list.EndItemIndex, 15);
        }
    }
}
