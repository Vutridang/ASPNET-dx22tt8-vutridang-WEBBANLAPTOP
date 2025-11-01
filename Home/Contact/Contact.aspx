<%@ Page Title="Liên hệ" Language="C#" MasterPageFile="~/Home/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="WebBanLapTop.Home.Contact" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .contact-container {
            max-width: 600px;
            margin: 40px auto;
            padding: 30px 40px;
            background: #f9f9f9;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            color: #333;
        }
        .contact-container h2 {
            text-align: center;
            margin-bottom: 25px;
            font-weight: 700;
            font-size: 28px;
            color: #222;
        }
        .contact-container p {
            font-size: 16px;
            line-height: 1.5;
            margin: 12px 0;
        }
        .social-links {
            margin-top: 35px;
            text-align: center;
        }
        .social-links a {
            margin: 0 18px;
            text-decoration: none;
            font-size: 22px;
            font-weight: 600;
            color: #555;
            transition: color 0.3s ease;
            display: inline-flex;
            align-items: center;
            gap: 8px;
        }
        .social-links a i {
            font-size: 28px;
        }
        .social-links a:hover {
            color: #007bff;
        }
    </style>

    <div class="contact-container">
        <h2>Thông tin liên hệ</h2>

        <p><strong>Địa chỉ:</strong> 123 Đường ABC, Quận XYZ, TP.HCM</p>
        <p><strong>Điện thoại:</strong> (028) 1234 5678</p>
        <p><strong>Email:</strong> support@webbanlaptop.com</p>

        <h3 style="margin-top: 40px; text-align:center; font-weight: 600; color: #222;">Kết nối với chúng tôi</h3>
        <div class="social-links">
            <a href="https://facebook.com/yourpage" target="_blank" rel="noopener noreferrer">
                <i class="fab fa-facebook-square" style="color:#3b5998;"></i> Facebook
            </a>
            <a href="https://twitter.com/yourhandle" target="_blank" rel="noopener noreferrer">
                <i class="fab fa-twitter-square" style="color:#1DA1F2;"></i> X
            </a>
            <a href="https://instagram.com/yourprofile" target="_blank" rel="noopener noreferrer">
                <i class="fab fa-instagram" style="color:#E4405F;"></i> Instagram
            </a>
        </div>
    </div>

    <!-- FontAwesome CDN for social icons -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
</asp:Content>
