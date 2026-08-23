using System;
using System.Data.SqlClient;
using web_ban_hang2.Models;

namespace web_ban_hang2.DAL
{
    public class DonHangDAL
    {
        private Database database = new Database();

        public int TaoDonHang(DonHang donHang)
        {
            using (SqlConnection conn =
                database.GetConnection())
            {
                conn.Open();

                SqlTransaction transaction =
                    conn.BeginTransaction();

                try
                {
                    string sqlDonHang = @"
                        INSERT INTO dbo.DonHang
                        (
                            MaKhachHang,
                            HoTenNguoiNhan,
                            SoDienThoai,
                            DiaChiGiaoHang,
                            TongTien,
                            TrangThai
                        )
                        VALUES
                        (
                            @MaKhachHang,
                            @HoTenNguoiNhan,
                            @SoDienThoai,
                            @DiaChiGiaoHang,
                            @TongTien,
                            N'Chờ xử lý'
                        );

                        SELECT SCOPE_IDENTITY();
                    ";

                    int maDonHang;

                    using (SqlCommand cmd =
                        new SqlCommand(
                            sqlDonHang,
                            conn,
                            transaction))
                    {
                        cmd.Parameters.AddWithValue(
                            "@MaKhachHang",
                            donHang.MaKhachHang.HasValue
                                ? (object)donHang.MaKhachHang.Value
                                : DBNull.Value
                        );

                        cmd.Parameters.AddWithValue(
                            "@HoTenNguoiNhan",
                            donHang.HoTenNguoiNhan
                        );

                        cmd.Parameters.AddWithValue(
                            "@SoDienThoai",
                            donHang.SoDienThoai
                        );

                        cmd.Parameters.AddWithValue(
                            "@DiaChiGiaoHang",
                            donHang.DiaChiGiaoHang
                        );

                        cmd.Parameters.AddWithValue(
                            "@TongTien",
                            donHang.TongTien
                        );

                        maDonHang =
                            Convert.ToInt32(
                                cmd.ExecuteScalar()
                            );
                    }

                    foreach (
                        ChiTietDonHang item
                        in donHang.ChiTiet)
                    {
                        string sqlChiTiet = @"
                            INSERT INTO dbo.ChiTietDonHang
                            (
                                MaDonHang,
                                MaSanPham,
                                SoLuong,
                                DonGia
                            )
                            VALUES
                            (
                                @MaDonHang,
                                @MaSanPham,
                                @SoLuong,
                                @DonGia
                            )
                        ";

                        using (SqlCommand cmd =
                            new SqlCommand(
                                sqlChiTiet,
                                conn,
                                transaction))
                        {
                            cmd.Parameters.AddWithValue(
                                "@MaDonHang",
                                maDonHang
                            );

                            cmd.Parameters.AddWithValue(
                                "@MaSanPham",
                                item.MaSanPham
                            );

                            cmd.Parameters.AddWithValue(
                                "@SoLuong",
                                item.SoLuong
                            );

                            cmd.Parameters.AddWithValue(
                                "@DonGia",
                                item.DonGia
                            );

                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();

                    return maDonHang;
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }
    }
}