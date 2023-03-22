# выгрузка списка групп Яндекс 360 в CSV файл
$OrgID = "ID организации"
$APIToken = "Токен доступа к API"
$CSVPath = "Путь к CSV файлу"
Import-Module Y360Management
Connect-Y360 -OrgId $OrgID -APIToken $APIToken
Get-Groups| Select-object -Property id, type, membersCount, email, @{label="aliases"; expression={$_.aliases -Join ','}}, removed, authorId, @{label="memberOf"; expression={$_.memberOf -Join ','}}, description, name, externalId, @{label="members"; expression={$_.members -Join ','}}, @{label="adminIds"; expression={$_.adminIds -Join ','}}, createdAt |	Export-Csv -Path $CSVPath