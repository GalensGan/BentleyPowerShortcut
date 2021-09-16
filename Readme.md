# MS超级快捷键功能使用说明

## 安装

1. 将编译的 `PowerShortcut.dll` 拷贝至 `C:\Program Files\Bentley\OpenRoads Designer CONNECT Edition\OpenRoadsDesigner\Mdlapps` 目录中。

2. 在 `C:\Users\%username%\AppData\Local\Bentley\OpenRoadsDesigner\10.0.0` 中建立 `shortcutsConfig.json` 文本文件（可以直接复制路径到资源管理器中打开）。然后将下面的模板复制到这个配置文件中。

3. 加载快捷键

   ![ptBMTQSJZvqUjGa.png](https://i.loli.net/2021/08/24/ptBMTQSJZvqUjGa.png)

4. 初始化超级快捷键

   请注意，初始化后，会覆盖原来的空格弹出功能。

   快捷键加载完成，按 Enter 或者 F9 弹出 keyin 窗体，在里面输入 `power install`,然后按 Enter 确认。

5. 添加快捷键

   按 空格 弹出快捷键输入框，输入 `config`，按空格确认，此时会打开快捷键定义文件，在里面进行快捷键定义，编辑完成后，保存关闭。再输入快捷键 `reload` 即可使用自己定义的快捷键了。

## 使用

1. 打开空格，如果安装成功，会弹出快捷键输入窗体：

   ![](https://i.loli.net/2021/08/24/s9TlypXKogYv4NZ.png)

2. 在里面输入定义的快捷键，按 Enter 或者 空格 响应命令

## 系统快捷键/按键

| 序号 | 快捷键  | 作用                               |
| ---- | ------- | ---------------------------------- |
| 1    | install | 安装快捷键，会原来空格键的弹出菜单 |
| 2    | 空格    | 激活快捷键输入窗体或者执行快捷键   |
| 3    | Enter   | 执行快捷键                         |
| 4    | config  | 打开快捷键定义文件                 |
| 5    | reload  | 重装加载快捷键文件                 |
| 6    | 上箭头  | 查看上一个命令                     |
| 7    | 下箭头  | 查看下一个命令                     |



## 快捷键定义

**模板：**

``` json
{
    "shortcuts":[
        {
            "names":["l"],
            "keyin":"place smartline",
            "description":"绘制直线"
        },
		{
            "names":["d","delete"],
            "keyin":"delete element",
            "description":"删除元素"
        },
		{
            "names":["c"],
            "keyin":"place circle icon",
            "description":"绘制圆弧"
        },
		{
            "names":["cc"],
            "keyin":"place circle center",
            "description":"通过圆心绘制圆弧"
        },
		{
            "names":["cd"],
            "keyin":"place circle diameter",
            "description":"通过直径绘制圆弧"
        },
		{
            "names":["m"],
            "keyin":"move icon",
            "description":"移动"
        },
		{
            "names":["co"],
            "keyin":"copy icon",
            "description":"复制"
        },
		{
            "names":["mi"],
            "keyin":"mirror icon",
            "description":"镜像"
        },
		{
            "names":["o"],
            "keyin":"MOVE PARALLEL OFFSET",
            "description":"偏移"
        },
		{
            "names":["tr"],
            "keyin":"trim multiple",
            "description":"修剪"
        },
		{
            "names":["te"],
            "keyin":"trim Extend",
            "description":"延长"
        },
		{
            "names":["b"],
            "keyin":"trim break",
            "description":"打断"
        },
		{
            "names":["ti"],
            "keyin":"trim tointersection",
            "description":"剪切到交点"
        },
		{
            "names":["reload"],
            "keyin":"power reloadConfig",
            "description":"重新加载快捷键"
        },
		{
            "names":["default"],
            "keyin":"model active default",
            "description":"打开/关闭 default model"
        },
		{
            "names":["modeltest"],
            "keyin":"model active ModelTest",
            "description":"打开/关闭 ModelTest model"
        },
		{
            "names":["model"],
            "keyin":"MDL KEYIN MODELMANAGER MODEL DIALOG TOGGLE",
            "description":"打开/关闭 model 管理界面"
        },
		{
            "names":["ref"],
            "keyin":"MDL KEYIN REF DIALOG REFERENCE TOGGLE",
            "description":"打开 model 管理界面"
        },
		{
            "names":["sw"],
            "keyin":"ribbon setworkflow swTools",
            "description":"打开 swTools 界面"
        },
		{
            "names":["drawing"],
            "keyin":"ribbon setworkflow drawing",
            "description":"打开 drawing 界面"
        },
		{
            "names":["skp"],
            "keyin":"snap keypoint",
            "description":"捕捉关键点"
        },
		{
            "names":["mref"],
            "keyin":"reference merge",
            "description":"合并参考文件"
        },
		{
            "names":["tk"],
            "keyin":"BOQAddin InsertDrawingBorder",
            "description":"图框"
        },
		{
            "names":["config"],
            "keyin":"power openConfig",
            "description":"打开快捷键配置文件"
        },
		{
            "names":["di"],
            "keyin":"measure distance",
            "description":"测量距离"
        },
		{
            "names":["acs"],
            "keyin":"MDL KEYIN BENTLEY.VIEWATTRIBUTESDIALOG,VAD VIEWATTRIBUTESDIALOG SETATTRIBUTE 0 ACSTriad True",
            "description":"打开 ACS"
        },
    ]
}
```

**单个快捷键定义：**

``` json
{
    "names":["d","delete"], // 可以设置多个快捷键
    "keyin":"delete element", // 快捷键对应的 keyin 命令
    "description":"删除元素" // 快捷键的描述
}, // 每一个定义都要以英文逗号结尾
```
