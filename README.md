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