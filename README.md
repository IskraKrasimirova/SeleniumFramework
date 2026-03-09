# SoA Selenium / RestSharp Framework (Endava SoA Training)


[![CI Status](https://github.com/IskraKrasimirova/SeleniumFramework/actions/workflows/ci.yml/badge.svg)](https://github.com/IskraKrasimirova/SeleniumFramework/actions/workflows/ci.yml)


This repository contains a **UI + API automation framework** built in C# using Selenium WebDriver, ReqNroll, RestSharp. The framework tests the **SoA SUT Application** — a microservices-based web platform for user management.

--- 

# 🏗️ System Under Test (SUT): SoA Application

This automation framework tests the **SoA Application** — a microservices-based user management system used in the Endava SoA Training.

The application consists of three Dockerized services:

- **PHP Frontend (Apache)** – http://localhost:8080  
- **Python REST API (Flask)** – http://localhost:5000  
- **MySQL Database** – localhost:3306  

It provides functionality for:
- User authentication  
- User CRUD operations  
- Country & city lookup  
- Skills management  

### 🔗 Full SUT Documentation  
The complete documentation for the SoA Application (architecture, API endpoints, Docker setup, database details, cleanup instructions, etc.) is available in the main application repository:

👉 **https://github.com/Endava-Sofia/SoA-SUT**  

### 🚀 Start the SUT Locally  
To run the application locally:

```bash
docker compose -f docker-compose-v2.yml up -d
```
### The application will be available at:
 - UI: http://localhost:8080
 - API: http://localhost:5000

### Default credentials:
 - Admin: admin@automation.com
 - Password: pass123

## Overview
This repository hosts a **Selenium-based BDD test automation framework** that is being iteratively developed during the **Endava SoA Training**.

The primary goal is to demonstrate **production-grade automation architecture**, tooling, and engineering practices using modern **.NET ecosystem** components.

The framework is intentionally designed as an educational yet **enterprise-ready reference implementation**.

In addition to UI automation, the repository has been expanded to include:
- A dedicated **API automation framework** using RestSharp
- A shared **Common library** that contains reusable cross-usable components

This allows the solution to scale beyond UI-only testing into a **multi-layer automation architecture**.

---

## Key Objectives
- Build a **maintainable and scalable** UI automation framework
- Apply **Behavior-Driven Development (BDD)** principles end-to-end
- Demonstrate **clean architecture**, **SOLID**, and **design patterns**
- Showcase **Dependency Injection (DI)** and configuration best practices
- Integrate UI automation with **API-level validation** where applicable
- Provide a foundation suitable for **CI/CD execution**
- Enable shared utilities and reusable test infrastructure across multiple test layers

---

## Technology Stack
| Area | Technology |
|----|----|
| Language | C# (.NET) |
| UI Automation | Selenium WebDriver |
| BDD | ReqNRoll |
| Dependency Injection | .NET DI / IoC containers |
| Configuration | appsettings / environment-based |
| Assertions | Build-in in nunit |
| Patterns | Page Object, Factory, Builder, Singleton |

---

## Framework Characteristics
- **BDD-first approach**
  - Feature files as the primary source of truth
  - Clear separation between *what* is tested and *how* it is implemented
- **Layered architecture**
  - Features → Steps → Pages → Infrastructure
- **Strong DI usage**
  - No static state
  - Fully injectable WebDriver, contexts, and services
- **Multi-layer test coverage**
  - UI tests validate user journeys
  - API tests validate backend/service behavior
  - Common components ensure consistency across both layers
- **Extensible by design**
  - Easy to add new browsers, environments, and test layers
- **Training-oriented evolution**
  - Codebase grows alongside the SoA training modules
  - Designed as a learning accelerator for scalable automation frameworks

---

## Solution Architecture

The repository is structured into **three core projects**, each responsible for a specific automation layer.

### 1. UI Automation Project (Selenium BDD)

This project contains the full **Selenium + Reqnroll** implementation, including:

- Gherkin feature files
- Step definitions
- Page Object Model abstractions
- WebDriver setup and hooks
- UI-specific test execution

**Purpose:** ✅ Validate end-to-end user flows through the browser.

### 2. API Automation Project (RestSharp)

A second project has been added for **API-level test coverage** using **RestSharp**.

It includes:

- API client abstractions
- Request builders
- Response models
- Service-level validations
- Integration-ready API test scenarios

**Purpose:** ✅ Validate backend endpoints independently or alongside UI flows.

### 3. Common Project (Shared Test Infrastructure)

A third project called **Common** has been introduced to host all **reusable framework components** shared across UI and API layers.

It contains:

- Configuration readers and environment loaders
- Utilities and helper services
- Common validation logic
- Cross-cutting framework abstractions

**Purpose:** ✅ Prevent duplication and ensure consistent architecture across frameworks.  
This project acts as the **foundation layer** of the entire automation solution.

---

## Repository Structure (High-Level)
```text
/Features        -> Gherkin feature files
/Steps           -> Step definitions
/Pages           -> Page Object abstractions
/Infrastructure  -> WebDriver, hooks, configuration, DI
/Tests           -> Test execution entry points
