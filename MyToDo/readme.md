### 创建项目
- 创建wpf应用，选择.Net6。
### 导入prism框架
- 在NuGet包管理中，安装prism.dryioc。并<b>重新生成工程！</b>为了工程能索引到框架资源。
```
# 在App.xaml中添加prism的命名空间
<prism:PrismApplication x:Class="MyToDo.App"
...
    xmlns:prism="http://prismlibrary.com/"
    # 删除StartupUri，由prism框架启动
    <!--StartupUri="MainWindow.xaml"-->
...
</prism:PrismApplication>
```
- 在App.xaml.cs中修改App的父类为，并实现其成员函数。
```
using Prism.DryIoc;
...
public partial class App : PrismApplication
{
    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
    }
}
```
### 导入MaterialDesign UI库
- 在NuGet包管理中，安装materialdesignthemes。
- 参考github帮助[链接](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit/wiki/Super-Quick-Start)，修改App.xaml。
```
...
# 添加materialDesign命名空间
xmlns:materialDesign="http://materialdesigninxaml.net/winfx/xaml/themes">
# 添加主题资源
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <materialDesign:BundledTheme BaseTheme="Light" PrimaryColor="DeepPurple" SecondaryColor="Lime" />
            <ResourceDictionary Source="pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml" />
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
...
```
- 在MainWindow.xaml中添加一个button，验证启动，成功！