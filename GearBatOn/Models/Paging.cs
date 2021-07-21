using System;
using System.Web;

namespace GearBatOn.Models
{
    public class Paging
    {
        public string Pagination(int total, int page, int Take)
        {
            int offset = 1;
            if (total > 0)
            {
                double rowPerPage = Take;
                if (Convert.ToDouble(total) < Take)
                {
                    rowPerPage = Convert.ToDouble(total);
                }

                int totalPage = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(total) / rowPerPage));
                int current = page;
                int pageStart = Convert.ToInt16(Convert.ToDouble(current) - Convert.ToDouble(offset));
                int pageEnd = Convert.ToInt16(Convert.ToDouble(current) + Convert.ToDouble(offset));
                string numPage = "";
                if (totalPage < 1) return "";
                numPage += "<nav aria-label='paging'><ul class='pagination justify-content-center'>";
                if (current > 1) numPage += "<li class='page-item'><a class='page-link' href='#' onclick='Paging(" + (page - 1) + ")'>&laquo;</a></li>";
                else numPage += "<li class='page-item disabled'><a class='page-link' href='#'>&laquo;</a></li>";
                if (current > (offset + 1)) numPage += "<li class='page-item'><a class='page-link' href='#' onclick='Paging(1)'>1</a></li><li class='page-item disabled'><a class='page-link' href='#'>...</a></li>";
                for (int i = 1; i <= totalPage; i++)
                {
                    if (pageStart <= i && pageEnd >= i)
                    {
                        if (i == current) numPage += "<li class='page-item active'><a class='page-link' href='#'>" + i + "</a></li>";
                        else numPage += "<li class='page-item'><a class='page-link' href='#' onclick='Paging(" + i + ")'>" + i + "</a></li>";
                    }
                }
                if (totalPage > pageEnd)
                {
                    numPage += "<li class='page-item disabled'><a class='page-link' href='#'>...</a></li><li class='page-item'><a class='page-link' href='#' onclick='Paging(" + totalPage + ")'>" + totalPage + "</a></li>";
                }
                if (current < totalPage) numPage += "<li class='page-item'><a class='page-link' href='#' onclick='Paging(" + (page + 1) + ")'>&raquo;</a></li>";
                else numPage += "<li class='page-item disabled'><a class='page-link' href='#'>&raquo;</a></li>";
                numPage += "</ul></nav>";
                return numPage;
            }
            else
            {
                return "no records found";
            }
        }

        //public string Pagination(int total, int page, int Take, string Controler, string View, string Params)
        //{
        //    int offset = 1;
        //    if (total > 0)
        //    {
        //        string c_url = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        //        string URL = c_url.Substring(0, c_url.IndexOf(Controler.ToLower()));
        //        double rowPerPage = Take;
        //        if (Convert.ToDouble(total) < Take)
        //        {
        //            rowPerPage = Convert.ToDouble(total);
        //        }

        //        int totalPage = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(total) / rowPerPage));
        //        int current = page;
        //        int pageStart = Convert.ToInt16(Convert.ToDouble(current) - Convert.ToDouble(offset));
        //        int pageEnd = Convert.ToInt16(Convert.ToDouble(current) + Convert.ToDouble(offset));
        //        string numPage = "";
        //        if (totalPage < 1) return "";
        //        numPage += "<nav aria-label='paging'><ul class='pagination justify-content-center'>";
        //        if (current > 1) numPage += "<li class='page-item'><a class='page-link' href='" + URL + Controler + "/" + View + "?Page=" + (page - 1) + Params + "'>Previous</a></li>";
        //        else numPage += "<li class='page-item disabled'><a class='page-link' href='#'>Previous</a></li>";
        //        if (current > (offset + 1)) numPage += "<li class='page-item'><a class='page-link' href='" + URL + Controler + "/" + View + "?Page=1" + Params + "'>1</a></li><li class='page-item disabled'><a class='page-link' href='#'>...</a></li>"; 
        //        for (int i = 1; i <= totalPage; i++)
        //        {
        //            if (pageStart <= i && pageEnd >= i)
        //            {
        //                if (i == current) numPage += "<li class='page-item active'><a class='page-link' href='#'>" + i + "</a></li>";
        //                else numPage += "<li class='page-item'><a class='page-link' href='" + URL + Controler + "/" + View + "?Page=" + i + Params + "'>" + i + "</a></li>";
        //            }
        //        }
        //        if (totalPage > pageEnd)
        //        {
        //            numPage += "<li class='page-item disabled'><a class='page-link' href='#'>...</a></li><li class='page-item'><a class='page-link' href='" + URL + Controler + "/" + View + "?Page=" + (totalPage) + Params + "'>" + totalPage + "</a></li>";
        //        }
        //        if (current < totalPage) numPage += "<li class='page-item'><a class='page-link' href='" + URL + Controler + "/" + View + "?Page=" + (page + 1) + Params + "'>Next</a></li>";
        //        else numPage += "<li class='page-item disabled'><a class='page-link' href='#'>Next</a></li>";
        //        numPage += "</ul></nav>";
        //        return numPage;
        //    }
        //    else
        //    {
        //        return "no records found";
        //    }
        //}
    }
}