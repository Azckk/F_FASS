# F_FASS

## 项目简介
F_FASS 是一个基于 .NET 的模块化后端系统，采用分层架构设计，使用 GitHub Packages 作为私有 NuGet 包管理方案

该项目通过将核心能力、业务逻辑和扩展模块拆分为独立组件，提升系统的可维护性与扩展性

---

## 项目结构

```text
F_FASS
├── Develop
│   └── SourceCode
│       ├── A_Backend
│       │   ├── FASS.Web.Api      # API入口（ASP.NET Core）
│       │   ├── FASS.Service      # 业务逻辑层
│       │   ├── FASS.Extend.*     # 扩展模块（设备/系统集成）
│       │   └── ...
│       ├── A_Frontend            # 前端项目
│       └── NuGet.Config          # 包源配置（GitHub Packages）