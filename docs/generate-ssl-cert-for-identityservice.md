# Generate SSL Cert for IdentityService

Generate the public private keypair:

```sh
§ openssl genrsa -aes256 -passout pass:Abc123! -out server.pass.key 4096
...
§ openssl rsa -passin pass:Abc123! -in server.pass.key -out server.key
writing RSA key
rm server.pass.key
§ openssl req -new -key server.key -out server.csr -config <(cat ../server.cnf)
...
Country Name (2 letter code) [AU]:US
State or Province Name (full name) [Some-State]:California
...
A challenge password []:
...
```

Sign the SSL certificate:

```sh
$ openssl x509 -req -extensions v3_req -sha256 -days 365 -in server.csr -signkey server.key -out server.crt -extfile ../server.cnf
```

On macOS, trust the cert:

```sh
sudo security add-trusted-cert -d -r trustRoot -k /Library/Keychains/System.keychain <<certificate>>
```