# Custom Https Dev Env API Project

[YouTube - Custom HTTPS Dev Environment using .NET Core, Kestrel & certificates](https://www.youtube.com/watch?v=96KHOaIe19w)

## Packages Installation


## Usefull Installations
### Dev Https Certificates
```bash
dotnet dev-certs https --trust
```
### Add domain name into hosts
```bash
127.0.0.1 weather.io
```
### Add https certificates to the new domain name
open powershell as administrator and ...
```bash
$cert = New-SelfSignedCertificate -certstorelocation cert:\localmachine\my -dns weather.io
```
```bash
$pwd = ConvertTo-SecureString -String "pa55w0rd!" -Force -AsPlainText
```
```bash
$certpath = "Cert:\localmachine\my\$($cert.Thumbprint)"
```
```bash
Export-PfxCertificate -Cert $certpath -FilePath c:\LocalDevHttpsCertificates\Weather.pfx -Password $pwd
```
(Win + R cert) to open certificates goto Trusted Root Certification Authorities righ click All Tasks -> Import and goto c:\LocalDevHttpsCertificates\Weather.pfx 

```bash
dotnet user-secrets set "CertPassword" "pa55w0rd!"
```
Kestrel certificate passwords are in -> C:\Users\\{user}\AppData\Roaming\Microsoft\UserSecrets

## Set Running Environment
```bash
dotnet run --environment "Development"
dotnet run --environment "Production"
``` 