# DapperEx

基于 Lambda表达式、Dapper 实现的，轻量级仓储模式ORM。

### 特点：
1，使用表达式写条件、排序，使用简单，提高工作效率；

2，支持 Select 出部分字段匿名类型；

3，支持部分字段更新

4，支持根据条件更新、删除

5，支持枚举字段检索

6，支持事务

### 安装程序包：

使用 SqlServer:

dotnet add package xLiAd.DapperEx.Repository

使用 Mysql:

dotnet add package xLiAd.DapperEx.RepositoryMysql

使用 PostgreSQL:

dotnet add package xLiAd.DapperEx.RepositoryPg

### 使用方法：

1，定义模型类：

```csharp
public class Model
{
	[Identity]
	[Key]
	public int Id { get; set; }

	public string Name { get; set; }

	public int Otherfields { get; set; }
}
```

2，实例化仓储：

```csharp
var repository = new Repository<Model>(Conn);
```
或者
```csharp
var repository = new RepositoryMysql<Model>(Conn);
```
或者
```csharp
var repository = new RepositoryPg<Model>(Conn);
```

3，执行增删改查等操作：

```csharp
repository.Add(new Model(){ Name = "ModelName" });

repository.Delete(x => x.Name == "ModelName");
repository.Delete(2);//主键

repository.Update(new Model(){ Id = 2, Name = "NewModelName", Otherfields = 3});
repository.UpdateWhere(x => x.Id == 2 || x.Id == 3, x => x.Name, "NewModelName");

var ids = new int[] { 1, 2 };
repository.Where(x => ids.Contains(x.Id));
repository.Where(x => x.Id == 2, x => x.Id, x => x.Name); //选择部分字段
repository.PageList(x => x.Id > 2, x => x.Id, 1, 10, true);

// And so on ...
```

### 历史记录：

2.2.0 解决日志重复的问题；
2.1.9 解决 !xxx.Contains(x.pro) 的问题；
2.1.8 解决 updatewhere 时 参数类型不一致时的问题；
2.1.7 修改了参数调试时不显示的BUG，把SetSql方法开放给子类使用；
2.1.5 解决 distinct 时的一个 bug；
2.1.4 支持 IEnumerable 类型参数和此类型参数的空值；
2.1.3 支持父类中定义的属性；
2.1.2 某些情况下 reader 没有关闭的 bug 修复；
2.1.1 DiagnosticLog 加开关，并测试了 record 的表现；
2.1.0 加了几个 Distinct 方法，增加了事务延迟决定的机制，增加了本地模型转换器(Respository.UseLocalParser 不使用Dapper的)；
2.0.0 DiagnosticLog 改造为标准的开始、成功、异常三种日志。
