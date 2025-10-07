# CarRentalDemo - Car Rental Request System

## Project Overview

GTS Demo is an ASP.NET Web Forms application (.NET Framework 4.8) that provides a rental car request management system.  
The application supports both Turkish and English languages and includes:

- User authentication (login and registration)  
- Rental request creation and listing  
- Multi-language interface (Turkish and English)  
- Client and server-side date validation using jQuery UI datepicker  
- Dynamic vehicle type and model selection  
- Responsive and mobile-friendly design  
- JSON-based data persistence  

### Date Validation Rules

The system includes both **client-side** and **server-side** date validation to ensure accurate and realistic rental periods.  
The following validation rules are enforced:

- The **start date** (rental begin date) can be selected up to **3 years from the current date**.  
- The **rental duration** cannot exceed **3 months (90 days)**.  
- Therefore, the **end date** can be at most **3 months after the start date**.  
- If a user selects a **start date that is later than the end date**, the system automatically updates the end date to be **one day after the new start date**.  
- Past dates cannot be selected.  

These rules are implemented through both **ASP.NET validators** and **jQuery UI datepicker** logic to maintain consistent validation behavior on both client and server sides.

---

## Project Structure

```
gts_demo/
├── App_Data/                   # Data storage (JSON files)
│   ├── rentals.json           # Rental requests data
│   └── users.json             # User accounts data
├── Models/                    # Data models
├── Services/                  # Business logic services
├── Scripts/                   # JavaScript files
├── Content/                   # CSS stylesheets
├── Assets/                    # Icons and images
├── *.aspx                     # Web Forms pages
├── *.aspx.cs                  # Code-behind files
└── Web.config                 # App configuration
```

---

## Prerequisites

Before running the project, ensure the following are installed:

- Visual Studio 2019 or later (with ASP.NET and web development workload)  
- .NET Framework 4.8 or higher  
- IIS Express (included with Visual Studio)  
- Modern web browser (Chrome, Edge, Firefox, or Safari)

---

## Installation and Setup

### 1. Clone or Download the Project

```bash
# Clone with Git
git clone [repository-url]

# OR download and extract the ZIP file
```

### 2. Open in Visual Studio

1. Launch Visual Studio  
2. Go to **File > Open > Project/Solution**  
3. Select the `gts_demo.csproj` file  

### 3. Restore NuGet Packages

- Right-click on the solution in Solution Explorer  
- Select **“Restore NuGet Packages”**  
- Wait for all packages to be restored  

### 4. Build the Solution

- Go to **Build > Build Solution** (or press `Ctrl+Shift+B`)  
- Ensure there are no build errors  

---

## Running the Application

### Method 1: Using Visual Studio

1. Press **F5** or click **“Start Debugging”**  
2. The application will open in your default browser  
3. The URL will typically be `https://localhost:44375/` or similar  

---

## Default Demo Account

Use the following credentials to log in regardless of language preference:

```
Email: admin@rentacar.com
Password: Admin123
```
