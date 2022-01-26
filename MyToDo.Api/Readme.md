### 使用entityframework管理sqlite数据库
- Nuget安装`Microsoft.EntityFrameworkCore.Sqlite`
- Nuget安装`Microsoft.EntityFrameworkCore.Tools`
- Nuget安装`Microsoft.EntityFrameworkCore.Design`

把API项目设置为启动项目，可执行Tools的如下命令：
```
# 在<工具>-<NuGet包管理器>-<程序包管理控制台>中执行：

Enables these commonly used commands:
Add-Migration
Bundle-Migration
Drop-Database
Get-DbContext
Get-Migration
Optimize-DbContext
Remove-Migration
Scaffold-DbContext
Script-Migration
Update-Database
```

生成迁移文件
```
# 首先，设置启动项目为MyToDo.Api，然后执行
PM> Add-Migration MyToDo -project MyToDo.Api
Build started...
Build succeeded.
Microsoft.EntityFrameworkCore.Infrastructure[10403]
      Entity Framework Core 6.0.1 initialized 'MyToDoContext' using provider 'Microsoft.EntityFrameworkCore.Sqlite:6.0.1' with options: None
```
创建数据库
```
PM> Update-Database -project MyToDo.Api
Build started...
Build succeeded.
...
Done.
```
