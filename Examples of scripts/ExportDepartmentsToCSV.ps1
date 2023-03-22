# выгрузка списка подразделений Яндекс 360 в CSV файл
$OrgID = "ID организации"
$APIToken = "Токен доступа к API"
$CSVPath = "Путь к CSV файлу"
Import-Module Y360Management
Connect-Y360 -OrgId $OrgID -APIToken $APIToken
Get-Departments| Select-object -Property id, email, name, @{label="aliases"; expression={$_.aliases -Join ','}}, membersCount, description, externalId, headId, label, parentId, createdAt |	Export-Csv -Path $CSVPath