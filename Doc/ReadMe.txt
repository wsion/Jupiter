�������˵��

- ������������Job

1.DataExport.exe.config�ڵ�<appSettings>�ڵ�
a)apiHost - �ļ��ϴ���������������IP
b)apiUrl - �����ļ����͵�Ŀ��API���·��(������ģ�
c)adminEmail - ��ʾ�ʼ����ռ��ˣ������ǵ����ռ��˻����ռ��ˡ�����Ϊ����ռ���ʱ�����ö��Ž��ռ��������ַ������

2.mail.xml
���ļ�Ϊ�����ʼ������������ã���Ǳ�Ҫ������ġ�
a)Host - SMTP��������ַ
b)Port - �˿�
c)RequireCredentials - �Ƿ���ҪȨ����֤
d)UserName - �����û���������c)Ϊtrueʱ��Ҫ���ô���
d)Password - �������룬����c)Ϊtrueʱ��Ҫ���ô���
e)Sender - ������Ϣ��email��ַ

3. job.xml
���ļ�Ϊ�������ݵ�������ã�ÿ��<job>�ڵ�Ϊһ������
a)ID - �����š����job����ʱ����Ϊÿ��jobָ��Ψһ��š�
b)Source - ������Դ���š��˴���Ϊ�˹�����������ʶ��������Դ��
c)SourceName - ������Դ���ơ���������������ʾ�ʼ��б�ʶ������Դ��
d)Prefix - �����ļ���ǰ׺������ʹ�� [������Դ����] + [���ݱ�����] ����ʽ�����job����ʱ���뱣֤Prefix��Ψһ�ԡ�
e)DbType - Դ���ݿ����ͣ�����ֵ��ѡ:mssql/oracle��
f)ConnectionString - ���ݿ������ַ�����
g)Query - ��ѯ��䡣�ɸ����������ָ��mssql/oracle֧�ֵĲ�ѯ���(��������ѯ)����֤��ѯ�����һ����ά���ɡ�


������г���
����DataExport.exe���ɡ������δ�ҵ���δ��װ.NET Framework��صĴ�����ʾ����ǰ�����µ�ַ����.NET Framework4.0��.NET Framework4.0֧��WINDOWS Server 2003 SP2���ϻ�WINDOWS XP SP3����ϵͳ��
https://www.microsoft.com/zh-cn/download/details.aspx?id=17718


- ����ʵʱ�������
1.��װ����
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe [execution path]
2.ж�ط���
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /u [execution path]
3.���ݿ��Broker Service
ALTER  DATABASE  [JUPITER]  SET  ENABLE_BROKER;