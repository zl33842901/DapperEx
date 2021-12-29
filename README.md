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

### ��ʷ��¼��

2.2.0 �����־�ظ������⣻
2.1.9 ��� !xxx.Contains(x.pro) �����⣻
2.1.8 ��� updatewhere ʱ �������Ͳ�һ��ʱ�����⣻
2.1.7 �޸��˲�������ʱ����ʾ��BUG����SetSql�������Ÿ�����ʹ�ã�
2.1.5 ��� distinct ʱ��һ�� bug��
2.1.4 ֧�� IEnumerable ���Ͳ����ʹ����Ͳ����Ŀ�ֵ��
2.1.3 ֧�ָ����ж�������ԣ�
2.1.2 ĳЩ����� reader û�йرյ� bug �޸���
2.1.1 DiagnosticLog �ӿ��أ��������� record �ı��֣�
2.1.0 ���˼��� Distinct �����������������ӳپ����Ļ��ƣ������˱���ģ��ת����(Respository.UseLocalParser ��ʹ��Dapper��)��
2.0.0 DiagnosticLog ����Ϊ��׼�Ŀ�ʼ���ɹ����쳣������־��
