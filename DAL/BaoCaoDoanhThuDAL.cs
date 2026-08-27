using System;
using System.Data;
using System.Data.SqlClient;

namespace web_ban_hang2.DAL
{
    public class BaoCaoDoanhThuDAL
    {
        private readonly Database database;


        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public BaoCaoDoanhThuDAL()
        {
            database = new Database();
        }


        // ==========================================
        // LẤY BÁO CÁO DOANH THU
        // CHỈ TÍNH ĐƠN ĐÃ GIAO
        // ==========================================

        public DataTable GetBaoCao(
            DateTime tuNgay,
            DateTime denNgay)
        {
            DateTime ngayBatDau =
                tuNgay.Date;

            DateTime ngayKetThuc =
                denNgay.Date.AddDays(1);


            const string sql = @"
                SELECT
                    dh.MaDonHang,

                    ISNULL(
                        kh.HoTen,
                        N'Khách vãng lai'
                    ) AS TenKhachHang,

                    dh.HoTenNguoiNhan,

                    dh.SoDienThoai,

                    dh.TongTien,

                    dh.TrangThai,

                    dh.NgayDat

                FROM DonHang dh

                LEFT JOIN KhachHang kh
                    ON dh.MaKhachHang =
                       kh.MaKhachHang

                WHERE
                    dh.TrangThai =
                    N'Đã giao'

                    AND dh.NgayDat >= @TuNgay

                    AND dh.NgayDat < @DenNgay

                ORDER BY
                    dh.NgayDat DESC,

                    dh.MaDonHang DESC
            ";


            using (
                SqlConnection conn =
                database.GetConnection())

            using (
                SqlCommand cmd =
                new SqlCommand(
                    sql,
                    conn))

            using (
                SqlDataAdapter adapter =
                new SqlDataAdapter(cmd))
            {
                cmd.Parameters.Add(
                    "@TuNgay",
                    SqlDbType.DateTime).Value =
                    ngayBatDau;


                cmd.Parameters.Add(
                    "@DenNgay",
                    SqlDbType.DateTime).Value =
                    ngayKetThuc;


                DataTable table =
                    new DataTable();


                adapter.Fill(table);


                return table;
            }
        }


        // ==========================================
        // TỔNG DOANH THU
        // ==========================================

        public decimal GetTongDoanhThu(
            DateTime tuNgay,
            DateTime denNgay)
        {
            DateTime ngayBatDau =
                tuNgay.Date;

            DateTime ngayKetThuc =
                denNgay.Date.AddDays(1);


            const string sql = @"
                SELECT
                    ISNULL(
                        SUM(TongTien),
                        0
                    )

                FROM DonHang

                WHERE
                    TrangThai =
                    N'Đã giao'

                    AND NgayDat >= @TuNgay

                    AND NgayDat < @DenNgay
            ";


            using (
                SqlConnection conn =
                database.GetConnection())

            using (
                SqlCommand cmd =
                new SqlCommand(
                    sql,
                    conn))
            {
                cmd.Parameters.Add(
                    "@TuNgay",
                    SqlDbType.DateTime).Value =
                    ngayBatDau;


                cmd.Parameters.Add(
                    "@DenNgay",
                    SqlDbType.DateTime).Value =
                    ngayKetThuc;


                conn.Open();


                object result =
                    cmd.ExecuteScalar();


                if (result == null ||
                    result == DBNull.Value)
                {
                    return 0m;
                }


                return Convert.ToDecimal(result);
            }
        }


        // ==========================================
        // ĐẾM ĐƠN ĐÃ GIAO
        // ==========================================

        public int GetSoDonHang(
            DateTime tuNgay,
            DateTime denNgay)
        {
            DateTime ngayBatDau =
                tuNgay.Date;

            DateTime ngayKetThuc =
                denNgay.Date.AddDays(1);


            const string sql = @"
                SELECT
                    COUNT(*)

                FROM DonHang

                WHERE
                    TrangThai =
                    N'Đã giao'

                    AND NgayDat >= @TuNgay

                    AND NgayDat < @DenNgay
            ";


            using (
                SqlConnection conn =
                database.GetConnection())

            using (
                SqlCommand cmd =
                new SqlCommand(
                    sql,
                    conn))
            {
                cmd.Parameters.Add(
                    "@TuNgay",
                    SqlDbType.DateTime).Value =
                    ngayBatDau;


                cmd.Parameters.Add(
                    "@DenNgay",
                    SqlDbType.DateTime).Value =
                    ngayKetThuc;


                conn.Open();


                object result =
                    cmd.ExecuteScalar();


                if (result == null ||
                    result == DBNull.Value)
                {
                    return 0;
                }


                return Convert.ToInt32(result);
            }
        }
    }
}
