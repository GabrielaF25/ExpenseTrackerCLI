# Expense Tracker

A comprehensive expense tracking solution built with .NET 8, offering both API and CLI interfaces for managing personal or business expenses with multi-currency support.

## Features

- **Multi-Currency Support**: 
  - Handles multiple currencies (RON, EUR, USD, GBP)
  - Real-time currency conversion capabilities
  - Support for fixed exchange rates on specific dates

- **RESTful API Endpoints**:
  - Complete CRUD operations for expenses
  - Currency conversion endpoints
  - Structured response handling with validation

- **Expense Management**:
  - Create and track expenses with detailed information
  - Categorize expenses by type
  - Store essential details including title, description, amount, and creation date
  - Support for both base currency and converted currency tracking

## Technical Stack

- **.NET 8.0**
- **C# 12.0**
- **Clean Architecture**
  - Separated API and CLI projects
  - Service-based architecture
  - Repository pattern for data access
  - AutoMapper for DTO mappings
  - Dependency Injection

## Project Structure

- **ExpenseTrackerApi**: RESTful API implementation
- **ExpenseTrackerCLI**: Command-line interface application
- **ExpenseProjectNUnitTests**: Comprehensive test suite

The system is designed with scalability in mind, featuring robust error handling, input validation, and a flexible architecture for future extensions.
