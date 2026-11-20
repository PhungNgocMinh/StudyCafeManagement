# Study Cafe Management

Study Cafe Management là ứng dụng quản lý quán cà phê và không gian học tập
dành cho Windows. Chương trình sử dụng Windows Forms, .NET Framework 4.7.2,
Entity Framework 6 và SQL Server để quản lý bàn, món, nguyên liệu, công thức,
khách hàng, tài khoản nhân viên, đơn hàng, thanh toán và báo cáo.

## Tính năng

- Quản lý danh mục món, sản phẩm, bàn, khách hàng, nguyên liệu và công thức.
- Quản lý tài khoản nhân viên và phân quyền theo vai trò.
- Sơ đồ bàn theo từng tầng và hóa đơn mua mang đi.
- Tìm kiếm, chọn món, thêm/xóa món trong hóa đơn.
- Kiểm tra tồn kho theo công thức, thanh toán, trừ kho và cộng điểm khách hàng.
- Báo cáo doanh thu và top 5 mặt hàng bán chạy theo khoảng ngày.
- Tự động cập nhật trạng thái bàn từ trống sang đang phục vụ và ngược lại.

## Yêu cầu

- Windows
- Visual Studio hỗ trợ phát triển Desktop ứng dụng .NET Framework
- .NET Framework 4.7.2
- SQL Server hoặc SQL Server Express
- SQL Server Management Studio (SSMS)
- Windows Authentication cho SQL Server instance mặc định `.`

## Cài đặt

### 1. Tạo cơ sở dữ liệu

1. Mở SQL Server Management Studio.
2. Kết nối tới SQL Server instance được dùng bởi ứng dụng.
3. Mở file `StudyCafeManagement\Create_StudyCafeManagement_Database.sql`.
4. Chọn `Execute` để chạy toàn bộ script.

Script tạo database `QuanLyCaPheHocTap`, các bảng dữ liệu, ràng buộc và dữ liệu
mẫu. Script chỉ dùng để cài đặt mới; nếu database đã tồn tại, script sẽ dừng lại
và không thay đổi dữ liệu hiện có.

### 2. Cấu hình kết nối

Chuỗi kết nối mặc định trong `StudyCafeManagement\App.config` là:

```xml
Data Source=.;Initial Catalog=QuanLyCaPheHocTap;Integrated Security=True;TrustServerCertificate=True
```

Nếu SQL Server của máy chạy ở một instance khác, cập nhật phần `Data Source`
trong:

- `StudyCafeManagement\App.config`
- `StudyCafeManagement\Forms\FrmLogin.cs`
- Các form báo cáo sử dụng chuỗi kết nối SQL riêng

### 3. Mở và khởi chạy chương trình

1. Mở `StudyCafeManagement.sln` bằng Visual Studio.
2. Chọn **Restore NuGet Packages** nếu Visual Studio chưa tự khôi phục.
3. Entity Framework được khai báo trong
   `StudyCafeManagement\packages.config`.
4. Chọn cấu hình `Debug` và nền tảng `Any CPU`.
5. Nhấn `F5` hoặc chọn **Start** để chạy.

## Tài khoản mặc định

Script cơ sở dữ liệu tạo sẵn ba tài khoản. Tất cả đều có mật khẩu `123456`.

| Tài khoản | Vai trò |
|---|---|
| `admin` | Quản trị viên |
| `thungan01` | Thu ngân |
| `phache01` | Pha chế |

Khuyến nghị đổi mật khẩu sau khi đăng nhập lần đầu.

## Sử dụng chương trình

### 1. Đăng nhập

Khi khởi động, ứng dụng mở màn hình đăng nhập `FrmLogin`. Nhập tên đăng nhập và
mật khẩu, sau đó chọn **Đăng nhập**. Tài khoản bị khóa sẽ không thể đăng nhập.
Sau khi đăng nhập thành công, tên tài khoản và vai trò được hiển thị ở thanh
trạng thái của cửa sổ chính.

<img width="502" height="452" alt="login" src="https://github.com/user-attachments/assets/d7783c0c-9893-441e-ae49-fb2d63688e73" />

### 2. Quyền theo vai trò

- **Admin:** truy cập đầy đủ danh mục, sản phẩm, bàn, khách hàng, nguyên liệu,
  công thức, tài khoản, nhóm quyền, nghiệp vụ và báo cáo.
- **Thu ngân:** mở bàn và gọi món, thanh toán, xem doanh thu và top mặt hàng.
- **Pha chế:** mở bàn và gọi món.

Chọn **Hệ thống > Đăng xuất** để đăng xuất. Chọn **Hệ thống > Đổi mật khẩu** để
thay đổi mật khẩu của tài khoản đang đăng nhập.

<img width="1280" height="720" alt="role" src="https://github.com/user-attachments/assets/03681dad-dfba-41c3-8c98-d142e9d1dcbf" />

### 3. Quản lý danh mục

Trong menu **Quản lý danh mục**, Admin có thể:

- Thêm, sửa, xóa danh mục món.
- Thêm, sửa, xóa sản phẩm.
- Cập nhật giá, trạng thái và hình ảnh của sản phẩm.
- Thêm, sửa, xóa bàn và giá thuê bàn theo giờ.
- Thêm, sửa, xóa khách hàng.
- Quản lý nguyên liệu tồn kho.
- Cấu hình công thức nguyên liệu cho từng sản phẩm.
- Quản lý tài khoản nhân viên và nhóm quyền trong menu **Hệ thống**.

Mã danh mục, sản phẩm và bàn thường được sinh tự động. Tên món và đơn giá phải
hợp lệ trước khi lưu. Trạng thái sản phẩm có thể là `Còn`, `Hết` hoặc
`Ngưng bán`.

#### Hình ảnh sản phẩm

Trong form **Sản phẩm**, chọn **Chọn...** để duyệt file ảnh `.jpg`, `.jpeg`,
`.png` hoặc `.bmp`. Cách đơn giản nhất là đặt ảnh trong thư mục `Images` ở thư
mục gốc repository, sau đó lưu tên file ảnh trong trường hình ảnh. Script cơ sở
dữ liệu đã có sẵn các tên ảnh mẫu trong thư mục `Images`.

<img width="1280" height="720" alt="category" src="https://github.com/user-attachments/assets/d5c9dd73-2b12-439d-85df-ff96a60b96bb" />

<img width="1280" height="720" alt="customer" src="https://github.com/user-attachments/assets/656e9358-e1fc-4897-af05-b707520e50e5" />

<img width="1280" height="720" alt="table" src="https://github.com/user-attachments/assets/fdf55bf0-6564-417f-9b43-97b5ae108857" />

<img width="1280" height="720" alt="customer" src="https://github.com/user-attachments/assets/aade493a-9f11-4e37-8ce4-9d327c99d872" />

<img width="1280" height="720" alt="ingredient" src="https://github.com/user-attachments/assets/b2c730d4-c757-4c41-8b38-9d40ee441e3b" />

<img width="1280" height="720" alt="recipe" src="https://github.com/user-attachments/assets/d63e4e6a-84ad-4f01-80bc-a718868e754f" />

<img width="1280" height="720" alt="employee" src="https://github.com/user-attachments/assets/8cbafb31-af8e-431e-a9fd-e5dd682b225e" />

<img width="1280" height="720" alt="role" src="https://github.com/user-attachments/assets/77bffe96-0fe5-43bf-b68a-143732e76ae0" />

### 4. Mở bàn và gọi món

Chọn **Nghiệp vụ > Sơ đồ bàn & gọi món**.

1. Chọn tầng và loại bàn trong sơ đồ.
2. Chọn bàn đang trống, sau đó xác nhận **Mở bàn**.
3. Chọn danh mục hoặc nhập tên món để lọc danh sách.
4. Chọn món, số lượng và ghi chú nếu cần, sau đó chọn **Thêm**.
5. Chọn món trong danh sách hóa đơn để xóa hoặc điều chỉnh khi cần.
6. Chọn **Thanh toán** để mở form thanh toán.

Bàn đang được sử dụng có màu khác với bàn trống. Một bàn không thể mở thêm hóa
đơn mới khi đang có hóa đơn `Đang order`.

<img width="1280" height="720" alt="order" src="https://github.com/user-attachments/assets/dda9a387-00c4-487f-9203-4c1b50d6d688" />

### 5. Mua mang đi

Trong form **Sơ đồ bàn & gọi món**, chọn chế độ **Mua mang đi** thay vì chọn bàn.
Thêm sản phẩm vào hóa đơn và mở thanh toán như với một đơn tại bàn. Đơn mang đi
không tính tiền thuê bàn.

### 6. Thanh toán

Thanh toán từ hóa đơn `Đang order` bằng nút **Thanh toán**.

- Với đơn tại bàn: hệ thống tính thời gian sử dụng theo giờ và tiền thuê bàn.
- Với đơn mang đi: không tính tiền bàn.
- Nhập số điện thoại khách hàng để áp dụng thông tin khách.
- Khách hàng `Thân thiết` được giảm 10% trên tổng tiền.
- Thanh toán thành công sẽ trừ nguyên liệu theo công thức, cộng điểm tích lũy,
  khóa hóa đơn và giải phóng bàn.

Nếu cần hủy hóa đơn chưa thanh toán, dùng nút **Hủy hóa đơn** trong form gọi món.
Bàn liên quan sẽ được đưa về trạng thái `Trống`.

<img width="682" height="644" alt="payment" src="https://github.com/user-attachments/assets/0e380a44-0e46-4f47-9c81-e70f20cc347f" />

### 7. Xem báo cáo

Chọn các mục trong menu **Nghiệp vụ**:

- **Doanh thu:** chọn ngày bắt đầu và ngày kết thúc, sau đó chọn **Thống kê**.
  Báo cáo chỉ lấy hóa đơn đã thanh toán và hiển thị doanh thu theo ngày.
- **Mặt hàng bán chạy:** chọn khoảng ngày và xem top 5 sản phẩm theo số lượng
  đã bán trong các hóa đơn đã thanh toán.
  
<img width="1280" height="720" alt="income" src="https://github.com/user-attachments/assets/b09aa313-5a7b-486b-8b4e-27a8e401f8fb" />

<img width="1280" height="720" alt="bestSeller" src="https://github.com/user-attachments/assets/8b555c9d-941a-4a16-a785-a102e442b0f0" />

## Cấu trúc thư mục

```text
StudyCafeManagement.sln
StudyCafeManagement\
  Create_StudyCafeManagement_Database.sql
  App.config
  CoffeeShopDbContext.cs
  Forms\
  Helpers\
  Models\
  Properties\
Images\
README.md
```

## Xử lý sự cố

- **Không kết nối được SQL Server:** kiểm tra SQL Server đang chạy, tên instance
  trong chuỗi kết nối và quyền Windows Authentication.
- **Database đã tồn tại:** script cài đặt mới chủ động dừng để tránh ghi đè dữ
  liệu; không chạy lại script khi database đã có.
- **Đăng nhập thất bại:** kiểm tra tài khoản đang `Đang hoạt động`, tên đăng nhập
  và mật khẩu `123456` cho tài khoản mẫu.
- **Không thấy ảnh sản phẩm:** kiểm tra file nằm trong thư mục `Images` hoặc
  nhập đúng tên file trong trường hình ảnh.
- **Thiếu package Entity Framework:** chọn **Restore NuGet Packages** hoặc chạy
  `NuGet Package Restore` cho solution trong Visual Studio.
