软件配置说明

1.DataExport.exe.config内的<appSettings>节点
a)apiHost - 文件上传服务器的域名或IP
b)apiUrl - 接收文件传送的目标API相对路径(请勿更改）
c)adminEmail - 提示邮件的收件人，可以是单个收件人或多个收件人。配置为多个收件人时，请用逗号将收件人邮箱地址隔开。

2.mail.xml
此文件为错误邮件发送信箱配置，如非必要请勿更改。
a)Host - SMTP服务器地址
b)Port - 端口
c)RequireCredentials - 是否需要权限验证
d)UserName - 邮箱用户名，仅当c)为true时需要配置此项
d)Password - 邮箱密码，仅当c)为true时需要配置此项
e)Sender - 发件信息的email地址

3. job.xml
此文件为导出数据的相关配置，每个<job>节点为一个任务
a)ID - 任务编号。多个job并存时，请为每个job指定唯一编号。
b)Source - 数据来源代号。此代号为人工命名，方便识别数据来源。
c)SourceName - 数据来源名称。此名称用于在提示邮件中标识数据来源。
d)Prefix - 导出文件名前缀。建议使用 [数据来源代号] + [数据表名称] 的形式。多个job并存时，请保证Prefix的唯一性。
e)DbType - 源数据库类型，两个值可选:mssql/oracle。
f)ConnectionString - 数据库连接字符串。
g)Query - 查询语句。可根据情况任意指定mssql/oracle支持的查询语句(单表或多表查询)，保证查询结果是一个二维表即可。


如何运行程序？
启动DataExport.exe即可。如出现未找到或未安装.NET Framework相关的错误提示，请前往以下地址下载.NET Framework4.0。.NET Framework4.0支持WINDOWS Server 2003 SP2以上或WINDOWS XP SP3以上系统。
https://www.microsoft.com/zh-cn/download/details.aspx?id=17718