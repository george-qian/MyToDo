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

### 使用UnitOfWork [链接](https://github.com/Arch/UnitOfWork)，进行数据库仓储管理
- 新建类库项目MyToDo.shared，并将其添加为MyToDo.Api的项目引用。
```
# 添加项目引用
在MyToDo.Api工程中，右键[依赖项]，选择[添加项目引用...],选择MyToDo.shared
```
- 在Github上下载代码，并将UnitOfWork源代码拷贝到工程中，将Collections目录中PagedList接口和实现移动到MyToDo.Shared工程中。将PagedList实现从internal改为public。
- Nuget安装`Microsoft.EntityFrameworkCore.AutoHistory`