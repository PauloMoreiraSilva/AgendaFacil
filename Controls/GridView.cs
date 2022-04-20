using System;
using System.Web.UI.WebControls;

namespace PI4Sem.Controls
{
    public class GridView : System.Web.UI.WebControls.GridView
    {
        private SortDirection _GridViewSortDirection = SortDirection.Descending;

        public GridView()
            : base()
        {
        }

        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.textDecoration='underline'";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none'";
            }

            if (e.Row.RowType == DataControlRowType.Header && this.AllowSorting)
            {
                int sortColumnIndex = GetSortColumnIndex();

                if (sortColumnIndex != -1)
                {
                    AddSortImage(sortColumnIndex, e.Row);
                }
            }

            base.OnRowDataBound(e);
        }

        protected int GetSortColumnIndex()
        {
            string sSortExpression = "";

            if (SortExpression?.Length == 0)
            {
                string[] sPKName = DataKeyNames;
                sSortExpression = sPKName[0];
            }
            else
            {
                sSortExpression = SortExpression;
            }

            foreach (DataControlField field in Columns)
            {
                if ((field.SortExpression == sSortExpression) || field.SortExpression.Contains("." + sSortExpression))
                {
                    return Columns.IndexOf(field);
                }
            }

            return -1;
        }

        protected void AddSortImage(int columnIndex, GridViewRow HeaderRow)
        {
            Image oImage = new Image();

            if (ViewState["GridViewSortDirection"] != null)
            {
                _GridViewSortDirection = (SortDirection)ViewState["GridViewSortDirection"];
            }

            if (_GridViewSortDirection == SortDirection.Ascending)
            {
                oImage.ImageUrl = "~/Images/seta_up.gif";
                oImage.ToolTip = "Crescente";
            }
            else
            {
                oImage.ImageUrl = "~/Images/seta_down.gif";
                oImage.ToolTip = "Decrescente";
            }

            HeaderRow.Cells[columnIndex].Controls.Add(oImage);
        }

        protected override void OnPageIndexChanging(GridViewPageEventArgs e)
        {
            SelectedIndex = -1;

            base.OnPageIndexChanging(e);
        }

        protected override void OnRowDeleted(GridViewDeletedEventArgs e)
        {
            /* Comentado para usar Ajax na própria página
            if (e.Exception == null)
            {
                AppProgram.SetAlert(this.Page, "Excluído com sucesso!");
            }
            */

            base.OnRowDeleted(e);
        }

        protected override void OnSorting(GridViewSortEventArgs e)
        {
            //_GridViewSortDirection = e.SortDirection;
            ViewState["GridViewSortDirection"] = e.SortDirection;

            base.OnSorting(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            //Tornar o pager visível mesmo quando tiver somente 1 página
            //Tem que executar primeiro o base para desenhar o Grid, só depois dará para tornar o pager visível
            base.OnPreRender(e);

            GridViewRow gvr = (GridViewRow)this.BottomPagerRow;

            if (gvr != null)
            {
                gvr.Visible = true;
            }
        }
    }
}