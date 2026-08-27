using iTextSharp.text;
using iTextSharp.text.pdf;

using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_BaoCaoDoanhThu : AdminBasePage
    {
        private readonly BaoCaoDoanhThuDAL baoCaoDAL =
            new BaoCaoDoanhThuDAL();


        // ==========================================
        // PAGE LOAD
        // ==========================================

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime dauThang =
                    new DateTime(
                        DateTime.Today.Year,
                        DateTime.Today.Month,
                        1);

                txtTuNgay.Text =
                    dauThang.ToString("yyyy-MM-dd");

                txtDenNgay.Text =
                    DateTime.Today.ToString("yyyy-MM-dd");

                LoadBaoCao();
            }
        }


        // ==========================================
        // XEM BÁO CÁO
        // ==========================================

        protected void btnXemBaoCao_Click(
            object sender,
            EventArgs e)
        {
            DateTime tuNgay;
            DateTime denNgay;

            if (!TryGetDateRange(
                out tuNgay,
                out denNgay))
            {
                return;
            }

            LoadBaoCao(
                tuNgay,
                denNgay);
        }


        // ==========================================
        // XUẤT EXCEL
        // ==========================================

        protected void btnExcel_Click(
            object sender,
            EventArgs e)
        {
            DateTime tuNgay;
            DateTime denNgay;

            if (!TryGetDateRange(
                out tuNgay,
                out denNgay))
            {
                return;
            }

            DataTable table =
                baoCaoDAL.GetBaoCao(
                    tuNgay,
                    denNgay);

            decimal tongDoanhThu =
                baoCaoDAL.GetTongDoanhThu(
                    tuNgay,
                    denNgay);

            ExportExcel(
                table,
                tongDoanhThu,
                tuNgay,
                denNgay);
        }


        // ==========================================
        // XUẤT PDF
        // ==========================================

        protected void btnPdf_Click(
            object sender,
            EventArgs e)
        {
            DateTime tuNgay;
            DateTime denNgay;

            if (!TryGetDateRange(
                out tuNgay,
                out denNgay))
            {
                return;
            }

            DataTable table =
                baoCaoDAL.GetBaoCao(
                    tuNgay,
                    denNgay);

            decimal tongDoanhThu =
                baoCaoDAL.GetTongDoanhThu(
                    tuNgay,
                    denNgay);

            ExportPdf(
                table,
                tongDoanhThu,
                tuNgay,
                denNgay);
        }


        // ==========================================
        // LOAD BÁO CÁO
        // ==========================================

        private void LoadBaoCao()
        {
            DateTime tuNgay;
            DateTime denNgay;

            if (TryGetDateRange(
                out tuNgay,
                out denNgay))
            {
                LoadBaoCao(
                    tuNgay,
                    denNgay);
            }
        }


        private void LoadBaoCao(
            DateTime tuNgay,
            DateTime denNgay)
        {
            DataTable table =
                baoCaoDAL.GetBaoCao(
                    tuNgay,
                    denNgay);

            gvBaoCao.DataSource =
                table;

            gvBaoCao.DataBind();


            lblSoDonHang.Text =
                baoCaoDAL
                .GetSoDonHang(
                    tuNgay,
                    denNgay)
                .ToString();


            lblTongDoanhThu.Text =
                FormatMoney(
                    baoCaoDAL.GetTongDoanhThu(
                        tuNgay,
                        denNgay));


            lblThongBao.Text =
                string.Empty;
        }


        // ==========================================
        // KIỂM TRA NGÀY
        // ==========================================

        private bool TryGetDateRange(
            out DateTime tuNgay,
            out DateTime denNgay)
        {
            tuNgay =
                DateTime.MinValue;

            denNgay =
                DateTime.MinValue;


            if (!DateTime.TryParseExact(
                txtTuNgay.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out tuNgay))
            {
                lblThongBao.Text =
                    "Ngày bắt đầu không hợp lệ.";

                return false;
            }


            if (!DateTime.TryParseExact(
                txtDenNgay.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out denNgay))
            {
                lblThongBao.Text =
                    "Ngày kết thúc không hợp lệ.";

                return false;
            }


            if (tuNgay.Date >
                denNgay.Date)
            {
                lblThongBao.Text =
                    "Từ ngày không được lớn hơn đến ngày.";

                return false;
            }


            return true;
        }


        // ==========================================
        // FORMAT TIỀN
        // ==========================================

        private string FormatMoney(
            decimal value)
        {
            return value.ToString("N0") +
                   " ₫";
        }


        // ==========================================
        // XUẤT EXCEL
        // ==========================================

        private void ExportExcel(
            DataTable table,
            decimal tongDoanhThu,
            DateTime tuNgay,
            DateTime denNgay)
        {
            string fileName =
                "BaoCaoDoanhThu_" +
                tuNgay.ToString("yyyyMMdd") +
                "_" +
                denNgay.ToString("yyyyMMdd") +
                ".xls";


            StringBuilder xml =
                new StringBuilder();


            // ======================================
            // HEADER XML EXCEL
            // ======================================

            xml.Append(
                "<?xml version=\"1.0\"?>");

            xml.Append(
                "<?mso-application progid=\"Excel.Sheet\"?>");


            xml.Append(
                "<Workbook " +
                "xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" " +
                "xmlns:o=\"urn:schemas-microsoft-com:office:office\" " +
                "xmlns:x=\"urn:schemas-microsoft-com:office:excel\" " +
                "xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">");


            // ======================================
            // STYLE
            // ======================================

            xml.Append("<Styles>");

            xml.Append(
                "<Style ss:ID=\"Header\">" +
                "<Font ss:Bold=\"1\"/>" +
                "<Interior ss:Color=\"#D9EAF7\" ss:Pattern=\"Solid\"/>" +
                "</Style>");

            xml.Append(
                "<Style ss:ID=\"Title\">" +
                "<Font ss:Bold=\"1\" ss:Size=\"16\"/>" +
                "</Style>");

            xml.Append(
                "<Style ss:ID=\"Money\">" +
                "<NumberFormat ss:Format=\"#,##0\"/>" +
                "</Style>");

            xml.Append("</Styles>");


            // ======================================
            // WORKSHEET
            // ======================================

            xml.Append(
                "<Worksheet ss:Name=\"BaoCaoDoanhThu\">");

            xml.Append("<Table>");


            // ======================================
            // TIÊU ĐỀ
            // ======================================

            xml.Append(
                "<Row>" +
                "<Cell ss:StyleID=\"Title\">" +
                "<Data ss:Type=\"String\">" +
                XmlEncode("BÁO CÁO DOANH THU") +
                "</Data>" +
                "</Cell>" +
                "</Row>");


            // ======================================
            // THỜI GIAN
            // ======================================

            xml.Append(
                CreateExcelRow(
                    "Thời gian",
                    tuNgay.ToString("dd/MM/yyyy") +
                    " - " +
                    denNgay.ToString("dd/MM/yyyy")));


            // ======================================
            // TRẠNG THÁI
            // ======================================

            xml.Append(
                CreateExcelRow(
                    "Trạng thái",
                    "Chỉ tính các đơn hàng đã giao"));


            // ======================================
            // TỔNG DOANH THU
            // ======================================

            xml.Append(
                "<Row>" +

                "<Cell>" +
                "<Data ss:Type=\"String\">" +
                "Tổng doanh thu" +
                "</Data>" +
                "</Cell>" +

                "<Cell ss:StyleID=\"Money\">" +
                "<Data ss:Type=\"Number\">" +
                tongDoanhThu
                    .ToString(
                        CultureInfo.InvariantCulture) +
                "</Data>" +
                "</Cell>" +

                "</Row>");


            // ======================================
            // SỐ ĐƠN
            // ======================================

            xml.Append(
                CreateExcelRow(
                    "Số đơn đã giao",
                    table.Rows.Count.ToString()));


            // ======================================
            // DÒNG TRỐNG
            // ======================================

            xml.Append("<Row></Row>");


            // ======================================
            // HEADER
            // ======================================

            xml.Append("<Row>");

            xml.Append(
                CreateExcelHeader("Mã đơn"));

            xml.Append(
                CreateExcelHeader("Khách hàng"));

            xml.Append(
                CreateExcelHeader("Người nhận"));

            xml.Append(
                CreateExcelHeader("Số điện thoại"));

            xml.Append(
                CreateExcelHeader("Tổng tiền"));

            xml.Append(
                CreateExcelHeader("Trạng thái"));

            xml.Append(
                CreateExcelHeader("Ngày đặt"));

            xml.Append("</Row>");


            // ======================================
            // DỮ LIỆU
            // ======================================

            foreach (DataRow row in table.Rows)
            {
                xml.Append("<Row>");


                xml.Append(
                    CreateExcelCell(
                        row["MaDonHang"],
                        "Number"));


                xml.Append(
                    CreateExcelCell(
                        row["TenKhachHang"],
                        "String"));


                xml.Append(
                    CreateExcelCell(
                        row["HoTenNguoiNhan"],
                        "String"));


                xml.Append(
                    CreateExcelCell(
                        row["SoDienThoai"],
                        "String"));


                decimal tongTien =
                    row["TongTien"] == DBNull.Value
                        ? 0m
                        : Convert.ToDecimal(
                            row["TongTien"]);


                xml.Append(
                    "<Cell ss:StyleID=\"Money\">" +
                    "<Data ss:Type=\"Number\">" +
                    tongTien.ToString(
                        CultureInfo.InvariantCulture) +
                    "</Data>" +
                    "</Cell>");


                xml.Append(
                    CreateExcelCell(
                        row["TrangThai"],
                        "String"));


                DateTime ngayDat =
                    Convert.ToDateTime(
                        row["NgayDat"]);


                xml.Append(
                    CreateExcelCell(
                        ngayDat.ToString(
                            "dd/MM/yyyy HH:mm"),
                        "String"));


                xml.Append("</Row>");
            }


            xml.Append("</Table>");

            xml.Append("</Worksheet>");

            xml.Append("</Workbook>");


            byte[] bytes =
                Encoding.UTF8.GetBytes(
                    xml.ToString());


            // ======================================
            // DOWNLOAD EXCEL
            // ======================================

            Response.Clear();

            Response.ClearHeaders();

            Response.ClearContent();


            Response.ContentType =
                "application/vnd.ms-excel";


            Response.AddHeader(
                "Content-Disposition",
                "attachment; filename=" +
                fileName);


            Response.OutputStream.Write(
                bytes,
                0,
                bytes.Length);


            Response.Flush();


            CompleteResponse();
        }


        // ==========================================
        // EXCEL ROW
        // ==========================================

        private string CreateExcelRow(
            string label,
            string value)
        {
            return
                "<Row>" +

                "<Cell>" +
                "<Data ss:Type=\"String\">" +
                XmlEncode(label) +
                "</Data>" +
                "</Cell>" +

                "<Cell>" +
                "<Data ss:Type=\"String\">" +
                XmlEncode(value) +
                "</Data>" +
                "</Cell>" +

                "</Row>";
        }


        // ==========================================
        // EXCEL HEADER
        // ==========================================

        private string CreateExcelHeader(
            string text)
        {
            return
                "<Cell ss:StyleID=\"Header\">" +

                "<Data ss:Type=\"String\">" +

                XmlEncode(text) +

                "</Data>" +

                "</Cell>";
        }


        // ==========================================
        // EXCEL CELL
        // ==========================================

        private string CreateExcelCell(
            object value,
            string type)
        {
            string text =
                value == null ||
                value == DBNull.Value
                    ? string.Empty
                    : value.ToString();


            return
                "<Cell>" +

                "<Data ss:Type=\"" +
                type +
                "\">" +

                XmlEncode(text) +

                "</Data>" +

                "</Cell>";
        }


        // ==========================================
        // XML ENCODE
        // ==========================================

        private string XmlEncode(
            string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }


            return
                HttpUtility.HtmlEncode(
                    value);
        }


        // ==========================================
        // XUẤT PDF
        // ==========================================

        private void ExportPdf(
            DataTable table,
            decimal tongDoanhThu,
            DateTime tuNgay,
            DateTime denNgay)
        {
            string fileName =
                "BaoCaoDoanhThu_" +
                tuNgay.ToString("yyyyMMdd") +
                "_" +
                denNgay.ToString("yyyyMMdd") +
                ".pdf";


            using (MemoryStream stream =
                   new MemoryStream())
            {
                Document pdf =
                    new Document(
                        PageSize.A4.Rotate(),
                        25,
                        25,
                        25,
                        25);


                PdfWriter writer =
                    PdfWriter.GetInstance(
                        pdf,
                        stream);


                pdf.Open();


                BaseFont baseFont =
                    GetVietnameseBaseFont();


                Font titleFont =
                    new Font(
                        baseFont,
                        18,
                        Font.BOLD);


                Font normalFont =
                    new Font(
                        baseFont,
                        9,
                        Font.NORMAL);


                Font boldFont =
                    new Font(
                        baseFont,
                        9,
                        Font.BOLD);


                // ======================================
                // TIÊU ĐỀ
                // ======================================

                Paragraph title =
                    new Paragraph(
                        "BÁO CÁO DOANH THU",
                        titleFont);


                title.Alignment =
                    Element.ALIGN_CENTER;


                pdf.Add(title);


                // ======================================
                // THỜI GIAN
                // ======================================

                Paragraph period =
                    new Paragraph(
                        "Thời gian: " +
                        tuNgay.ToString("dd/MM/yyyy") +
                        " - " +
                        denNgay.ToString("dd/MM/yyyy"),
                        normalFont);


                period.Alignment =
                    Element.ALIGN_CENTER;


                pdf.Add(period);


                pdf.Add(
                    new Paragraph(
                        "Chỉ tính các đơn hàng có trạng thái: Đã giao",
                        normalFont));


                pdf.Add(
                    new Paragraph(
                        "Tổng số đơn đã giao: " +
                        table.Rows.Count,
                        boldFont));


                pdf.Add(
                    new Paragraph(
                        "Tổng doanh thu: " +
                        FormatMoney(
                            tongDoanhThu),
                        boldFont));


                pdf.Add(
                    new Paragraph(
                        " ",
                        normalFont));


                // ======================================
                // BẢNG PDF
                // ======================================

                PdfPTable pdfTable =
                    new PdfPTable(7);


                pdfTable.WidthPercentage =
                    100;


                pdfTable.SetWidths(
                    new float[]
                    {
                        0.7f,
                        1.8f,
                        1.8f,
                        1.5f,
                        1.5f,
                        1.2f,
                        1.5f
                    });


                AddPdfHeader(
                    pdfTable,
                    "Mã đơn",
                    boldFont);


                AddPdfHeader(
                    pdfTable,
                    "Khách hàng",
                    boldFont);


                AddPdfHeader(
                    pdfTable,
                    "Người nhận",
                    boldFont);


                AddPdfHeader(
                    pdfTable,
                    "SĐT",
                    boldFont);


                AddPdfHeader(
                    pdfTable,
                    "Tổng tiền",
                    boldFont);


                AddPdfHeader(
                    pdfTable,
                    "Trạng thái",
                    boldFont);


                AddPdfHeader(
                    pdfTable,
                    "Ngày đặt",
                    boldFont);


                // ======================================
                // DATA
                // ======================================

                foreach (DataRow row in table.Rows)
                {
                    AddPdfCell(
                        pdfTable,
                        GetCellValue(
                            row["MaDonHang"]),
                        normalFont);


                    AddPdfCell(
                        pdfTable,
                        GetCellValue(
                            row["TenKhachHang"]),
                        normalFont);


                    AddPdfCell(
                        pdfTable,
                        GetCellValue(
                            row["HoTenNguoiNhan"]),
                        normalFont);


                    AddPdfCell(
                        pdfTable,
                        GetCellValue(
                            row["SoDienThoai"]),
                        normalFont);


                    string tongTien =
                        row["TongTien"] == DBNull.Value
                            ? "0 ₫"
                            : Convert.ToDecimal(
                                row["TongTien"])
                            .ToString("N0") +
                            " ₫";


                    AddPdfCell(
                        pdfTable,
                        tongTien,
                        normalFont);


                    AddPdfCell(
                        pdfTable,
                        GetCellValue(
                            row["TrangThai"]),
                        normalFont);


                    string ngayDat =
                        row["NgayDat"] == DBNull.Value
                            ? string.Empty
                            : Convert.ToDateTime(
                                row["NgayDat"])
                            .ToString(
                                "dd/MM/yyyy HH:mm");


                    AddPdfCell(
                        pdfTable,
                        ngayDat,
                        normalFont);
                }


                pdf.Add(pdfTable);


                pdf.Close();

                writer.Close();


                byte[] bytes =
                    stream.ToArray();


                // ======================================
                // DOWNLOAD PDF
                // ======================================

                Response.Clear();

                Response.ClearHeaders();

                Response.ClearContent();


                Response.ContentType =
                    "application/pdf";


                Response.AddHeader(
                    "Content-Disposition",
                    "attachment; filename=" +
                    fileName);


                Response.OutputStream.Write(
                    bytes,
                    0,
                    bytes.Length);


                Response.Flush();


                CompleteResponse();
            }
        }


        // ==========================================
        // LẤY GIÁ TRỊ CELL
        // ==========================================

        private string GetCellValue(
            object value)
        {
            if (value == null ||
                value == DBNull.Value)
            {
                return string.Empty;
            }


            return value.ToString();
        }


        // ==========================================
        // FONT TIẾNG VIỆT
        // ==========================================

        private BaseFont GetVietnameseBaseFont()
        {
            string[] fontPaths =
            {
                @"C:\Windows\Fonts\arial.ttf",
                @"C:\Windows\Fonts\tahoma.ttf"
            };


            foreach (string path in fontPaths)
            {
                if (File.Exists(path))
                {
                    return BaseFont.CreateFont(
                        path,
                        BaseFont.IDENTITY_H,
                        BaseFont.EMBEDDED);
                }
            }


            return BaseFont.CreateFont(
                BaseFont.HELVETICA,
                BaseFont.WINANSI,
                BaseFont.NOT_EMBEDDED);
        }


        // ==========================================
        // PDF HEADER
        // ==========================================

        private void AddPdfHeader(
            PdfPTable table,
            string text,
            Font font)
        {
            PdfPCell cell =
                new PdfPCell(
                    new Phrase(
                        text,
                        font));


            cell.HorizontalAlignment =
                Element.ALIGN_CENTER;


            cell.VerticalAlignment =
                Element.ALIGN_MIDDLE;


            cell.Padding =
                5;


            table.AddCell(cell);
        }


        // ==========================================
        // PDF CELL
        // ==========================================

        private void AddPdfCell(
            PdfPTable table,
            string text,
            Font font)
        {
            PdfPCell cell =
                new PdfPCell(
                    new Phrase(
                        text ?? string.Empty,
                        font));


            cell.VerticalAlignment =
                Element.ALIGN_MIDDLE;


            cell.Padding =
                4;


            table.AddCell(cell);
        }


        // ==========================================
        // COMPLETE REQUEST
        // ==========================================

        private void CompleteResponse()
        {
            HttpContext.Current
                .ApplicationInstance
                .CompleteRequest();
        }
    }
}
