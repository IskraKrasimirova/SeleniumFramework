# SeleniumFramework
# Selenium Test Automation Framework with C#

# Selenium BDD Framework (Endava SoA Training)

## Overview
This repository hosts a **Selenium-based BDD test automation framework** that is being **iteratively developed during the Endava SoA Training**.  
The primary goal is to demonstrate **production-grade automation architecture**, tooling, and engineering practices using modern .NET ecosystem components.

The framework is intentionally designed as an **educational yet enterprise-ready reference implementation**.

---

## Key Objectives
- Build a **maintainable and scalable** UI automation framework
- Apply **Behavior-Driven Development (BDD)** principles end-to-end
- Demonstrate **clean architecture**, **SOLID**, and **design patterns**
- Showcase **Dependency Injection (DI)** and configuration best practices
- Integrate UI automation with **API-level validation** where applicable
- Provide a foundation suitable for **CI/CD execution**

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
| Patterns | Page Object, Factory, Builder |

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
- **Extensible by design**
  - Easy to add new browsers, environments, and test layers
- **Training-oriented evolution**
  - Codebase grows alongside the SoA training modules

---

## Repository Structure (High-Level)
```text
/Features        -> Gherkin feature files
/Steps           -> Step definitions
/Pages           -> Page Object abstractions
/Infrastructure  -> WebDriver, hooks, configuration, DI
/Tests           -> Test execution entry points
