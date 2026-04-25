<h1>vue-pure-admin精简版（国际化版本）</h1>

[![license](https://img.shields.io/github/license/pure-admin/vue-pure-admin.svg)](LICENSE)

**中文** | [English](./README.en-US.md)

## 介绍

精简版是基于 [vue-pure-admin](https://github.com/pure-admin/vue-pure-admin) 提炼出的架子，包含主体功能，更适合实际项目开发，打包后的大小在全局引入 [element-plus](https://element-plus.org) 的情况下仍然低于 `2.3MB`，并且会永久同步完整版的代码。开启 `brotli` 压缩和 `cdn` 替换本地库模式后，打包大小低于 `350kb`

## 版本选择

当前是国际化版本，如果您需要非国际化版本 [请点击](https://github.com/pure-admin/pure-admin-thin)

## 配套视频

[点我查看 UI 设计](https://www.bilibili.com/video/BV17g411T7rq)  
[点我查看快速开发教程](https://www.bilibili.com/video/BV1kg411v7QT)

## 配套保姆级文档

[点我查看 vue-pure-admin 文档](https://pure-admin.github.io/pure-admin-doc)  
[点我查看 @pureadmin/utils 文档](https://pure-admin-utils.netlify.app)

## 优质服务、软件外包、赞助支持

[点我查看详情](https://pure-admin.github.io/pure-admin-doc/pages/service/)

## 预览

[查看预览](https://pure-admin-thin.netlify.app/#/login)

## 维护者

[xiaoxian521](https://github.com/xiaoxian521)

## ⚠️ 注意

精简版不接受任何 `issues` 和 `pr`，如果有问题请到完整版 [issues](https://github.com/pure-admin/vue-pure-admin/issues/new/choose) 去提，谢谢！

## 许可证

[MIT © 2020-present, pure-admin](./LICENSE)

## 2024.11.27 更新日志

## 开发环境要求

- Node.js = 18.19.1  推荐使用nvm管理node版本，[安装教程](https://github.com/nvm-sh/nvm)
- npm = 10.2.4
- pnpm >= 9.1.1   安装命令：npm install -g pnpm

## 安装

```bash
# 克隆项目
git clone https://gitee.com/Fairyland_1/frcs

# 进入项目目录
cd xxx

# 安装依赖 不要更新依赖，否则会导致依赖版本不兼容
pnpm install

# 启动服务
pnpm dev
## 之后去vite.config.ts中修改配置
## ip地址修改为你后端项目的ip地址，默认127.0.0.1

# 构建生产环境
pnpm build
```
