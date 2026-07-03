using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace duan_totnghiep.Migrations
{
    /// <inheritdoc />
    public partial class ThemGhiChuDonHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DANHMUC",
                columns: table => new
                {
                    MADM = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TENDM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MOTA = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DANHMUC__603F005C68B08698", x => x.MADM);
                });

            migrationBuilder.CreateTable(
                name: "KHUYENMAI",
                columns: table => new
                {
                    MAKM = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TENKM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PHANTRAMGIAM = table.Column<int>(type: "int", nullable: false),
                    NGAYBATDAU = table.Column<DateOnly>(type: "date", nullable: true),
                    NGAYKETTHUC = table.Column<DateOnly>(type: "date", nullable: true),
                    TRANGTHAI = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KHUYENMA__603F592BE78437AB", x => x.MAKM);
                });

            migrationBuilder.CreateTable(
                name: "TAIKHOAN",
                columns: table => new
                {
                    MATK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TENDANGNHAP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MATKHAU = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    VAITRO = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TRANGTHAI = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TAIKHOAN__602372168E262B7D", x => x.MATK);
                });

            migrationBuilder.CreateTable(
                name: "THUONGHIEU",
                columns: table => new
                {
                    MATH = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TENTH = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MOTA = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__THUONGHI__6023721B1BF9AE32", x => x.MATH);
                });

            migrationBuilder.CreateTable(
                name: "KHACHHANG",
                columns: table => new
                {
                    MAKH = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HOTEN = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SDT = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    EMAIL = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DIACHI = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MATK = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KHACHHAN__603F592CEC605BBA", x => x.MAKH);
                    table.ForeignKey(
                        name: "FK__KHACHHANG__MATK__4CA06362",
                        column: x => x.MATK,
                        principalTable: "TAIKHOAN",
                        principalColumn: "MATK");
                });

            migrationBuilder.CreateTable(
                name: "NHANVIEN",
                columns: table => new
                {
                    MANV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HOTEN = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SDT = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    EMAIL = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DIACHI = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CHUCVU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MATK = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NHANVIEN__603F511415B19D8E", x => x.MANV);
                    table.ForeignKey(
                        name: "FK__NHANVIEN__MATK__4F7CD00D",
                        column: x => x.MATK,
                        principalTable: "TAIKHOAN",
                        principalColumn: "MATK");
                });

            migrationBuilder.CreateTable(
                name: "SANPHAM",
                columns: table => new
                {
                    MASP = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TENSP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GIA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MOTA = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HINHANH = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    SOLUONGTON = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    Ngaytao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MATH = table.Column<int>(type: "int", nullable: true),
                    MADM = table.Column<int>(type: "int", nullable: true),
                    MAKM = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SANPHAM__60228A3236F869AC", x => x.MASP);
                    table.ForeignKey(
                        name: "FK_SANPHAM_DANHMUC",
                        column: x => x.MADM,
                        principalTable: "DANHMUC",
                        principalColumn: "MADM");
                    table.ForeignKey(
                        name: "FK_SANPHAM_KHUYENMAI",
                        column: x => x.MAKM,
                        principalTable: "KHUYENMAI",
                        principalColumn: "MAKM");
                    table.ForeignKey(
                        name: "FK__SANPHAM__MATH__5535A963",
                        column: x => x.MATH,
                        principalTable: "THUONGHIEU",
                        principalColumn: "MATH");
                });

            migrationBuilder.CreateTable(
                name: "DONHANG",
                columns: table => new
                {
                    MADH = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MAKH = table.Column<int>(type: "int", nullable: false),
                    MANV = table.Column<int>(type: "int", nullable: true),
                    NGAYDAT = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    TONGTIEN = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TRANGTHAI = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Ghichu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DIACHINHAN = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PHUONGTHUCTHANHTOAN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DONHANG__603F00472BA9D9C5", x => x.MADH);
                    table.ForeignKey(
                        name: "FK_DONHANG_NHANVIEN",
                        column: x => x.MANV,
                        principalTable: "NHANVIEN",
                        principalColumn: "MANV");
                    table.ForeignKey(
                        name: "FK__DONHANG__MAKH__60A75C0F",
                        column: x => x.MAKH,
                        principalTable: "KHACHHANG",
                        principalColumn: "MAKH",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GIOHANG",
                columns: table => new
                {
                    MAGH = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MAKH = table.Column<int>(type: "int", nullable: false),
                    MASP = table.Column<int>(type: "int", nullable: false),
                    SOLUONG = table.Column<int>(type: "int", nullable: false),
                    NGAYTAO = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GIOHANG__603F38A3B480C08A", x => x.MAGH);
                    table.ForeignKey(
                        name: "FK_GIOHANG_SANPHAM",
                        column: x => x.MASP,
                        principalTable: "SANPHAM",
                        principalColumn: "MASP",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__GIOHANG__MAKH__59063A47",
                        column: x => x.MAKH,
                        principalTable: "KHACHHANG",
                        principalColumn: "MAKH",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TONKHO",
                columns: table => new
                {
                    MATONKHO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MASP = table.Column<int>(type: "int", nullable: false),
                    SOLUONGNHAP = table.Column<int>(type: "int", nullable: false),
                    SOLUONGXUAT = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    NGAYCAPNHAT = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TONKHO__9E1E7421F850ACF7", x => x.MATONKHO);
                    table.ForeignKey(
                        name: "FK__TONKHO__MASP__693CA210",
                        column: x => x.MASP,
                        principalTable: "SANPHAM",
                        principalColumn: "MASP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CHITIETDONHANG",
                columns: table => new
                {
                    MADH = table.Column<int>(type: "int", nullable: false),
                    MASP = table.Column<int>(type: "int", nullable: false),
                    SOLUONG = table.Column<int>(type: "int", nullable: false),
                    DONGIA = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CHITIETD__563D28E482B9A612", x => new { x.MADH, x.MASP });
                    table.ForeignKey(
                        name: "FK__CHITIETDON__MADH__6383C8BA",
                        column: x => x.MADH,
                        principalTable: "DONHANG",
                        principalColumn: "MADH");
                    table.ForeignKey(
                        name: "FK__CHITIETDON__MASP__6477ECF3",
                        column: x => x.MASP,
                        principalTable: "SANPHAM",
                        principalColumn: "MASP");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETDONHANG_MASP",
                table: "CHITIETDONHANG",
                column: "MASP");

            migrationBuilder.CreateIndex(
                name: "IX_DONHANG_MAKH",
                table: "DONHANG",
                column: "MAKH");

            migrationBuilder.CreateIndex(
                name: "IX_DONHANG_MANV",
                table: "DONHANG",
                column: "MANV");

            migrationBuilder.CreateIndex(
                name: "IX_GIOHANG_MAKH",
                table: "GIOHANG",
                column: "MAKH");

            migrationBuilder.CreateIndex(
                name: "IX_GIOHANG_MASP",
                table: "GIOHANG",
                column: "MASP");

            migrationBuilder.CreateIndex(
                name: "IX_KHACHHANG_MATK",
                table: "KHACHHANG",
                column: "MATK");

            migrationBuilder.CreateIndex(
                name: "IX_NHANVIEN_MATK",
                table: "NHANVIEN",
                column: "MATK");

            migrationBuilder.CreateIndex(
                name: "IX_SANPHAM_MADM",
                table: "SANPHAM",
                column: "MADM");

            migrationBuilder.CreateIndex(
                name: "IX_SANPHAM_MAKM",
                table: "SANPHAM",
                column: "MAKM");

            migrationBuilder.CreateIndex(
                name: "IX_SANPHAM_MATH",
                table: "SANPHAM",
                column: "MATH");

            migrationBuilder.CreateIndex(
                name: "UQ__TAIKHOAN__6C836FE5B3A643FF",
                table: "TAIKHOAN",
                column: "TENDANGNHAP",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TONKHO_MASP",
                table: "TONKHO",
                column: "MASP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CHITIETDONHANG");

            migrationBuilder.DropTable(
                name: "GIOHANG");

            migrationBuilder.DropTable(
                name: "TONKHO");

            migrationBuilder.DropTable(
                name: "DONHANG");

            migrationBuilder.DropTable(
                name: "SANPHAM");

            migrationBuilder.DropTable(
                name: "NHANVIEN");

            migrationBuilder.DropTable(
                name: "KHACHHANG");

            migrationBuilder.DropTable(
                name: "DANHMUC");

            migrationBuilder.DropTable(
                name: "KHUYENMAI");

            migrationBuilder.DropTable(
                name: "THUONGHIEU");

            migrationBuilder.DropTable(
                name: "TAIKHOAN");
        }
    }
}
