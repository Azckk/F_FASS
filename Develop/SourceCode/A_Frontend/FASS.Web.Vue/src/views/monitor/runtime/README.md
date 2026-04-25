# webMstscs使用方法

 1. 下载webMstscs.bat，放到D盘根目录下

 2. 修改注册表，

- 手动操作步骤：
打开注册表编辑器：
按 Win + R 键，输入 regedit，然后按回车，打开注册表编辑器。
备份注册表（可选）：
在左上角的菜单中选择 文件 -> 导出，以防止意外错误修改注册表，可以创建一个备份。
创建新的键和值：
根据路径 HKEY_CLASSES_ROOT\webMstsc，逐级创建新的键。
右键点击 HKEY_CLASSES_ROOT，选择 新建 -> 键，输入 webMstsc。
在 webMstsc 中，双击默认值 @，输入 "URL:webMstsc Protocol Handler"。
右键点击 webMstsc，选择 新建 -> 字符串值，命名为 URL Protocol，不需要为它赋值。
重复上述步骤，逐级创建子键 DefaultIcon、shell、shell\open 和 shell\open\command。
在 DefaultIcon 键下，将默认值 @ 设置为 "D:\\webMstsc.bat"。
在 command 键下，将默认值 @ 设置为 "\"D:\\webMstsc.bat\" \"%1\""。

- 命令行操作步骤：
1.创建 .reg 文件：
打开记事本，将以下内容粘贴进去：

``` reg
Windows Registry Editor Version 5.00

[HKEY_CLASSES_ROOT\webMstsc]
@="URL:webMstsc Protocol Handler"
"URL Protocol"=""

[HKEY_CLASSES_ROOT\webMstsc\DefaultIcon]
@="D:\\webMstsc.bat"

[HKEY_CLASSES_ROOT\webMstsc\shell]

[HKEY_CLASSES_ROOT\webMstsc\shell\open]

[HKEY_CLASSES_ROOT\webMstsc\shell\open\command]
@="\"D:\\webMstsc.bat\" \"%1\""

```

2.保存文件：
保存为 add-webMstsc.reg 文件，确保文件扩展名为 .reg 而不是 .txt。
3.导入注册表：
双击保存的 .reg 文件，系统会提示确认更改注册表，点击“是”导入。
4.验证结果：
打开注册表编辑器，导航到 HKEY_CLASSES_ROOT\webMstsc，检查是否成功创建了所有键和值。

注意事项：
在执行 webMstsc:// 协议时，D:\webMstsc.bat 文件需要存在，并且可以被执行。
如果文件路径中有空格，确保使用双引号将路径包裹起来，例如 "\"D:\\My Folder\\webMstsc.bat\" \"%1\""。
这样你就可以通过自定义协议在浏览器或其他应用中调用这个 .bat 文件了。
