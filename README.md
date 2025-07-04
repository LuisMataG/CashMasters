# 💰 CashMasters - Change Calculator Console App

CashMasters is a console-based currency change calculator designed not only to solve the problem of returning optimal change, but to demonstrate solid software engineering principles such as clean architecture, SOLID principles, unit testing, and extensibility.

## 📌 Project Goals

Although this is a console application, the core goal was to develop a professional-grade software solution focused on:

1. ✅ **Clean Architecture & SOLID Principles**
2. 🔧 **Extensibility & Configuration**
3. 🧪 **Automated Unit Testing**
4. 🧑‍💻 **User Experience**
5. 📘 **Code Legibility & Mentorship-Oriented Design**

---

## 🧠 Technical Highlights

- **SOLID**: All 5 principles implemented across the architecture.
- **Design Patterns**: `Factory` pattern for currency configuration and `Dependency Inversion` for loosely-coupled services.
- **Layered Structure**: Clear separation of concerns (`Core`, `Interfaces`, `Configuration`, `Tests`, `ConsoleApp`).
- **Persistent Configuration**: Stores last-used currency using a decoupled `JsonConfigManager`.
- **Testability**: Easily testable with injected interfaces and business logic decoupled from UI.
- **Extensibility**: Adding a new currency is straightforward by extending configuration.

---

## 🧪 Unit Tests

The solution includes a comprehensive suite of unit tests built with **xUnit**, covering:

- Valid scenarios for multiple currencies.
- Error cases (invalid prices, payment shortages, unresolvable change).
- Boundary validations and exception handling.
