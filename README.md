# Web Bán Laptop – Đồ án ASP.NET DX22TT8  
**SV:** Vũ Trí Đăng (MSSV: 170122391)  
**Môn học:** Lập trình Web với ASP.NET (DX22TT8)  
**Ngày nộp:** 12/11/2025

---

## 1. Giới thiệu  
Ứng dụng web **bán laptop trực tuyến** được xây dựng nhằm mục tiêu hoàn thiện đồ án môn học về ASP.NET.  
Dự án cung cấp hệ thống quản lý sản phẩm, người dùng, giỏ hàng và đơn hàng với giao diện trực quan, dễ sử dụng.

**Các chức năng chính:**  
- Hiển thị danh sách laptop, tìm kiếm và lọc theo thương hiệu/giá.  
- Quản lý tài khoản người dùng (đăng ký, đăng nhập, cập nhật thông tin).  
- Chức năng giỏ hàng, đặt hàng, quản lý đơn hàng.  
- Trang admin: thêm/xóa/sửa sản phẩm, quản lý đơn hàng và thống kê doanh thu.  

---

## 2. Công nghệ sử dụng  
- **Ngôn ngữ lập trình:** C#, ASP.NET Framework 3.5  
- **CSDL:** Microsoft SQL Server  
- **Giao diện:** HTML, CSS, Bootstrap, JavaScript  
- **Công cụ:** Visual Studio 2022, SQL Server, GitHub  

---

## 3. Cấu trúc dự án  
```
ASPNET-dx22tt8-vutridang-WEBBANLAPTOP/
│
├── setup/                       # (Tuỳ chọn) Các tập tin cài đặt hoặc dữ liệu thử
│
├── src/                         # Mã nguồn (source code) của hệ thống Web ASP.NET
│   ├── Controllers/
│   ├── Models/
│   ├── Views/
│   ├── wwwroot/
│   ├── appsettings.json
│   └── WebBanLaptop.sln
│
├── progress-report/              # Báo cáo tiến độ hàng tuần (bắt buộc theo quy định)
│   └── progress-report-tuần1-3.pdf
│
├── thesis/                       # Thư mục tài liệu đồ án (bắt buộc)
│   ├── doc/                      # Báo cáo gốc (.docx)
│   │   └── 170122391_VU_TRI_DANG_DX22TT8.docx
│   ├── pdf/                      # Báo cáo xuất PDF
│   │   └── 170122391_VU_TRI_DANG_DX22TT8.pdf
│   ├── abs/                      # Bản trình bày (PowerPoint, video)
│   │   └── DoAn_WebBanLaptop.pptx
│   ├── refs/                     # Tài liệu tham khảo (PDF, hình, link ngoài)
│   │   └── ref01-aspnet-core.pdf
│   └── html/                     # (Tuỳ chọn) Phiên bản báo cáo web/html
│
├── soft/                         # (Tuỳ chọn) Các phần mềm, công cụ dùng trong quá trình làm
│
├── docker/                       # (Tuỳ chọn) File cấu hình triển khai Docker
│
├── README.md                     # File mô tả dự án (mô tả chung, hướng dẫn chạy)
└── .gitignore                    # File loại trừ commit Git



---


   ```
## 4. Cách cài đặt và chạy  
1. Clone repository:  
   ```bash
   git clone https://github.com/Vutridang/ASPNET-dx22tt8-vutridang-WEBBANLAPTOP.git
---
<img width="1920" height="1081" alt="h1" src="https://github.com/user-attachments/assets/a8d2770a-e18b-47c7-830f-322a0d05f59d" />

2. Mở project bằng **Visual Studio 2022**.
---   
<img width="1920" height="1081" alt="h2" src="https://github.com/user-attachments/assets/b71834cb-a364-4f58-b1d9-08c01f5a1fc5" />

3. Kiểm tra file **Web.config** → chỉnh lại `connectionString` trỏ đến SQL Server cục bộ của bạn.
---
<img width="1920" height="1081" alt="h3" src="https://github.com/user-attachments/assets/feb28529-91cb-4e7f-8ad7-803e04975318" />
<img width="1920" height="1081" alt="h4" src="https://github.com/user-attachments/assets/345e6fd4-3754-4ba0-aaec-cb5fcda71535" />

4. Tạo database WebBanLapTop -> click new query -> copy paste script file `WebBanLapTop.sql` -> click execute.
---
<img width="1920" height="1081" alt="h5" src="https://github.com/user-attachments/assets/ec0c9e49-ae2b-4a46-9153-e1879fc8dec8" />
<img width="1920" height="1081" alt="h6" src="https://github.com/user-attachments/assets/6696bd71-7d80-424d-8ecf-498124248b90" />

5. Click chuột phải vào file Home/index.aspx -> chọn Set as Start Page
---
<img width="1920" height="1081" alt="h7" src="https://github.com/user-attachments/assets/2e212edc-a04c-4c64-bcd6-6f56adcd384c" />

6. Chạy chương trình bằng `IIS Express` hoặc `Ctrl + F5`.
---
<img width="1920" height="1081" alt="h8" src="https://github.com/user-attachments/assets/be0c7d51-0840-428c-b986-bace9ef5fcf9" />

Kết quả
---
<img width="1920" height="1081" alt="h9" src="https://github.com/user-attachments/assets/f54a2507-ce9b-4563-853a-f5a51da9f7d5" />
<img width="1920" height="1081" alt="h10" src="https://github.com/user-attachments/assets/f7eee629-c1dd-4b86-84b2-f7b984bfbd2c" />

Nhập đường dẫn: https://localhost:44343/Admin/Login.aspx để vào trang Admin -> sử dụng tài khoản mẫu Admin để đăng nhập vào trang quản trị
---
<img width="1920" height="1081" alt="a" src="https://github.com/user-attachments/assets/878ecf29-b1d7-48e0-800c-d406b6850d33" />

**Tài khoản mẫu:**  
- Admin: `admin` / `12345678`  
- User: `customer1` / `12345678`

Video setup dự án WebBanLapTop trên Visual Studio và Sql Server: https://www.youtube.com/watch?v=caGwDusFJEQ

---

## 5. Hướng phát triển  
- Thêm tính năng thanh toán online (VNPay, Momo).  
- Cải thiện responsive cho thiết bị di động.  
- Tích hợp API đánh giá sản phẩm.  

---

## 6. Tài liệu tham khảo  
- Giáo trình “Lập trình Web với ASP.NET” – Khoa CNTT.  
- Microsoft Docs: [https://learn.microsoft.com/aspnet/](https://learn.microsoft.com/aspnet/)  
- Các tài liệu và ví dụ tham khảo từ W3Schools, StackOverflow.

---

## 7. Liên hệ  
**Sinh viên thực hiện:** Vũ Trí Đăng  
- Email: dangvu8244@gmail.com  
- GitHub: https://github.com/Vutridang
- Điện thoại: 0788388749
- Link video thuyết trình: https://drive.google.com/file/d/1dpDEBDDC7JpJqX-kXYEDHXEGHYcWACCA/view?usp=drive_link
