# выгрузка списка пользователей Яндекс 360 в CSV файл
$OrgID = "ID организации"
$APIToken = "Токен доступа к API"
$CSVPath = "Путь к CSV файлу"
Import-Module Y360Management
Connect-Y360 -OrgId $OrgID -APIToken $APIToken
Get-Users | Select-object -Property id, nickname, departmentId,	email, name, gender, position, avatarId, about,	birthday,@{label="contacts"; expression={$_.contacts -Join ','}}, @{label="aliases"; expression={$_.aliases -Join ','}}, @{label="groups"; expression={$_.groups -Join ','}}, externalId, isAdmin, isRobot,	isDismissed, isEnabled,	timezone, language,	createdAt, updatedAt |	Export-Csv -Path $CSVPath