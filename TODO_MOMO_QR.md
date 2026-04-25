# TODO: Tích hợp VietQR / MoMo QR trên trang Hóa đơn

## Mục tiêu
Khi trả phòng, mã QR trên hóa đơn có thể quét bằng MoMo và hiển thị đúng số tiền cần trả (pre-fill amount).

## Các bước thực hiện

### Backend
- [x] 1. Tạo `Models/VietQRConfig.cs` — POCO cấu hình BankId, AccountNo, AccountName
- [x] 2. Tạo `Models/VietQRResponse.cs` — DTO trả về qrImageUrl, amount, v.v.
- [x] 3. Tạo `Interfaces/IVietQRService.cs` — Interface sinh QR
- [x] 4. Tạo `Services/VietQRService.cs` — Build URL VietQR từ img.vietqr.io
- [x] 5. Tạo `Controllers/VietQRController.cs` — Endpoint `GET /api/vietqr/{invoiceId}`
- [x] 6. Sửa `appsettings.json` — Thêm section `VietQR` (demo MB Bank)
- [x] 7. Sửa `Program.cs` — Register `IVietQRService` / `VietQRService`

### Frontend
- [x] 8. Tạo `src/api/momoAPI.js` — API client gọi `/api/vietqr/{invoiceId}`
- [x] 9. Sửa `InvoicePage.jsx` — Bỏ QR hardcode, gọi API lấy QR động, render `<img>`

## Ghi chú
- Sử dụng API miễn phí `https://img.vietqr.io` để sinh QR chuẩn EMVCo.
- STK demo: MB Bank `970422` / `0000123456789` — chỉ cần đổi trong `appsettings.json` khi dùng thật.

