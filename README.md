# plate_recognition_2022

> **Nhận Dạng Biển Số Xe - C# Windows Forms 2022**

---

## Giới thiệu

Dự án **Nhận dạng biển số xe** được phát triển bằng **C# (.NET Framework 4.7.2)** trên nền **Windows Forms Application**.

Ứng dụng sử dụng:

- **EmguCV** (giao diện .NET của OpenCV) để xử lý ảnh.
- **Tesseract OCR** để nhận dạng ký tự.

Chức năng chính:

- Tiền xử lý ảnh
- Xác định vùng chứa biển số
- Nhận diện ký tự trên biển số xe

---

## ✨ Tính năng nổi bật

- Mở ảnh từ máy tính hoặc chụp ảnh bằng webcam.
- Tự động phát hiện vị trí biển số trong ảnh.
- Phân đoạn và nhận diện ký tự trên biển số.
- Hiển thị kết quả biển số sau khi nhận dạng.
- Hỗ trợ nhận dạng biển số xe Việt Nam.

---

## 🛠️ Công nghệ sử dụng

|       Công nghệ       |                Vai trò                |
| :-------------------: | :-----------------------------------: |
|   C# Windows Forms    | Xây dựng giao diện và xử lý nghiệp vụ |
|        EmguCV         |   Xử lý ảnh, phát hiện vùng biển số   |
|     Tesseract OCR     |        Nhận diện ký tự từ ảnh         |
| .NET Framework 4.7.2+ |          Nền tảng phát triển          |

---

## 🚀 Cách sử dụng

1. **Chạy ứng dụng** trong Visual Studio 2022.
2. **Chọn**:
   - `Mở ảnh`: Tải ảnh biển số xe từ máy tính.
   - `Chụp ảnh`: Sử dụng webcam để chụp ảnh mới.
3. **Nhấn "Nhận dạng"** để hệ thống thực hiện:
   - Tiền xử lý ảnh.
   - Phát hiện vùng chứa biển số.
   - Phân đoạn và nhận diện ký tự bằng Tesseract OCR.
4. **Xem kết quả** biển số nhận dạng được ngay trên giao diện.

---

## 📂 Cấu trúc thư mục

```bash
/plate_recognition_2022
 ├── /Images          #  Ảnh mẫu đầu vào
 ├── /Resources       #  File traineddata của Tesseract
 ├── FormMain.cs      #  Giao diện chính và sự kiện
 ├── LicensePlateDetector.cs #  Xử lý nhận dạng biển số
 └── README.md        #  File hướng dẫn
```

---

## 📧 Liên hệ

- **Tên**: Tăng Gia Huy
- **Email**: [tanggiahuy.developer@gmail.com](mailto:tanggiahuy.developer@gmail.com)
- **GitHub**: [github.com/TangGiaHuy-Developer](https://github.com/TangGiaHuy-Developer)
