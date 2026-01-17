# Order Management Application

## Giới Thiệu (Overview)

Ứng dụng **Order Management** là một hệ thống quản lý đơn hàng được xây dựng bằng **ASP.NET Core 10.0** và **Entity Framework Core 9.0**, thực hiện các yêu cầu của bài thi FIT4016 Câu 1 và Câu 2.

### Chức Năng Chính (Key Features)

- ✅ **CRUD Operations**: Tạo (Create), Đọc (Read), Cập nhật (Update), Xóa (Delete) đơn hàng
- ✅ **Database**: SQL Server với Code-First migrations
- ✅ **Validation**: Kiểm tra dữ liệu (Số đơn hàng, Email, Số lượng, Ngày, v.v.)
- ✅ **Pagination**: Hiển thị 10 đơn hàng trên mỗi trang
- ✅ **Search**: Tìm kiếm theo Mã đơn hàng hoặc Tên khách hàng
- ✅ **Status Tracking**: Hiển thị trạng thái (Pending/Delivered)
- ✅ **Seed Data**: Khởi tạo 15 sản phẩm và 40 đơn hàng mẫu

---

## Cấu Trúc Dự Án (Project Structure)

```
FIT4016-KiemTra-2026/
├── OrderManagementApp/
│   ├── Controllers/
│   │   └── OrdersController.cs          # CRUD operations
│   ├── Models/
│   │   ├── Entities/
│   │   │   ├── Order.cs                 # Entity model - Đơn hàng
│   │   │   └── Product.cs               # Entity model - Sản phẩm
│   │   ├── OrderManagementContext.cs    # DbContext
│   │   └── ErrorViewModel.cs
│   ├── Views/
│   │   ├── Orders/
│   │   │   ├── Index.cshtml             # Danh sách đơn hàng (có phân trang & tìm kiếm)
│   │   │   ├── Create.cshtml            # Form tạo đơn hàng
│   │   │   ├── Edit.cshtml              # Form chỉnh sửa đơn hàng
│   │   │   └── Delete.cshtml            # Xác nhận xóa đơn hàng
│   │   └── Shared/
│   ├── Data/
│   │   └── SeedData.cs                  # Khởi tạo dữ liệu (15 sản phẩm + 40 đơn)
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── wwwroot/
│   │   ├── css/
│   │   └── js/
│   ├── appsettings.json                 # Database connection string
│   ├── appsettings.Development.json
│   ├── Program.cs                       # Cấu hình ứng dụng
│   └── OrderManagementApp.csproj
├── FIT4016-KiemTra-2026.sln
├── README.md                            # File này
├── .gitignore
└── Connection String (xem appsettings.json)
```

---

## Yêu Cầu Hệ Thống (System Requirements)

- **Runtime**: .NET 10.0 (Hoặc cao hơn)
- **Database**: SQL Server 2019 hoặc cao hơn
- **IDE**: Visual Studio Code / Visual Studio 2022+

### Cài Đặt (Installation)

1. **Cài đặt .NET SDK**:
   ```bash
   # Kiểm tra version .NET hiện tại
   dotnet --version
   ```

2. **Clone / Mở Project**:
   ```bash
   cd FIT4016-KiemTra-2026
   ```

3. **Restore Dependencies**:
   ```bash
   dotnet restore
   ```

---

## Cấu Hình (Configuration)

### 1. Connection String

Mở file `appsettings.json` trong thư mục `OrderManagementApp`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=DUCK;Database=OrderManagement;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Thay đổi**:
- `Server=DUCK` → Tên SQL Server instance của bạn (ví dụ: `localhost`, `(localdb)\\mssqllocaldb`)
- `Database=OrderManagement` → Tên database (giữ nguyên hoặc thay đổi)
- `Trusted_Connection=True` → Sử dụng Windows Authentication

### 2. Đồng Bộ Database (Database Migrations)

```bash
cd OrderManagementApp
dotnet ef database update
```

Hoặc tự động khi ứng dụng khởi động.

---

## Chạy Ứng Dụng (Running the Application)

### Phương Pháp 1: Sử dụng Terminal

```bash
cd OrderManagementApp
dotnet run
```

Ứng dụng sẽ chạy tại: **http://localhost:5109**

### Phương Pháp 2: Sử dụng Visual Studio / VS Code

1. Mở project
2. Nhấn `F5` hoặc chọn **Run & Debug**
3. Trình duyệt tự động mở tại `http://localhost:5109/orders`

---

## Hướng Dẫn Sử Dụng (Usage Guide)

### 📋 Xem Danh Sách Đơn Hàng (List Orders)
- Truy cập: `http://localhost:5109/orders`
- Hiển thị 10 đơn hàng trên mỗi trang (phân trang)
- Tìm kiếm theo:
  - **Mã Đơn Hàng** (Order Number): Ví dụ: `ORD-20250116-0001`
  - **Tên Khách Hàng** (Customer Name): Ví dụ: `Nguyễn Văn A`

### ➕ Tạo Đơn Hàng (Create Order)
1. Nhấn **"Create New"**
2. Điền thông tin:
   - **Customer Name**: Tên khách hàng (2-100 ký tự)
   - **Customer Email**: Email khách hàng (định dạng email đúng, không trùng)
   - **Product**: Chọn sản phẩm từ dropdown
   - **Quantity**: Số lượng (1 đến số lượng tồn kho của sản phẩm)
   - **Order Date**: Ngày đặt hàng (không thể là ngày tương lai)
   - **Delivery Date**: Ngày giao hàng (tùy chọn, ≥ Order Date)

3. Nhấn **"Create"**

**Quy tắc Validation**:
- Mã đơn hàng tự động sinh: `ORD-YYYYMMDD-XXXX`
- Email phải duy nhất trong hệ thống
- Số lượng không vượt quá tồn kho
- Ngày giao phải ≥ Ngày đặt hàng

### ✏️ Chỉnh Sửa Đơn Hàng (Edit Order)
1. Chọn đơn hàng và nhấn **"Edit"**
2. Có thể chỉnh sửa:
   - ✅ Tên khách hàng
   - ✅ Email khách hàng
   - ✅ Số lượng (vẫn phải ≤ tồn kho)
   - ✅ Ngày giao hàng
   - ❌ Không thể thay đổi: Mã đơn, Sản phẩm, Ngày đặt

3. Nhấn **"Save"**

### 🗑️ Xóa Đơn Hàng (Delete Order)
1. Chọn đơn hàng và nhấn **"Delete"**
2. Xác nhận xóa trên trang confirmation
3. Nhấn **"Delete"** để xóa vĩnh viễn

---

## Cấu Trúc Dữ Liệu (Database Schema)

### Bảng Products (Sản Phẩm)
| Cột | Kiểu | Ghi Chú |
|-----|------|--------|
| ProductId | INT | Primary Key, Auto-increment |
| ProductName | NVARCHAR(100) | Tên sản phẩm (bắt buộc) |
| Description | NVARCHAR(MAX) | Mô tả sản phẩm |
| Price | DECIMAL(10,2) | Giá sản phẩm |
| StockQuantity | INT | Số lượng tồn kho |

### Bảng Orders (Đơn Hàng)
| Cột | Kiểu | Ghi Chú |
|-----|------|--------|
| OrderId | INT | Primary Key, Auto-increment |
| OrderNumber | NVARCHAR(20) | Mã đơn hàng (UNIQUE, định dạng: ORD-YYYYMMDD-XXXX) |
| CustomerName | NVARCHAR(100) | Tên khách hàng (2-100 ký tự) |
| CustomerEmail | NVARCHAR(100) | Email khách hàng (UNIQUE, định dạng email) |
| ProductId | INT | Foreign Key → Products |
| Quantity | INT | Số lượng (1-999) |
| OrderDate | DATETIME | Ngày đặt hàng (không thể là tương lai) |
| DeliveryDate | DATETIME (NULL) | Ngày giao hàng (tùy chọn, ≥ OrderDate) |
| Status | NVARCHAR(20) | Computed: "Pending" hoặc "Delivered" |

---

## Quy Tắc Validation (Validation Rules)

### Tạo Đơn Hàng

| Quy Tắc | Lỗi | Mô Tả |
|---------|-----|-------|
| Order Number Format | `Invalid order number format` | Phải có dạng `ORD-YYYYMMDD-XXXX` |
| Email Format | `Invalid email format` | Phải là email hợp lệ (ví dụ: abc@gmail.com) |
| Email Uniqueness | `Email already exists` | Email không được trùng với đơn hàng khác |
| Customer Name Length | `Customer name must be 2-100 characters` | Tên phải có 2-100 ký tự |
| Quantity Range | `Quantity must be 1-[StockQty]` | Số lượng phải hợp lệ |
| Order Date | `Order date cannot be in the future` | Ngày đặt không được là ngày tương lai |
| Delivery Date | `Delivery date must be >= Order date` | Ngày giao ≥ Ngày đặt |
| Stock Available | `Insufficient stock for this product` | Số lượng không vượt quá tồn kho |

---

## Dữ Liệu Khởi Tạo (Seed Data)

### Sản Phẩm
15 sản phẩm mẫu được khởi tạo:
- Laptop, Desktop Computer, Monitor, Keyboard, Mouse, v.v.
- Giá từ 100,000 đến 50,000,000 VNĐ
- Tồn kho từ 5 đến 500 cái

### Đơn Hàng
40 đơn hàng mẫu được khởi tạo:
- **70%** có Delivery Date (Trạng thái: Delivered)
- **30%** không có Delivery Date (Trạng thái: Pending)
- Tên khách hàng ngẫu nhiên nhưng hợp lý (Nguyễn Văn A, Trần Thị B, v.v.)
- Email duy nhất (customer1000@gmail.com, customer1001@gmail.com, v.v.)
- Ngày đặt hàng ngẫu nhiên trong 30 ngày qua

---

## Công Nghệ & Stack

| Công Nghệ | Phiên Bản | Mục Đích |
|-----------|----------|---------|
| .NET | 10.0 | Runtime & Framework |
| ASP.NET Core | 10.0 | Web Framework (MVC) |
| Entity Framework Core | 9.0 | ORM - Database Access |
| SQL Server | 2019+ | Database |
| Bootstrap | 5.3 | UI Framework (Frontend Styling) |
| Razor | Latest | View Engine (HTML Templating) |

---

## Troubleshooting (Khắc Phục Sự Cố)

### 1. Lỗi: "Cannot connect to server"
**Giải Pháp**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=OrderManagement;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```
Hoặc sử dụng tên server đúng của bạn.

### 2. Lỗi: "Database does not exist"
**Giải Pháp**:
```bash
cd OrderManagementApp
dotnet ef database update
```

### 3. Lỗi: "Port 5109 is already in use"
**Giải Pháp**:
```bash
# Đóng application hiện tại hoặc chỉnh sửa port trong launchSettings.json
```

### 4. Lỗi: "Migration pending"
**Giải Pháp**:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## Đánh Giá (Evaluation Criteria)

Dự án này đáp ứng các tiêu chí:
- ✅ **Code Quality**: XML Documentation, Clean Code, Naming Conventions
- ✅ **CRUD Operations**: Create, Read, Update, Delete đầy đủ
- ✅ **Validation**: Kiểm tra dữ liệu toàn diện
- ✅ **Database**: Entity Framework Code-First, Migrations
- ✅ **UI/UX**: Bootstrap styling, User-friendly interface
- ✅ **Error Handling**: Try-catch, Validation messages
- ✅ **Seed Data**: 15+ products, 40+ orders
- ✅ **Pagination & Search**: 10 items/page, Search filters

---

## Tác Giả (Author)

**Sinh viên FIT4016**  
**Ngày**: 17/01/2026

---

## Liên Hệ & Hỗ Trợ (Support)

Nếu có vấn đề hoặc câu hỏi, vui lòng kiểm tra:
1. Connection string trong `appsettings.json`
2. SQL Server instance đang chạy
3. Bảng Products và Orders tồn tại trong database
4. Logs trong terminal khi chạy ứng dụng

---

**Version**: 1.0  
**Status**: Hoàn thành / Complete
