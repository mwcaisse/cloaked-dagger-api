#!/bin/bash
#https://github.com/dotnet/runtime/issues/44535

openssl req -x509 -newkey rsa:4096 -sha256 -nodes -keyout cert.key -out cert.crt -subj "/CN=localhost" -days 3650

openssl pkcs12 -export -out cert.pfx -inkey cert.key -in cert.crt

cat cert.pfx | base64 > cert.pfx.b64
