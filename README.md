# PigGoGo

## 项目简介

PigGoGo是一个基于Unity引擎开发的游戏项目，使用SFramework框架构建。

## 技术架构

- 游戏引擎：Unity
- 框架：SFramework
- 编程语言：C#

## 项目结构

项目主要包含以下部分：

- `Assets/Script`: 包含游戏的所有脚本文件
  - `App`: 包含游戏应用程序的核心逻辑
    - `GameApp.cs`: 游戏启动器，负责初始化游戏和加载必要的控制器
  - 其他脚本模块（如UI、游戏逻辑、工具等）

## 核心框架

项目使用SFramework框架，该框架提供了以下功能：

- 控制器管理系统（通过`SBundleManager`加载和管理）
- 静态资源管理（通过`SFStaticsControl`访问）
- 调试工具（通过`SRDebug`）

## 启动流程

1. 游戏启动时，首先执行`GameApp.Initialize()`方法
2. 初始化调试工具（`SRDebug.Init()`）
3. 检查当前场景，如果不是"Persisitence"场景，则加载该场景
4. 通过`installBundle()`方法初始化所有控制器
5. 按顺序加载语言控制器、物品栏控制器和主页控制器

## 控制器系统

游戏使用控制器系统来管理不同的功能模块：
- 语言控制器（`App_Language_LanguageControl`）：管理游戏的多语言支持
- 物品栏控制器（`App_Inventory_InventoryControl`）：管理玩家的物品和库存
- 主页控制器（`App_Home_HomeControl`）：管理游戏的主界面

## 开发指南

### 环境设置

1. 安装Unity（推荐版本：[填写推荐的Unity版本]）
2. 克隆项目到本地
3. 使用Unity打开项目文件夹

### 添加新功能

1. 在适当的Script子文件夹中创建新的脚本
2. 如需添加新的控制器，请遵循现有控制器的模式，并在`SFStaticsControl`中注册

### 构建与运行

[在此处添加构建和运行项目的说明]

## 调试

游戏默认启用了Unity日志（`Debug.unityLogger.logEnabled = true`），可以通过Unity控制台查看日志信息。

## 贡献指南

[在此处添加贡献指南]

## 许可证

[在此处添加许可证信息]