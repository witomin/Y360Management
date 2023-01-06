# Y360Management
Модуль PowerShell для управления структурой организацииЯндекс 360 для бизнеса
На основе библиотеки [Yandex.API360](https://github.com/witomin/Yandex.API360)

## Командлеты:

### **Connect-Y360** - подключиться к API Яндекс 360

#### Синтаксис:

    Connect-Y360 [-OrgId] <string> [-APIToken] <string> [<CommonParameters>]

#### Параметры:
+ OrgId. Обязательный параметр.  Задает идентификатор организации 
+ APIToken. Обязательный параметр. Задает токен авторизации в API Яндекс 360.  Для получения токена ознакомьтесь с [официальной документацией Яндекса](https://yandex.ru/dev/api360/doc/concepts/access.html)

### **Get-AllowList** - получить информацию о белом списке
#### Синтаксис:

    Get-AllowList [<CommonParameters>]

#### Результат: 
Список разрешенных IP-адресов и CIDR-подсетей.

### **Set-AllowList** - редактировать белый список
#### Синтаксис:

    Set-AllowList [[-Items] <StringCollection>] [[-AllowList] <List[string]>] [<CommonParameters>]

#### Параметры:
+ Items. Управление списком разрешенных IP-адресов и CIDR-подсетей. Для добавления адресов в список передайте параметр как показано в следующем примере:

        Set-AllowList -Items @{add="xxx.xxx.xxx.xxx, yyy.yyy.yyy.yyy"}

    Для удаления адресов из списка передайте параметр как показано в следующем примере:

        Set-AllowList -Items @{remove="xxx.xxx.xxx.xxx, yyy.yyy.yyy.yyy"}
    
    
+ AllowList. Задает список разрешенных IP-адресов и CIDR-подсетей. Нельзя использовать совместно с параметром Items.

### **Remove-AllowList** - удалить белый список
#### Синтаксис:

        Remove-AllowList [<CommonParameters>]


### **Get-Status2FA** - получить статус 2FA
#### Синтаксис:

    Get-Status2FA [<CommonParameters>]

#### Результат:

    duration enabled enabledAt
    -------- ------- ---------
       0   False

+ duration. Период (в секундах), в течение которого при включенной 2FA пользователю в процессе авторизации предлагается настроить 2FA с возможностью пропустить этот шаг. По истечении периода возможность отложить настройку 2FA отключается.
+ enabled. Статус обязательной 2FA: true — включена; false — выключена.
+ enabledAt. Время включения 2FA.

### **New-Department** - создать подразделение
#### Синтаксис:

    New-Department [-Name] <string> [[-Description] <string>] [[-ExternalId] <string>] [[-Head] <string>] [[-Label] <string>] [[-Parent] <string>] [<CommonParameters>]

#### Параметры:
+ Name. Обязательный параметр. Название подразделения.
+ Description. Описание подразделения.
+ ExternalId. Произвольный внешний идентификатор подразделения.
+ Head. Сотрудник-руководител подразделения. Можно использовать любое значение, которое однозначно определяет сотрудника: Id, Nickname, Email.
+ Label. Имя почтовой рассылки подразделения. Например, для адреса new-department@ваш-домен.ru имя почтовой рассылки — это new-department.
+ Parent. Родительское подразделение. Можно использовать любое значение, которое однозначно определяет подразделение: Id, Email.
#### Результат:
Возвращает созданное подразделение.

### **Get-Departments** - получение информации о  подразделениях
#### Синтаксис:

    Get-Departments [[-Identity] <string>] [<CommonParameters>]


#### Параметры:
+ Identity. Обязательный параметр. Определяет подразделение, которое требуется просмотреть. Можно использовать любое значение, которое однозначно определяет подразделение: Id, email.
#### Результат:
Возвращает подразделение или NULL, если подразделение не найдено.

### **Set-Department** - изменить информацию о  подразделении
#### Синтаксис:

    Set-Department [-Identity] <string> [[-Description] <string>] [[-ExternalId] <string>] [[-Head] <string>] [[-Label] <string>] [[-Name] <string>] [[-Parent] <string>] [[-Aliases] <StringCollection>] [[-AliasList] <List[string]>] [<CommonParameters>]

#### Параметры:
+ Identity. Обязательный параметр. Определяет подразделение, которое требуется изменить. Можно использовать любое значение, которое однозначно определяет подразделение: Id, email.
+ Description. Описание подразделения.
+ ExternalId. Произвольный внешний идентификатор подразделения.
+ Head. Сотрудник-руководител подразделения.
+ Label. Имя почтовой рассылки подразделения. Например, для адреса new-department@ваш-домен.ru имя почтовой рассылки — это new-department.
+ Name. Название подразделения.
+ Parent. Родительское подразделение. Можно использовать любое значение, которое однозначно определяет подразделение: Id, Email.
+ Aliases. Параметр для управления алиасами. Для добавления алиаса передайте параметр в виде:
        
        -Alises @{add="alias"}

    Для удаления алиаса передайте параметр в виде:

        -Alises @{remove="alias"}

+ AliasList. Список алиасов, не работает совместно с параметром Aliases
#### Результат:
Возвращает измененное подразделение или NULL, если подразделение не найдено.

### **Remove-Department** - удалить подразделение
#### Синтаксис:

    Remove-Department [-Identity] <string> [<CommonParameters>]

#### Параметры:
+ Identity.Обязательный параметр. Определяет подразделение, которое требуется изменить. Можно использовать любое значение, которое однозначно определяет подразделение: Id, email.

### **New-User** - создать сотрудника
#### Синтаксис:

    New-User [-NickName] <string> [-FirstName] <string> [-LastName] <string> [[-MiddleName] <string>] [-Password] <string> [[-About] <string>] [[-Birthday] <string>] [[-Contacts] <List[BaseContact]>] [-DepartmentId] <ulong> [[-ExternalId] <string>] [[-Gender] <string>] [-isAdmin] [[-Language] <string>] [[-Position] <string>] [[-Timezone] <string>] [<CommonParameters>]

#### Параметры:
+ NickName
+ FirstName
+ LastName
+ MiddleName
+ Password 
+ About
+ Birthday
+ Contacts
+ DepartmentId
+ ExternalId
+ Gender
+ isAdmin
+ Language
+ Position
+ Timezone

### **Get-Users** - получить информацию о сотрудниках
#### Синтаксис:

    Get-Users [[-Identity] <string>] [[-ResultSize] <int>] [-EnableOnly] [-DisableOnly] [<CommonParameters>]

#### Параметры:
+ Identity
+ ResultSize
+ EnableOnly
+ DisableOnly

### **Set-User** - изменение информации о сотруднике
#### Синтаксис:

    Set-User [-Identity] <string> [[-About] <string>] [[-Birthday] <string>] [[-Contacts] <List[BaseContact]>] [[-DepartmentId] <ulong>] [[-ExternalId] <string>] [[-Gender] <string>] [[-isAdmin] <bool>] [-Enable] [-Disable] [[-Language] <string>] [[-FirstName] <string>] [[-LastName] <string>] [[-MiddleName] <string>] [[-Password] <string>] [[-Position] <string>] [[-Timezone] <string>] [[-Aliases] <StringCollection>] [<CommonParameters>]

#### Параметры:
+ Identity
+ About
+ Birthday
+ Contacts
+ DepartmentId
+ ExternalId
+ Gender
+ isAdmin
+ Enable
+ Disable
+ Language
+ FirstName
+ LastName
+ MiddleName
+ Password
+ Position
+ Timezone
+ Aliases

### **Get-Status2FAUser** - Просмотреть статус 2FA сотрудника
#### Синтаксис:

    Get-Status2FAUser [-Identity] <string> [<CommonParameters>]

#### Параметры:
+ Identity

### **New-Group** - создать новую группу
#### Синтаксис:

    New-Group [-Name] <string> [[-Description] <string>] [[-ExternalId] <string>] [[-Label] <string>] [[-Admins] <List[string]>] [[-Members] <List[string]>] [<CommonParameters>]

#### Параметры:
+ Name
+ Description
+ ExternalId
+ Label
+ Admins
+ Members 

### **Get-Groups** - получение информации о группах
#### Синтаксис:

    Get-Groups [[-Identity] <string>] [<CommonParameters>]

#### Параметры:
+ Identity

### **Get-GroupMembers** - получение информации об участниках группы
#### Синтаксис:

    Get-GroupMembers [-Identity] <string> [<CommonParameters>]

#### Параметры:
+ Identity

### **Set-Group** - изменить параметры группы
#### Синтаксис:

    Set-Group [-Identity] <string> [[-Name] <string>] [[-Description] <string>] [[-ExternalId] <string>] [[-Members] <StringCollection>] [[-MemberList] <List[string]>] [[-Admins] <StringCollection>] [[-AdminList] <List[string]>] [<CommonParameters>]

#### Параметры:
+ Identity
+ Name
+ Description
+ ExternalId
+ Members
+ MemberList
+ Admins
+ AdminList

### **Remove-Group** - изменить параметры группы
#### Синтаксис:

    Remove-Group [-Identity] <string> [<CommonParameters>]

#### Параметры:
+ Identity