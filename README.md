# DapperEx

���� Lambda���ʽ��Dapper ʵ�ֵģ��������ִ�ģʽORM��

### �ص㣺
1��ʹ�ñ��ʽд����������ʹ�ü򵥣���߹���Ч�ʣ�

2��֧�� Select �������ֶ��������ͣ�

3��֧�ֲ����ֶθ���

4��֧�ָ����������¡�ɾ��

5��֧��ö���ֶμ���

6��֧������

### ��װ�������

ʹ�� SqlServer:

dotnet add package xLiAd.DapperEx.Repository

ʹ�� Mysql:

dotnet add package xLiAd.DapperEx.RepositoryMysql

ʹ�� PostgreSQL:

dotnet add package xLiAd.DapperEx.RepositoryPg

### ʹ�÷�����

1������ģ���ࣺ

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

2��ʵ�����ִ���

```csharp
var repository = new Repository<Model>(Conn);
```
����
```csharp
var repository = new RepositoryMysql<Model>(Conn);
```
����
```csharp
var repository = new RepositoryPg<Model>(Conn);
```

3��ִ����ɾ�Ĳ�Ȳ�����

```csharp
repository.Add(new Model(){ Name = "ModelName" });

repository.Delete(x => x.Name == "ModelName");
repository.Delete(2);//����

repository.Update(new Model(){ Id = 2, Name = "NewModelName", Otherfields = 3});
repository.UpdateWhere(x => x.Id == 2 || x.Id == 3, x => x.Name, "NewModelName");

var ids = new int[] { 1, 2 };
repository.Where(x => ids.Contains(x.Id));
repository.Where(x => x.Id == 2, x => x.Id, x => x.Name); //ѡ�񲿷��ֶ�
repository.PageList(x => x.Id > 2, x => x.Id, 1, 10, true);

// And so on ...
```